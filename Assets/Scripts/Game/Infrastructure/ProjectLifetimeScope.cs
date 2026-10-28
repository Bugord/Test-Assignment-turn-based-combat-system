using UIKit;
using UIKit.VContainer;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;

namespace Game
{
    public class ProjectLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private PanelSettings panelSettings;
        
        [SerializeField]
        private ScreenCatalog screenCatalog;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.AddUIKit(panelSettings, screenCatalog);
        }
    }
}