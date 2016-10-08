using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[AddComponentMenu("UI/Tween Alpha")]
public class TweenAlpha : TweenerBase
{
    [Range(0f, 1f)]
    public float from = 0f;
    [Range(0f, 1f)]
    public float to = 1f;

    Graphic graphic;
    CanvasGroup canvasGroup;

    protected override void Initialize()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        graphic = GetComponent<Graphic>();

        if (canvasGroup == null && graphic == null)
        {
            graphic = GetComponentInChildren<Graphic>();
            if (graphic.GetComponent<CanvasGroup>() == null)
            {
                canvasGroup = canvasGroup.gameObject.AddComponent<CanvasGroup>();
            }
        }
		
		if (canvasGroup != null)
		{
			tweener = canvasGroup.DOFade(to, duration);
		}
		else if (graphic != null)
		{
			tweener = graphic.DOFade(to, duration);
		}
    }

	protected override void ResetFrom ()
	{
		if (canvasGroup != null)
		{
			canvasGroup.alpha = from;
		}
		else if (graphic != null)
		{
			graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, from);
		}
	}

    public Graphic cachedGraphic { get { if (graphic == null) graphic = GetComponent<Graphic>(); return graphic; } }
    public CanvasGroup cachedCanvasGroup
    {
        get
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();

            if (canvasGroup == null)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();

            return canvasGroup;
        }
    }

    public float value
    {
        get
        {
            if (cachedCanvasGroup != null)
            {
                return cachedCanvasGroup.alpha;
            }
            else if (cachedGraphic != null)
            {
                return cachedGraphic.color.a;
            }

            return 1;
        }
    }

    [ContextMenu("Set 'From' to current value")]
    public override void SetStartToCurrentValue() { from = value; }

    [ContextMenu("Set 'To' to current value")]
    public override void SetEndToCurrentValue() { to = value; }
}