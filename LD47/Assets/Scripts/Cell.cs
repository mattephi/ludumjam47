using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

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
            die();
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

    public int getNeighbourIndex(Direction direct)
    {
        switch (direct)
        {
            case Direction.UpLeft: return 0;
            case Direction.Up: return 1;
            case Direction.UpRight: return 2;
            case Direction.Right: return 3;
            case Direction.DownRight: return 4;
            case Direction.Down: return 5;
            case Direction.DownLeft: return 6;
            case Direction.Left: return 7;
            default:
                UnityEngine.Debug.Log("Unexpected input");
                return -1;
        }
    }

    public GameObject[] borders = new GameObject[4];
    public int bordersCount = 0;

    struct borderCheckElem
    {
        public int mask;
        public int controlVal;
    }
    borderCheckElem[] borderCheck = 
    {
        new borderCheckElem { mask =  0b_1100_0001, controlVal = 0b_1000_0000}, //a1
        new borderCheckElem { mask =  0b_0111_0000, controlVal = 0b_0010_0000}, //a3
        new borderCheckElem { mask =  0b_0001_1100, controlVal = 0b_0000_1000}, //a5
        new borderCheckElem { mask =  0b_0000_0111, controlVal = 0b_0000_0010}, //a7
                
        new borderCheckElem { mask =  0b_0101_0000, controlVal = 0b_0101_0000}, //c24
        new borderCheckElem { mask =  0b_0100_0001, controlVal = 0b_0100_0001}, //c28
        new borderCheckElem { mask =  0b_0001_0100, controlVal = 0b_0001_0100}, //c46
        new borderCheckElem { mask =  0b_0000_0101, controlVal = 0b_0000_0101}, //c68

        new borderCheckElem { mask =  0b_0101_0101, controlVal = 0b_0101_0100}, //d246
        new borderCheckElem { mask =  0b_0101_0101, controlVal = 0b_0101_0001}, //d248
        new borderCheckElem { mask =  0b_0101_0101, controlVal = 0b_0100_0101}, //d268
        new borderCheckElem { mask =  0b_0101_0101, controlVal = 0b_0001_0101}, //d468
        
        new borderCheckElem { mask =  0b_0101_0001, controlVal = 0b_0100_0000}, //l2
        new borderCheckElem { mask =  0b_0101_0100, controlVal = 0b_0001_0000}, //l4
        new borderCheckElem { mask =  0b_0001_0101, controlVal = 0b_0000_0100}, //l6
        new borderCheckElem { mask =  0b_0100_0101, controlVal = 0b_0000_0001}, //l8
    };

    void drawBorders()
    {
        int i_identifier = 0;
        foreach (KeyValuePair<Direction, Cell> item in NeighborCells)
        {
            UnityEngine.Debug.Log(item.Value.MyState);
            if (item.Value.MyState == State.Resource || item.Value.MyState == State.Surface || item.Value.MyState == State.Bonus)
                i_identifier += 1 << (7 - getNeighbourIndex(item.Key));
        }
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        UnityEngine.Debug.Log(i_identifier);
        #region drawing broders

        foreach(var item in borderCheck.Select((value, i) => new { i, value }))
        {
            if ((i_identifier & item.value.mask) == item.value.controlVal)
            {
                borders[bordersCount] = new GameObject();
                borders[bordersCount].transform.parent = gameObject.transform;
                borders[bordersCount].transform.position = gameObject.transform.position;
                borders[bordersCount].AddComponent<SpriteRenderer>().sprite = borderSprites[item.i];
                UnityEngine.Debug.Log(borders[bordersCount]);
                bordersCount++;
            }
        }
        #endregion 
    }

    void die() // Debug purposes
    {
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
        drawBorders();

        foreach (var item in NeighborCells)
        {
            UnityEngine.Debug.Log("Obj: " + item.Value.gameObject + "\nState: " + item.Value.MyState);
            if (item.Value.MyState == State.Transition || item.Value.MyState == State.Deadly)
            {
                item.Value.drawBorders();
            }
        }
    }

    void OnMouseDown() // Debug purposes
    {
        die();
    }
}