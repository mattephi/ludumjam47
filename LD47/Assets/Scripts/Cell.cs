// using System;
// using System.Collections;
using System.Collections.Generic;
// using System.Diagnostics;
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
    public State myState;

    [SerializeField] private float maxCellHp = 100f;
    private const float MINCellHp = 0f;
    private float _curHp;

    public Bonus myBonus;
    public Resource myResource;

    #endregion

    private void OnEnable()
    {
        switch (myState)
        {
            case State.Bonus:
            case State.Surface:
                _curHp = maxCellHp;
                break;
            case State.Resource:
                _curHp = myResource.value * maxCellHp;
                break;
            default:
                _curHp = maxCellHp;
                break;
        }
    }

    public void GetDamage(Character character)
    {
        if (_curHp - character.curDamage > 0)
        {
            _curHp -= character.curDamage;
        }
        else
        {
            _curHp = 0f;
            Die();
        }
    }

    public bool IsExist(Direction direction)
    {
        return (NeighborCells.TryGetValue(direction, out var neighbourCell) && !ReferenceEquals(neighbourCell, null));
    }

    public bool IsAvailable(Direction direction)
    {
        return NeighborCells[direction].myState != State.Deadly;
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

    void drawBorders()
    {
        int i_identifier = 0;
        foreach (KeyValuePair<Direction, Cell> item in NeighborCells)
        {
            UnityEngine.Debug.Log(item.Value.myState);
            if (item.Value.myState == State.Deadly || item.Value.myState == State.Transition)
                i_identifier += 1 << getNeighbourIndex(item.Key);
        }
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        UnityEngine.Debug.Log(i_identifier);

        #region drawing broders
        //if (i_identifier ~ 0b_1000_0011 == 0b_10000000)
        #endregion 
    }

    void Die() // Debug purposes
    {
        drawBorders();
        switch (myState)
        {
            //break / return to the player
            case State.Bonus:
                break;
            case State.Surface:
                break;
            case State.Resource:
                break;
        }
        myState = State.Transition;
    }

    void OnMouseDown() // Debug purposes
    {
        Die();
    }
}