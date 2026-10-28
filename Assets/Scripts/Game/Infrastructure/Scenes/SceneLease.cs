using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Infrastructure.Scenes
{
    public class SceneLease : IAsyncDisposable
    {
        private readonly string sceneName;
        private Scene previousActiveScene;
        private bool disposed;

        private SceneLease(string sceneName)
        {
            this.sceneName = sceneName;
            previousActiveScene = SceneManager.GetActiveScene();
        }

        public Scene Scene => SceneManager.GetSceneByName(sceneName);

        public static async UniTask<SceneLease> LoadAsync(string sceneName, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            
            if (SceneManager.GetSceneByName(sceneName).isLoaded) {
                throw new InvalidOperationException($"Scene '{sceneName}' is already loaded.");
            }

            var lease = new SceneLease(sceneName);
            try {
                var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                if (operation == null) {
                    throw new InvalidOperationException($"Could not load '{sceneName}'.");
                }

                await operation;
                token.ThrowIfCancellationRequested();

                var scene = lease.Scene;
                if (!scene.IsValid() || !scene.isLoaded) {
                    throw new InvalidOperationException($"Could not load '{sceneName}'.");
                }

                SceneManager.SetActiveScene(scene);
                return lease;
            }
            catch {
                await lease.DisposeAsync();
                throw;
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (disposed) {
                return;
            }

            disposed = true;
            if (previousActiveScene.IsValid() && previousActiveScene.isLoaded) {
                SceneManager.SetActiveScene(previousActiveScene);
            }

            var scene = Scene;
            if (!scene.IsValid() || !scene.isLoaded) {
                return;
            }

            var operation = SceneManager.UnloadSceneAsync(scene);
            if (operation != null) {
                await operation;
            }
        }
    }
}