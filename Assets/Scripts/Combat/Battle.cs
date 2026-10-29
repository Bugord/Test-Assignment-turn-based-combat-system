using System;
using System.Collections.Generic;
using System.Linq;
using Combat.Abilities;
using Combat.Rules;
using Combat.Units;

namespace Combat
{
    public class Battle
    {
        private readonly BattleRules rules;
        private readonly List<Unit> queue = new List<Unit>();

        public Group Player { get; }
        public Group Enemy { get; }
        public int Round { get; private set; }
        public Unit ActiveUnit { get; private set; }
        public IReadOnlyList<Unit> RemainingQueue => queue;

        public Team? Winner {
            get {
                if (!Enemy.HasAlive) {
                    return Team.Player;
                }
                if (!Player.HasAlive) {
                    return Team.Enemy;
                }
                return null;
            }
        }

        public Battle(Group player, Group enemy, BattleRules rules)
        {
            if (player.Team != Team.Player) {
                throw new ArgumentException("Expected the player group.", nameof(player));
            }
            
            if (enemy.Team != Team.Enemy) {
                throw new ArgumentException("Expected the enemy group.", nameof(enemy));
            }

            Player = player;
            Enemy = enemy;
            this.rules = rules ?? throw new ArgumentNullException(nameof(rules));

            AdvanceTurn();
        }

        public Group AlliesOf(Unit unit) => unit.Team == Team.Player ? Player : Enemy;
        public Group OpponentsOf(Unit unit) => unit.Team == Team.Player ? Enemy : Player;

        public IReadOnlyList<Ability> AvailableAbilities(Unit actor)
        {
            return rules.AvailableAbilities(actor, AlliesOf(actor), OpponentsOf(actor));
        }

        public IReadOnlyList<Unit> ValidTargets(Unit actor, Ability ability)
        {
            return rules.AvailableTargets(actor, ability, AlliesOf(actor), OpponentsOf(actor));
        }

        public ActionResult Execute(Decision decision)
        {
            if (Winner != null) {
                throw new InvalidOperationException($"The battle is over, {Winner} won.");
            }
            
            if (decision.Actor != ActiveUnit) {
                throw new ArgumentException($"It's {ActiveUnit}'s turn, not {decision.Actor}'s.", nameof(decision));
            }

            var allies = AlliesOf(decision.Actor);
            var opponents = OpponentsOf(decision.Actor);
            rules.Validate(decision, allies, opponents);
            var result = rules.Apply(decision, allies, opponents);

            AdvanceTurn();
            return result;
        }

        private void AdvanceTurn()
        {
            if (Winner != null) {
                ActiveUnit = null;
                queue.Clear();
                return;
            }

            queue.RemoveAll(unit => !unit.IsAlive);
            if (queue.Count == 0) {
                StartRound();
            }

            ActiveUnit = queue[0];
            queue.RemoveAt(0);
        }

        private void StartRound()
        {
            Round++;

            queue.AddRange(Player.Alive
                .Concat(Enemy.Alive)
                .OrderByDescending(unit => unit.Stats.Speed)
                .ThenBy(unit => unit.Team == Team.Player ? 0 : 1)
                .ThenBy(unit => unit.Slot));
        }
    }
}
