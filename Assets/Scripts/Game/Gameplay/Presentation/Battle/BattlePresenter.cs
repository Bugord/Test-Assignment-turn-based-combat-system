using System;
using System.Linq;
using System.Threading;
using Combat.Abilities;
using Combat.Rules;
using Combat.Units;
using Cysharp.Threading.Tasks;

namespace Game.Gameplay.Presentation
{
    public class BattlePresenter : IBattlePresentation
    {
        private readonly TimeSpan turnStartDelay = TimeSpan.FromSeconds(0.3f);
        private readonly TimeSpan skipDelay = TimeSpan.FromSeconds(0.4f);
        private readonly TimeSpan actionEndDelay = TimeSpan.FromSeconds(0.2f);
        private readonly BattleHud hud;
        private readonly UnitViews views;

        private UnitView focused;

        public BattlePresenter(BattleHud hud, UnitViews views)
        {
            this.hud = hud ?? throw new ArgumentNullException(nameof(hud));
            this.views = views ?? throw new ArgumentNullException(nameof(views));
        }

        public async UniTask ShowTurnStartAsync(Unit actor, CancellationToken token)
        {
            hud.SetActiveUnit(actor);
            Focus(views[actor]);
            await UniTask.Delay(turnStartDelay, cancellationToken: token);
        }

        public async UniTask PlayAsync(ActionResult result, CancellationToken token)
        {
            var actor = views[result.Actor];
            var motion = MotionOf(result.Ability);

            await PlayWindupAsync(actor, motion, result.Target, token);
            await UniTask.WhenAll(result.Outcomes.Select(outcome => PlayOutcomeAsync(result.Ability.Effect, outcome, token)));
            await PlayRecoverAsync(actor, motion, token);

            hud.AddAction(result);
            await UniTask.Delay(actionEndDelay, cancellationToken: token);
        }

        public UniTask ShowBattleEndAsync(Team winner, CancellationToken token)
        {
            Focus(null);
            hud.SetWinner(winner);
            return UniTask.CompletedTask;
        }

        private AbilityMotion MotionOf(Ability ability)
        {
            if (ability.Effect == EffectType.None) {
                return AbilityMotion.None;
            }

            return ability.RequiresFrontline ? AbilityMotion.Melee : AbilityMotion.Ranged;
        }

        private UniTask PlayWindupAsync(UnitView actor, AbilityMotion motion, Unit target, CancellationToken token)
        {
            switch (motion) {
                case AbilityMotion.None:
                    return UniTask.Delay(skipDelay, cancellationToken: token);
                case AbilityMotion.Melee:
                    return actor.PlayLungeAsync(views[target], token);
                case AbilityMotion.Ranged:
                    return actor.PlayShotAsync(token);
                default:
                    throw new ArgumentOutOfRangeException(nameof(motion), motion, null);
            }
        }

        private UniTask PlayRecoverAsync(UnitView actor, AbilityMotion motion, CancellationToken token)
        {
            return motion == AbilityMotion.Melee ? actor.PlayReturnAsync(token) : UniTask.CompletedTask;
        }

        private async UniTask PlayOutcomeAsync(EffectType effect, TargetOutcome outcome, CancellationToken token)
        {
            var view = views[outcome.Target];
            if (!outcome.Hit) {
                await view.PlayDodgeAsync(token);
                return;
            }

            view.SetHp(outcome.HpAfter, outcome.Target.Stats.MaxHp);
            await (effect == EffectType.Heal ? view.PlayHealAsync(token) : view.PlayHitAsync(token));

            if (outcome.Died) {
                await view.PlayDeathAsync(token);
            }
        }

        private void Focus(UnitView view)
        {
            if (focused != null) {
                focused.SetFocused(false);
            }

            focused = view;

            if (focused != null) {
                focused.SetFocused(true);
            }
        }
    }
}
