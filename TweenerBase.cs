using UnityEngine;
using System.Collections;
using DG.Tweening;

public abstract class TweenerBase : MonoBehaviour {
    public bool autoPlay = false;
    public bool autoKill = true;

    public float delay = 0f;
    public float duration = 1f;

	public Ease ease = Ease.Linear;

	public int loops = 1;
	public LoopType loopType = LoopType.Restart;

	protected Tweener tweener;
	private bool isForward;

	protected TweenParams CommonTweenParams {
		get {
			return new TweenParams().SetEase(ease)
					.SetLoops(loops, loopType)
					.SetDelay(delay)
                    .SetAutoKill(autoKill);
		}
	}											

    void Start () {
        Initialize();

        if (autoPlay) {
            isForward = !isForward;
        } else
        {
            tweener.Pause();
        }
    }
	
	protected virtual void Initialize() { }
	protected virtual void ResetFrom() { }

	public void Play(bool forward = true, TweenCallback onCompleteCallback = null, TweenCallback onRewindCallback = null) {
        if (!this.enabled)
        {
            // onCompleteCallback?.Invoke();
            if (onCompleteCallback != null)
            {
                onCompleteCallback();
            }

            return;
        }

		ResetFrom();

        var newTweenParams = CommonTweenParams;

        if (onCompleteCallback != null)
        {
            newTweenParams.OnComplete(onCompleteCallback);
        }

        if (onRewindCallback != null)
        {
            newTweenParams.OnRewind(onRewindCallback);
        }

        tweener.SetAs(newTweenParams);

        if (forward) {
			tweener.PlayForward();
		} else {
			tweener.PlayBackwards();
		}

		isForward = forward;
	}

    public void Toggle(TweenCallback onCompleteCallback = null, TweenCallback onRewindCallback = null)
    {
        Play(!isForward, onCompleteCallback, onRewindCallback);
    }

    public virtual void SetStartToCurrentValue() { }

    public virtual void SetEndToCurrentValue() { }
}