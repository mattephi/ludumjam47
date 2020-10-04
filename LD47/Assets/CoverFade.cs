using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverFade : MonoBehaviour
{
    public float fadeInTime;
    public float stayTime;
    public float fadeOutTime;
    float opacity = 0;
    float startTime;
    enum stages
    {
        fadeIn,
        stay,
        fadeOut,
        disappear
    }
    stages state = stages.fadeIn;

    void Start()
    {
        this.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        startTime = Time.time;
    }


    void Update()
    {
        if (Time.time > startTime + fadeInTime + stayTime + fadeOutTime)
            state = stages.disappear;
        else if (Time.time > startTime + fadeInTime + stayTime)
            state = stages.fadeOut;
        else if (Time.time > startTime + fadeInTime)
            state = stages.stay;
        else if (Time.time > startTime)
            state = stages.fadeIn;

        switch (state)
        {
            case stages.fadeIn:
                opacity = (Time.time - startTime) / fadeInTime;
            break;
            case stages.stay:
                opacity = 1;
            break;
            case stages.fadeOut:
                opacity = 1 - (Time.time - startTime - fadeInTime - stayTime) / fadeOutTime;
            break;
            case stages.disappear:
                Destroy(gameObject);
            break;
        }
        this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, opacity);
        Debug.Log(state + "\n" + opacity);
    }
}
