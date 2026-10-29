using System;
using System.Collections.Generic;

namespace Game.Gameplay.Presentation
{
    public class BattleLog
    {
        private readonly List<string> lines = new List<string>();

        public IReadOnlyList<string> Lines => lines;

        public event Action Changed;

        public void Add(string line)
        {
            lines.Add(line ?? throw new ArgumentNullException(nameof(line)));
            Changed?.Invoke();
        }
    }
}
