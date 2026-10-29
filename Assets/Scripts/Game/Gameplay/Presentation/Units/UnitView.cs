using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Gameplay.Presentation
{
    public class UnitView : MonoBehaviour
    {
        [SerializeField]
        private UnitAnimator animator;

        [SerializeField]
        private UnitHealth health;

        [SerializeField]
        private GameObject focusMarker;

        private void Awake()
        {
            focusMarker.SetActive(false);
        }

        public void Setup(Sprite sprite, bool facingLeft)
        {
            animator.Setup(sprite, facingLeft);
            health.Setup();
        }

        public void SetFocused(bool focused)
        {
            focusMarker.SetActive(focused);
        }

        public UniTask PlayLungeAsync(UnitView target, CancellationToken token) => animator.PlayLungeAsync(target.transform.position, token);
        public UniTask PlayShotAsync(CancellationToken token) => animator.PlayShotAsync(token);
        public UniTask PlayReturnAsync(CancellationToken token) => animator.PlayReturnAsync(token);
        public UniTask PlayHitAsync(CancellationToken token) => animator.PlayHitAsync(token);
        public UniTask PlayDodgeAsync(CancellationToken token) => animator.PlayDodgeAsync(token);
        public UniTask PlayHealAsync(CancellationToken token) => animator.PlayHealAsync(token);
        public void SetHp(int hp, int maxHp) => health.SetHp(hp, maxHp);

        public UniTask PlayDeathAsync(CancellationToken token)
        {
            focusMarker.SetActive(false);
            health.Hide();
            return animator.PlayDeathAsync(token);
        }
    }
}