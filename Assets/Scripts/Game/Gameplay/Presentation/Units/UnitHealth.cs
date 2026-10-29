using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Gameplay.Presentation
{
    public class UnitHealth : MonoBehaviour
    {
        [SerializeField]
        private UIDocument hpDocument;
        
        private VisualElement hpFill;

        public void Setup()
        {
            hpFill = hpDocument.rootVisualElement.Q("hp-fill");
        }
        
        public void SetHp(int hp, int maxHp)
        {
            hpFill.style.width = Length.Percent(100f * hp / maxHp);
        }

        public void Hide()
        {
            hpDocument.gameObject.SetActive(false);
        }
    }
}