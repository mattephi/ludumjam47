using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Cell : MonoBehaviour
{
    #region Initialization
    public Sprite[] borderSprites;
    public Dictionary<State, Sprite> StateSprites = new Dictionary<State, Sprite>();

    [SerializeField] private Sprite _surfaceSprite;
    [SerializeField] private Sprite _transitionSprite;
    [SerializeField] private Sprite _deadlySprite;
    
    public SpriteRenderer MainSpriteRenderer;
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
    public Cell(State myState)
    {
        initDict();
        MyState = myState;
        MainSpriteRenderer = GetComponent<SpriteRenderer>();
        MainSpriteRenderer.sprite = StateSprites[MyState];
    }
    
    public Cell(State myState, Bonus myBonus)
    {
        initDict();
        MyBonus = myBonus;
        MyState = myState;
        MainSpriteRenderer = GetComponent<SpriteRenderer>();
        MainSpriteRenderer.sprite = StateSprites[MyState];
    }
    
    public Cell(State myState, Resource myResource)
    {
        initDict();
        MyResource = myResource;
        MyState = myState;
        MainSpriteRenderer = GetComponent<SpriteRenderer>();
        MainSpriteRenderer.sprite = StateSprites[MyState];
    }

    void initDict()
    {
        StateSprites[State.Surface] = _surfaceSprite;
        StateSprites[State.Transition] = _transitionSprite;
        StateSprites[State.Deadly] = _deadlySprite;
    }
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

    void drawBorders()
    {
        int i_identifier = 0;
        foreach (KeyValuePair<Direction, Cell> item in NeighborCells)
        {
            UnityEngine.Debug.Log(item.Value.MyState);
            if (item.Value.MyState == State.Deadly || item.Value.MyState == State.Transition)
                i_identifier += 1 << getNeighbourIndex(item.Key);
        }
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        UnityEngine.Debug.Log(i_identifier);

        #region drawing broders
        //if (i_identifier ~ 0b_1000_0011 == 0b_10000000)
        #endregion 
    }

    void die() // Debug purposes
    {
        drawBorders();
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

    void OnMouseDown() // Debug purposes
    {
        die();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && MyState == State.Deadly)
        {
            other.GetComponent<Character>().Die();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && MyState == State.Transition)
        {
            MyState = State.Deadly;
        }
    }
}