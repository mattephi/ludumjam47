using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    #region Initialization
    public Dictionary<Character.Direction, Cell> NeighborCells;
    
    public enum State
    {
        Resource,
        Surface,
        Bonus,
        //Obstacle,
        Transition,
        Deadly
    }
    public State MyState;

    [SerializeField] private float MaxCellHp = 100f;
    private const float minCellHp = 0f;
    private float curHp;
    
    public Bonus MyBonus;
    public Resource MyResource;

    #endregion
    private void OnEnable()
    {
        switch (MyState)
        {
            case State.Bonus:
            case State.Surface:
                curHp = MaxCellHp;
                break;
            case State.Resource:
                curHp = MyResource.value * MaxCellHp;
                break;
            default:
                curHp = MaxCellHp;
                break;
        }
    }

    public void GetDamage(Character character)
    {
        if (curHp - character.CurDamage > 0)
        {
            curHp -= character.CurDamage;
        }
        else
        {
            curHp = 0f;
            MyState = State.Transition;
        }
    }
    
    public bool IsExist(Character.Direction direction)
    {
        Cell neighborcell;
        return (NeighborCells.TryGetValue(direction, out neighborcell) && !ReferenceEquals(neighborcell, null));
    }
    
    public bool IsAvailable(Character.Direction direction)
    {
        return NeighborCells[direction].MyState != State.Deadly;
    }
    
    
    
    
    
}
