using System.Threading;
using Cysharp.Threading.Tasks;
using UniState;
using UnityEngine;

namespace Game.App.States
{
    public class ClosingAppState : StateBase
    {
        public override UniTask<StateTransitionInfo> Execute(CancellationToken token)
        {
            Debug.Log("Closing application...");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

            return UniTask.FromResult(Transition.GoToExit());
        }
    }
}
