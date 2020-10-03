using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite _sprite;
    public int value = 1;
    [SerializeField] private Sprite[] SpriteArr;

    private void OnValidate()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _sprite = SpriteArr[value - 1];
        spriteRenderer.sprite = _sprite;
    }
    
}
