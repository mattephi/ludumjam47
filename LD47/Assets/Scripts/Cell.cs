using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Cell : MonoBehaviour
{
    #region Initialization
    public Sprite[] borderSprites;
    private Dictionary<State, Sprite> _stateSprites = new Dictionary<State, Sprite>();

    [SerializeField] private Sprite surfaceSprite;
    [SerializeField] private Sprite transitionSprite;
    [SerializeField] private Sprite deadlySprite;
    
    public SpriteRenderer mainSpriteRenderer;
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
    
    public readonly Dictionary<Direction, Cell> NeighborCells = new Dictionary<Direction, Cell>();

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
    public Cell(State myState)
    {
        InitDict();
        this.myState = myState;
        mainSpriteRenderer = GetComponent<SpriteRenderer>();
        mainSpriteRenderer.sprite = _stateSprites[this.myState];
    }
    
    public Cell(State myState, Bonus myBonus)
    {
        InitDict();
        this.myBonus = myBonus;
        this.myState = myState;
        mainSpriteRenderer = GetComponent<SpriteRenderer>();
        mainSpriteRenderer.sprite = _stateSprites[this.myState];
    }
    
    public Cell(State myState, Resource myResource)
    {
        InitDict();
        this.myResource = myResource;
        this.myState = myState;
        mainSpriteRenderer = GetComponent<SpriteRenderer>();
        mainSpriteRenderer.sprite = _stateSprites[this.myState];
    }

    void InitDict()
    {
        _stateSprites[State.Surface] = surfaceSprite;
        _stateSprites[State.Transition] = transitionSprite;
        _stateSprites[State.Deadly] = deadlySprite;
    }
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
        return (NeighborCells.TryGetValue(direction, out var neighborcell) && !ReferenceEquals(neighborcell, null));
    }

    public bool IsAvailable(Direction direction)
    {
        return NeighborCells[direction].myState != State.Deadly;
    }

    private int getNeighbourIndex(Direction direct)
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

    void DrawBorders()
    {
        int iIdentifier = 0;
        foreach (KeyValuePair<Direction, Cell> item in NeighborCells)
        {
            UnityEngine.Debug.Log(item.Value.myState);
            if (item.Value.myState == State.Deadly || item.Value.myState == State.Transition)
                iIdentifier += 1 << getNeighbourIndex(item.Key);
        }
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        UnityEngine.Debug.Log(iIdentifier);

        #region drawing broders
        //if (i_identifier ~ 0b_1000_0011 == 0b_10000000)
        #endregion 
    }

    void Die() // Debug purposes
    {
        DrawBorders();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && myState == State.Deadly)
        {
            other.GetComponent<Character>().Die();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && myState == State.Transition)
        {
            myState = State.Deadly;
        }
    }
}