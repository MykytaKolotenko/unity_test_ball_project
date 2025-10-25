using UnityEngine;
using Zenject;

namespace Projectile
{
    public class ProjectileViewFactory : PlaceholderFactory<ProjectileView>
    {
        public ProjectileView Create(Transform parent, Vector3 position, float radius)
        {
            ProjectileView view = base.Create();

            view.transform.SetParent(parent, false);
            view.transform.localPosition = position;
            view.transform.SetSiblingIndex(int.MaxValue);
            view.Init(radius);

            return view;
        }
    }
}
