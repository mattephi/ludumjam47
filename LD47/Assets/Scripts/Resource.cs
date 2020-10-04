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
    public float coefficient = 1.05f;

    public void Enable(Character character)
    {
        character.curDamage *= coefficient;
        character.curCell.RemoveResBonSurf();
        character.curCell.SetState(Cell.State.Transition);
    }
}
