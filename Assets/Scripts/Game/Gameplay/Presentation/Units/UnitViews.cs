using System;
using System.Collections.Generic;
using System.Linq;
using Combat;
using Combat.Units;
using Game.Gameplay.Config;
using Object = UnityEngine.Object;

namespace Game.Gameplay.Presentation
{
    public class UnitViews : IDisposable
    {
        private readonly Dictionary<Unit, UnitView> views = new Dictionary<Unit, UnitView>();

        public UnitViews(Battle battle, BattleStage stage, BattleLineup lineup)
        {
            foreach (var unit in battle.Player.Units.Concat(battle.Enemy.Units)) {
                var view = stage.Spawn(unit.Team, unit.Slot);
                var unitConfig = lineup.UnitConfigAt(unit.Team, unit.Slot);
                view.Setup(unitConfig.Sprite, unit.Team == Team.Enemy);
                view.SetHp(unit.Hp, unit.Stats.MaxHp);
                views.Add(unit, view);
            }
        }

        public UnitView this[Unit unit] => views[unit];

        public void Dispose()
        {
            foreach (var view in views.Values) {
                if (view != null) {
                    Object.Destroy(view.gameObject);
                }
            }

            views.Clear();
        }
    }
}