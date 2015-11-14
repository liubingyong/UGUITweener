using UnityEngine;
using System.Collections;

using DG.Tweening;

public class TweenScale : TweenerBase {

	public Vector3 from = Vector3.one;
	public Vector3 to = Vector3.one;

	private RectTransform rectTransform;

	protected override void Initialize ()
	{
		rectTransform = GetComponent<RectTransform>();

		tweener = rectTransform.DOScale(to, duration).SetAs(CommonTParms);
	}

	protected override void ResetFrom ()
	{
		rectTransform.localScale = from;
	}
}
