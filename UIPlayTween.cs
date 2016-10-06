using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UIPlayTween : MonoBehaviour {

	public enum Direction
	{
		Reverse = -1,
		Toggle = 0,
		Forward = 1,
	}

	public GameObject tweenTarget;
	public bool includeChildren = false;

	public Direction playDirection = Direction.Forward;

	public EventTriggerType eventTriggerType = EventTriggerType.PointerClick;

	TweenerBase[] tweens;

	// Use this for initialization
	void Start () {
		EventTrigger trigger = GetComponentInParent<EventTrigger>();

		if (trigger == null) {
			trigger = gameObject.AddComponent<EventTrigger>();
		}

		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = eventTriggerType;
		entry.callback.AddListener( (eventData) => { Play(true); } );
		trigger.triggers.Add(entry);
	}

	public void Play (bool forward) {
		var go = (tweenTarget == null) ? gameObject : tweenTarget;

		if (playDirection == Direction.Reverse) forward = !forward;

		tweens = includeChildren ? go.GetComponentsInChildren<TweenerBase>() : go.GetComponents<TweenerBase>();

		for (int i = 0, imax = tweens.Length; i < imax; ++i) {
			var tween = tweens[i];

			if (playDirection == Direction.Toggle) {
				tween.Toggle();
			} else {
				tween.Play(forward);
			}
		}
	}
}
