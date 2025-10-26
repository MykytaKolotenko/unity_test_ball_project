using Configs;
using UnityEngine;
using Utils;
using Zenject;

public abstract class AbstractCircleView : MonoBehaviour
{
    [SerializeField] protected RectTransform circleTransform;

    [Inject] protected CircleAnimationConfig CircleAnimationConfig;

    public Vector3 LocalPosition => circleTransform.localPosition;

    public float Radius { get; protected set; }

    protected Vector3 StartPosition { get; private set; }

    private void Awake()
    {
        StartPosition = circleTransform.position;
    }

    public void Init(float radius)
    {
        SetRadius(radius);
    }

    public void SetRadius(float radius)
    {
        Radius = radius;
        UpdateView();
    }

    protected void UpdateView()
    {
        circleTransform.sizeDelta = MathUtils.GetDimensionsFromRadius(Radius);
    }
}
