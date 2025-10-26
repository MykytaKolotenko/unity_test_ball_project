using UnityEngine;
using Zenject;

namespace Projectile
{
    public class ProjectileViewFactory : PlaceholderFactory<ProjectileController>
    {
        public ProjectileController Create(Transform parent, Vector3 position, float radius)
        {
            ProjectileController view = base.Create();

            view.transform.SetParent(parent, false);
            view.transform.localPosition = position;
            view.transform.SetSiblingIndex(int.MaxValue);
            view.Init(radius);

            return view;
        }
    }
}
