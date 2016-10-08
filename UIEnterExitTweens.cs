using UnityEngine;
using DG.Tweening;
using System.Collections;

public class UIEnterExitTweens : MonoBehaviour {
    public static int activeTweensCounter = 0;

    public bool isToggle = false;

    public TweenerBase[] enterTweens;
    public TweenerBase[] exitTweens;

    public void StartEnterTweens()
    {
        for (int i = 0; i < enterTweens.Length; i++)
        {
            if (!enterTweens[i].enabled)
            {
                continue;
            }

            ++activeTweensCounter;
            if (isToggle)
            {
                enterTweens[i].Toggle(
                    onCompleteCallback: () =>
                    {
                        --activeTweensCounter;
                    },
                    onRewindCallback: () =>
                    {
                        --activeTweensCounter;
                    }
                );
            }
            else
            {
                enterTweens[i].Play(
                    onCompleteCallback: () =>
                    {
                        --activeTweensCounter;
                    },
                    onRewindCallback: () =>
                    {
                        --activeTweensCounter;
                    }
                );
            }
        }
    }

    public void StartExitTweens()
    {
        for (int i = 0; i < exitTweens.Length; i++)
        {
            if (!exitTweens[i].enabled)
            {
                continue;
            }

            ++activeTweensCounter;
            if (isToggle) {
                exitTweens[i].Toggle(
                    onCompleteCallback: () =>
                    {
                        --activeTweensCounter;
                    },
                    onRewindCallback: () =>
                    {
                        --activeTweensCounter;
                    }
                );
            } else
            {
                exitTweens[i].Play(
                    onCompleteCallback: () =>
                    {
                        --activeTweensCounter;
                    },
                    onRewindCallback: () =>
                    {
                        --activeTweensCounter;
                    }
                );
            }
        }
    }
}
