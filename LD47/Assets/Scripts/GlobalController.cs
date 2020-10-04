using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
    [SerializeField] private Generator generator;
    public Character char1;
    public Character char2;
    private const float DefaultSwapTiming = 1.0f;

    void Start()
    {
        
    }

    private void Update()
    {
        char1.GetComponent<Animator>().Play("digs-down");
    }

    public void SwapCharacters()
    {
        StartCoroutine(AsyncSwap());
    }

    IEnumerator AsyncSwap(float timing = DefaultSwapTiming)
    {
        yield return new WaitForSeconds(DefaultSwapTiming);
        if (char1 && char2)
        {
            {
                var temp = char1.curCell;
                char1.curCell = char2.curCell;
                char2.curCell = temp;
            }
            {
                var temp = char1.myDirection;
                char1.myDirection = char2.myDirection;
                char2.myDirection = temp;
            }
            {
                var temp = char1.baseDirection;
                char1.baseDirection = char2.baseDirection;
                char2.baseDirection = temp;
            }
            {
                var temp = char1.transform.position;
                char1.transform.position = char2.transform.position;
                char2.transform.position = temp;
            }
        }
    }
}
