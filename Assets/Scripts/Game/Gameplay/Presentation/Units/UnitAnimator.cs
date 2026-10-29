using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Game.Gameplay.Presentation
{
    public class UnitAnimator : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer body;

        [SerializeField]
        private SpriteRenderer effect;

        [SerializeField]
        private Sprite hitEffect;

        [SerializeField]
        private Sprite healEffect;

        [SerializeField]
        private float meleeGap = 1.4f;

        [SerializeField]
        private float shotStep = 0.3f;

        [SerializeField]
        private float dodgeStep = 0.5f;

        private readonly Color hitColor = new Color(1f, 0.3f, 0.3f);
        private readonly Color healColor = new Color(0.5f, 1f, 0.5f);

        private float effectScale;
        private float facing = 1f;

        private void Awake()
        {
            effectScale = effect.transform.localScale.x;
            effect.color = Color.clear;
        }

        public void Setup(Sprite sprite, bool facingLeft)
        {
            body.sprite = sprite;
            body.flipX = facingLeft;
            facing = facingLeft ? -1f : 1f;
        }

        public UniTask PlayLungeAsync(Vector3 targetPosition, CancellationToken token)
        {
            var x = targetPosition.x - facing * meleeGap;
            return Play(body.transform.DOMoveX(x, 0.2f).SetEase(Ease.OutQuad), token);
        }

        public async UniTask PlayShotAsync(CancellationToken token)
        {
            await Play(body.transform.DOLocalMoveX(facing * shotStep, 0.1f).SetEase(Ease.OutQuad), token);
            await PlayReturnAsync(token);
        }

        public UniTask PlayReturnAsync(CancellationToken token)
        {
            return Play(body.transform.DOLocalMove(Vector3.zero, 0.2f).SetEase(Ease.InOutQuad), token);
        }

        public UniTask PlayHitAsync(CancellationToken token)
        {
            var sequence = DOTween.Sequence()
                .Append(body.DOColor(hitColor, 0.08f))
                .Join(body.transform.DOShakePosition(0.25f, 0.15f, 20))
                .Append(body.DOColor(Color.white, 0.15f));

            return UniTask.WhenAll(Play(sequence, token), PlayEffectAsync(hitEffect, Color.white, token));
        }

        public async UniTask PlayDodgeAsync(CancellationToken token)
        {
            await Play(body.transform.DOLocalMoveX(-facing * dodgeStep, 0.1f).SetEase(Ease.OutQuad), token);
            await PlayReturnAsync(token);
        }

        public UniTask PlayHealAsync(CancellationToken token)
        {
            var sequence = DOTween.Sequence()
                .Append(body.DOColor(healColor, 0.15f))
                .Append(body.DOColor(Color.white, 0.25f));

            return UniTask.WhenAll(Play(sequence, token), PlayEffectAsync(healEffect, healColor, token));
        }

        public UniTask PlayDeathAsync(CancellationToken token)
        {
            var sequence = DOTween.Sequence()
                .Append(body.transform.DOLocalRotate(new Vector3(0f, 0f, facing * 90f), 0.3f))
                .Join(body.transform.DOLocalMoveY(-0.5f, 0.3f))
                .Append(body.DOFade(0f, 0.3f));

            return Play(sequence, token);
        }

        private UniTask PlayEffectAsync(Sprite sprite, Color color, CancellationToken token)
        {
            if (sprite == null) {
                return UniTask.CompletedTask;
            }

            effect.sprite = sprite;
            effect.color = color;
            effect.transform.localScale = Vector3.one * effectScale * 0.4f;

            var sequence = DOTween.Sequence()
                .Append(effect.transform.DOScale(effectScale, 0.25f).SetEase(Ease.OutBack))
                .Append(effect.DOFade(0f, 0.2f));

            return Play(sequence, token);
        }

        private UniTask Play(Tween tween, CancellationToken token)
        {
            return tween.SetLink(gameObject).ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }
    }
}
