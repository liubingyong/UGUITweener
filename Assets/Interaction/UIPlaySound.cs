using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Plays the specified sound.
/// </summary>

[AddComponentMenu("UI/Interaction/Play Sound")]
public class UIPlaySound : MonoBehaviour
{
    public enum Trigger
    {
        OnClick,
        OnMouseOver,
        OnMouseOut,
        //OnPress,
        //OnRelease,
        //Custom,
        OnEnable,
        OnDisable,
    }

    public AudioClip audioClip;
    public Trigger trigger = Trigger.OnClick;

    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0f, 2f)]
    public float pitch = 1f;

    bool mIsOver = false;

    bool canPlay
    {
        get
        {
            if (!enabled) return false;
            var btn = GetComponent<Button>();
            return (btn == null || btn.enabled);
        }
    }

    void Awake()
    {
        switch (trigger)
        {
            case Trigger.OnClick:
                var btn = this.gameObject.GetComponent<Button>();

                if (btn != null)
                {
                    btn.onClick.AddListener(() => { OnClick(); });
                    break;
                }

                var eventTrigger = this.gameObject.GetComponent<EventTrigger>();

                if (eventTrigger == null)
                {
                    eventTrigger = this.gameObject.AddComponent<EventTrigger>();
                }

                var entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((eventData) => { OnClick(); });
                eventTrigger.triggers.Add(entry);

                break;
            case Trigger.OnMouseOver:
            case Trigger.OnMouseOut:
                eventTrigger = this.gameObject.GetComponent<EventTrigger>();

                if (eventTrigger == null)
                {
                    eventTrigger = this.gameObject.AddComponent<EventTrigger>();
                }

                entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener((eventData) => { OnHover(true); });
                eventTrigger.triggers.Add(entry);

                entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerExit;
                entry.callback.AddListener((eventData) => { OnHover(false); });
                eventTrigger.triggers.Add(entry);

                break;
            default:
                break;
        }
    }

    void OnEnable()
    {
        if (trigger == Trigger.OnEnable)
            UGUITools.PlaySound(audioClip, volume, pitch);
    }

    void OnDisable()
    {
        if (trigger == Trigger.OnDisable)
            UGUITools.PlaySound(audioClip, volume, pitch);
    }

    void OnHover(bool isOver)
    {
        if (trigger == Trigger.OnMouseOver)
        {
            if (mIsOver == isOver) return;
            mIsOver = isOver;
        }

        if (canPlay && ((isOver && trigger == Trigger.OnMouseOver) || (!isOver && trigger == Trigger.OnMouseOut)))
            UGUITools.PlaySound(audioClip, volume, pitch);
    }

    void OnPress(bool isPressed)
    {
        //if (trigger == Trigger.OnPress)
        //{
        //    if (mIsOver == isPressed) return;
        //    mIsOver = isPressed;
        //}

        //if (canPlay && ((isPressed && trigger == Trigger.OnPress) || (!isPressed && trigger == Trigger.OnRelease)))
        //    UITools.PlaySound(audioClip, volume, pitch);
    }

    void OnClick()
    {
        if (canPlay && trigger == Trigger.OnClick)
            UGUITools.PlaySound(audioClip, volume, pitch);
    }

    void OnSelect(bool isSelected)
    {
        if (canPlay && !isSelected)
            OnHover(isSelected);
    }

    public void Play()
    {
        UGUITools.PlaySound(audioClip, volume, pitch);
    }
}
