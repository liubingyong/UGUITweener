using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TweenPosition : TweenerBase {
	public Vector3 from;
	public Vector3 to;

	private RectTransform rectTransform;

	protected override void Initialize ()
	{
		rectTransform = GetComponent<RectTransform>();
        
        tweener = rectTransform
			.DOLocalMove(to, duration);
    }

	protected override void ResetFrom ()
	{
		rectTransform.localPosition = from;
	}
}