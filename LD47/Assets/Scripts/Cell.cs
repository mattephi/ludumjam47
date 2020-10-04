// using System;
// using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

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
    public void Init(State myState)
    {
        InitDict();
        this.myState = myState;
        mainSpriteRenderer = GetComponent<SpriteRenderer>();
        mainSpriteRenderer.sprite = _stateSprites[this.myState];
    }
    
    public void Init(State myState, Bonus myBonus)
    {
        InitDict();
        this.myBonus = myBonus;
        this.myState = myState;
        mainSpriteRenderer = GetComponent<SpriteRenderer>();
        mainSpriteRenderer.sprite = _stateSprites[this.myState];
    }
    
    public void Init(State myState, Resource myResource)
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

    private void AssignStates()
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

    public GameObject[] borders = new GameObject[8];
    public int bordersCount = 0;

    void drawBorders()
    {
        foreach(var item in borders)
        {
            Destroy(item);
        }
        int i_identifier = 0;
        foreach (KeyValuePair<Direction, Cell> item in NeighborCells)
        {
            UnityEngine.Debug.Log(item.Value.myState);
            if (item.Value.myState == State.Resource || item.Value.myState == State.Surface || item.Value.myState == State.Bonus)
                i_identifier += 1 << (7 - getNeighbourIndex(item.Key));
        }
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        UnityEngine.Debug.Log(i_identifier);
       
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
    }

    void Die() // Debug purposes
    {
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
        drawBorders();

        foreach (var item in NeighborCells)
        {
            UnityEngine.Debug.Log("Obj: " + item.Value.gameObject + "\nState: " + item.Value.myState);
            if (item.Value.myState == State.Transition || item.Value.myState == State.Deadly)
            {
                item.Value.drawBorders();
            }
        }
    }

    void OnMouseDown() // Debug purposes
    {
        Die();
    }

    private void OnCollisionEnter2D(Collision2D other1)
    {
        if (other1.gameObject.CompareTag("Player") && myState == State.Deadly)
        {
            other1.gameObject.GetComponent<Character>().Die();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other1)
    {
        if (other1.CompareTag("Player") && myState == State.Transition)
        {
            myState = State.Deadly;
        }
    }

}