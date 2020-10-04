using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite sprite;
    public int value = 1;
    [SerializeField] private Sprite[] spriteArr;

    private void OnValidate()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sprite = spriteArr[value - 1];
        spriteRenderer.sprite = sprite;
    }
    
}
