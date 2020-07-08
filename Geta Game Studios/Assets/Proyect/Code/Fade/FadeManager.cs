using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FadeManager : MonoBehaviour
{
    //Singleton
    public static FadeManager Instance;
    [Header("UI")] 
    public Image fade;
    
    [Header("Fade Core")]
    public float timeFade;
    private Color c;
    private IEnumerator startFade;

    [Header("Unity Events")] 
    public UnityEvent OnFinishFadeOut;
    public UnityEvent OnFinishFadeIn;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (OnFinishFadeOut == null)
        {
            OnFinishFadeOut = new UnityEvent();
        }

        if (OnFinishFadeIn == null)
        {
            OnFinishFadeIn = new UnityEvent();
        }
    }

    IEnumerator StartFadeOut()
    {
        c = fade.color;
        c.a = 1;
        while (c.a > 0)
        {
            yield return null;
            c.a = Mathf.MoveTowards(c.a, 0, Time.deltaTime/timeFade);
            fade.color = c;
        }
        OnFinishFadeOut.Invoke();
    }

    IEnumerator StartfadeIn()
    {
        c = fade.color;
        c.a = 0;
        while (c.a < 1)
        {
            yield return null;
            c.a = Mathf.MoveTowards(c.a, 1, Time.deltaTime/timeFade);
            fade.color = c;
        }
        OnFinishFadeIn.Invoke();
    }

    public void StartFade(bool fadeIn)
    {
        if (startFade != null)
        {
            StopCoroutine(startFade);
        }

        if (fadeIn)
        {
            startFade = StartfadeIn();
        }
        else
        {
            startFade = StartFadeOut();
        }
        StartCoroutine(startFade);
    }
}
