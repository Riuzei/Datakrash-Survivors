using PrimeTween;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIElementAnimator : UIAnimator
{
    [SerializeField] private bool animateAlpha = true;

    private CanvasGroup _cg;
    private Tween _alphaTween;

    protected override void Awake()
    {
        base.Awake();
        InitCanvasGroup();
    }

    private void InitCanvasGroup()
    {
        if (_cg != null) return;
        _cg = GetComponent<CanvasGroup>();
    }

    private void OnDisable()
    {
        _alphaTween.Stop();
    }

    public override void ResetState()
    {
        base.ResetState();
        InitCanvasGroup();
        if (!animateAlpha) return;
        _alphaTween.Stop();
        _cg.alpha = 0f;
        _cg.interactable = false;
        _cg.blocksRaycasts = false;
    }

    protected override void OnPlayEnter(float enterDelay = 0f)
    {
        if (!animateAlpha) return;
        _alphaTween.Stop();
        _alphaTween = Tween.Alpha(_cg, 1f, duration, ease, startDelay: enterDelay)
                           .OnComplete(() => { _cg.interactable = true; _cg.blocksRaycasts = true; });
    }

    protected override void OnPlayExit(float exitDelay = 0f)
    {
        if (!animateAlpha) return;
        _cg.interactable = false;
        _cg.blocksRaycasts = false;
        _alphaTween.Stop();
        _alphaTween = Tween.Alpha(_cg, 0f, duration, ease, startDelay: exitDelay);
    }
}
