using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TweenColor : TweenerBase {

	public Color from = Color.white;
	public Color to = Color.white;
	
	Graphic graphic;
	
	protected override void Initialize()
	{
		graphic = GetComponent<Graphic>();

		if (graphic != null)
		{
			tweener = graphic.DOColor(to, duration).SetAs(CommonTParms);
		}
	}

	protected override void ResetFrom ()
	{
		graphic.color = from;
	}
}
