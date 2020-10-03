using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


public class Cell : MonoBehaviour
{
    public enum Direction
    {
        UpLeft,
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        DownLeft,
        Left
    };

    #region Initialization
    [SerializeField] private SpriteRenderer LowerspriteRenderer; 
    [SerializeField] private SpriteRenderer UpperspriteRenderer;

    public Dictionary<Direction, Cell> NeighborCells = new Dictionary<Direction, Cell>();
    
    public enum State
    {
        Resource,
        Surface,
        Bonus,
        //Obstacle,
        Transition,
        StartingPoint,
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
            switch (MyState)
            {
                //break / return to the player
                case State.Bonus:
                    break;
                case State.Surface:
                    break;
                case State.Resource:
                    break;
            }
            MyState = State.Transition;
        }
    }
    
    public bool IsExist(Direction direction)
    {
        Cell neighborcell;
        return (NeighborCells.TryGetValue(direction, out neighborcell) && !ReferenceEquals(neighborcell, null));
    }
    
    public bool IsAvailable(Direction direction)
    {
        return NeighborCells[direction].MyState != State.Deadly;
    }

    /*
    void OnMouseDown() // Debug purposes
    {
        UnityEngine.Debug.Log(NeighborCells);
        Direction i_it = Direction.UpLeft;
        if (curHp > 0)
            curHp = 0f;
        else
        {
            Destroy(NeighborCells[i_it].gameObject);
            i_it = i_it.Next();
        }
    }
    */
}
