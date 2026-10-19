using PrimeTween;
using UnityEngine;

public abstract class UIAnimator : MonoBehaviour
{
    public enum SlideDirection { None, Up, Down, Left, Right }

    [Header("Slide")]
    [SerializeField] protected SlideDirection enterFrom = SlideDirection.Up;
    [SerializeField] protected SlideDirection exitTo = SlideDirection.Up;
    [SerializeField] protected float slideDistance = 12f;

    [Header("Timing")]
    [SerializeField] protected float duration = 0.3f;
    [SerializeField] protected Ease ease = Ease.OutCubic;

    [Header("Behaviour")]
    [SerializeField] private bool animateOnEnable = true;

    public float Duration => duration;

    protected RectTransform Rt;
    protected Vector2 RestPosition;

    private Tween _positionTween;

    protected virtual void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (Rt != null) return;
        Rt = (RectTransform)transform;
        RestPosition = Rt.anchoredPosition;
    }

    private void OnEnable()
    {
        if (!animateOnEnable) return;
        ResetState();
        PlayEnter();
    }

    public void PlayEnter(float enterDelay = 0f)
    {
        _positionTween.Stop();
        Rt.anchoredPosition = RestPosition + GetOffset(enterFrom);
        if (enterFrom != SlideDirection.None)
            _positionTween = Tween.UIAnchoredPosition(Rt, RestPosition, duration, ease, startDelay: enterDelay);
        OnPlayEnter(enterDelay);
    }

    public void PlayExit(float exitDelay = 0f)
    {
        _positionTween.Stop();
        if (exitTo != SlideDirection.None)
            _positionTween = Tween.UIAnchoredPosition(Rt, RestPosition + GetOffset(exitTo), duration, ease, startDelay: exitDelay);
        OnPlayExit(exitDelay);
    }

    public virtual void ResetState()
    {
        Init();
        _positionTween.Stop();
        Rt.anchoredPosition = RestPosition;
    }

    protected abstract void OnPlayEnter(float delay = 0f);
    protected abstract void OnPlayExit(float delay = 0f);

    private Vector2 GetOffset(SlideDirection dir) => dir switch
    {
        SlideDirection.Up => Vector2.up * slideDistance,
        SlideDirection.Down => Vector2.down * slideDistance,
        SlideDirection.Left => Vector2.left * slideDistance,
        SlideDirection.Right => Vector2.right * slideDistance,
        _ => Vector2.zero
    };
}
