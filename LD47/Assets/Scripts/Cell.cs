using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


public class Cell : MonoBehaviour
{
    public Sprite[] borderSprites;
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
    
    public bool IsExist(Cell.Direction direction)
    {
        Cell neighborcell;
        return (NeighborCells.TryGetValue(direction, out neighborcell) && !ReferenceEquals(neighborcell, null));
    }
    
    public bool IsAvailable(Cell.Direction direction)
    {
        return NeighborCells[direction].MyState != State.Deadly;
    }

    
    void OnMouseDown() // Debug purposes
    {
        int i_identifier = 0;
        int i_magnitude = 0;
        foreach (KeyValuePair<Direction,Cell> item in NeighborCells)
        {
            UnityEngine.Debug.Log(item.Value.MyState);
            if (item.Value.MyState == State.Deadly)
                i_identifier += 1 << i_magnitude;
            i_magnitude++;
        }
        UnityEngine.Debug.Log(i_identifier);
        MyState = State.Deadly;
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
    
        
    
    }
}
