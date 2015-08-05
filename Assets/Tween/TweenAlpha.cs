using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tween the object's alpha. Works with both UI widgets as well as renderers.
/// </summary>

[AddComponentMenu("UI/Tween Alpha")]
public class TweenAlpha : UGUITweener
{
    [Range(0f, 1f)]
    public float from = 1f;
    [Range(0f, 1f)]
    public float to = 1f;

    bool mCached = false;
    Graphic mGrap;
    CanvasGroup mCg;

    void Cache()
    {
        mCached = true;
        mCg = GetComponent<CanvasGroup>();
        mGrap = GetComponent<Graphic>();

        if (mCg == null && mGrap == null)
        {
            mGrap = GetComponentInChildren<Graphic>();
            if (mGrap.GetComponent<CanvasGroup>() == null)
            {
                mCg = mCg.gameObject.AddComponent<CanvasGroup>();
            }
        }
    }

    /// <summary>
    /// Tween's current value.
    /// </summary>

    public float value
    {
        get
        {
            if (!mCached) Cache();
            return mGrap != null ? mGrap.color.a : 1f;
        }
        set
        {
            if (!mCached) Cache();

            if (mCg != null)
            {
                mCg.alpha = value;
            } else if (mGrap != null)
            {
                Color c = mGrap.color;
                c.a = value;
                mGrap.color = c;
            }
        }
    }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished) { value = Mathf.Lerp(from, to, factor); }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public TweenAlpha Begin(GameObject go, float duration, float alpha)
    {
        TweenAlpha comp = UGUITweener.Begin<TweenAlpha>(go, duration);
        comp.from = comp.value;
        comp.to = alpha;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }

    public override void SetStartToCurrentValue() { from = value; }
    public override void SetEndToCurrentValue() { to = value; }
}
