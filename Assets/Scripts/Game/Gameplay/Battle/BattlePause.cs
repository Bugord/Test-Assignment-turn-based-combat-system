using System;
using UnityEngine;

namespace Game.Gameplay
{
    public class BattlePause : IDisposable
    {
        public void Pause()
        {
            Time.timeScale = 0f;
        }

        public void Resume()
        {
            Time.timeScale = 1f;
        }

        public void Dispose()
        {
            Resume();
        }
    }
}
