using UnityEngine;
using System.Collections;
using DG.Tweening;

public abstract class TweenerBase : MonoBehaviour {

    public float delay = 0f;
    public float duration = 1f;

	public Ease ease = Ease.Linear;

	public int loops = 1;
	public LoopType loopType = LoopType.Restart;

	protected Tweener tweener;
	private bool isForward;

	protected TweenParams CommonTParms {
		get {
			return new TweenParams().SetEase(ease)
					.SetLoops(loops, loopType)
					.SetDelay(delay);
		}
	}											

    void Awake()
    {
        Initialize();
    }

    void Start () {
		Play(true);
		isForward = !isForward;
	}
	
	protected virtual void Initialize() { }
	protected virtual void ResetFrom() { }

	public void Play(bool forward) {
		ResetFrom();

		if (forward) {
			tweener.PlayForward();
		} else {
			tweener.PlayBackwards();
		}

		isForward = forward;
	}

	public void PlayForward() {
		Play(true);
	}

	public void PlayReverse() {
		Play(false);
	}

	public void Toggle() {
		Play(!isForward);
	}
	
    public virtual void SetStartToCurrentValue() { }

    public virtual void SetEndToCurrentValue() { }
}