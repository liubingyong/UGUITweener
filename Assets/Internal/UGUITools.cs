using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

/// <summary>
/// Helper class containing generic functions used throughout the UI library.
/// </summary>

static public class UGUITools
{
    static AudioListener mListener;

    static bool mLoaded = false;
    static float mGlobalVolume = 1f;

    /// <summary>
    /// Globally accessible volume affecting all sounds played via NGUITools.PlaySound().
    /// </summary>

    static public float soundVolume
    {
        get
        {
            if (!mLoaded)
            {
                mLoaded = true;
                mGlobalVolume = PlayerPrefs.GetFloat("Sound", 1f);
            }
            return mGlobalVolume;
        }
        set
        {
            if (mGlobalVolume != value)
            {
                mLoaded = true;
                mGlobalVolume = value;
                PlayerPrefs.SetFloat("Sound", value);
            }
        }
    }

    /// <summary>
    /// Convenience function that converts Class + Function combo into Class.Function representation.
    /// </summary>

    static public string GetFuncName(object obj, string method)
    {
        if (obj == null) return "<null>";
        string type = obj.GetType().ToString();
        int period = type.LastIndexOf('/');
        if (period > 0) type = type.Substring(period + 1);
        return string.IsNullOrEmpty(method) ? type : type + "/" + method;
    }

    /// <summary>
    /// Helper function that returns whether the specified MonoBehaviour is active.
    /// </summary>

    [System.Diagnostics.DebuggerHidden]
    [System.Diagnostics.DebuggerStepThrough]
    static public bool GetActive(Behaviour mb)
    {
        return mb && mb.enabled && mb.gameObject.activeInHierarchy;
    }

    /// <summary>
    /// Unity4 has changed GameObject.active to GameObject.activeself.
    /// </summary>

    [System.Diagnostics.DebuggerHidden]
    [System.Diagnostics.DebuggerStepThrough]
    static public bool GetActive(GameObject go)
    {
        return go && go.activeInHierarchy;
    }

    /// <summary>
    /// Unity4 has changed GameObject.active to GameObject.SetActive.
    /// </summary>

    [System.Diagnostics.DebuggerHidden]
    [System.Diagnostics.DebuggerStepThrough]
    static public void SetActiveSelf(GameObject go, bool state)
    {
        go.SetActive(state);
    }

    /// <summary>
    /// Play the specified audio clip.
    /// </summary>

    static public AudioSource PlaySound(AudioClip clip) { return PlaySound(clip, 1f, 1f); }

    /// <summary>
    /// Play the specified audio clip with the specified volume.
    /// </summary>

    static public AudioSource PlaySound(AudioClip clip, float volume) { return PlaySound(clip, volume, 1f); }

    static float mLastTimestamp = 0f;
    static AudioClip mLastClip;

    /// <summary>
    /// Play the specified audio clip with the specified volume and pitch.
    /// </summary>

    static public AudioSource PlaySound(AudioClip clip, float volume, float pitch)
    {
        float time = Time.time;
        if (mLastClip == clip && mLastTimestamp + 0.1f > time) return null;

        mLastClip = clip;
        mLastTimestamp = time;
        volume *= soundVolume;

        if (clip != null && volume > 0.01f)
        {
            if (mListener == null || !UGUITools.GetActive(mListener))
            {
                AudioListener[] listeners = GameObject.FindObjectsOfType(typeof(AudioListener)) as AudioListener[];

                if (listeners != null)
                {
                    for (int i = 0; i < listeners.Length; ++i)
                    {
                        if (UGUITools.GetActive(listeners[i]))
                        {
                            mListener = listeners[i];
                            break;
                        }
                    }
                }

                if (mListener == null)
                {
                    Camera cam = Camera.main;
                    if (cam == null) cam = GameObject.FindObjectOfType(typeof(Camera)) as Camera;
                    if (cam != null) mListener = cam.gameObject.AddComponent<AudioListener>();
                }
            }

            if (mListener != null && mListener.enabled && UGUITools.GetActive(mListener.gameObject))
            {
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
				AudioSource source = mListener.audio;
#else
                AudioSource source = mListener.GetComponent<AudioSource>();
#endif
                if (source == null) source = mListener.gameObject.AddComponent<AudioSource>();
#if !UNITY_FLASH
                source.priority = 50;
                source.pitch = pitch;
#endif
                source.PlayOneShot(clip, volume);
                return source;
            }
        }
        return null;
    }
}
