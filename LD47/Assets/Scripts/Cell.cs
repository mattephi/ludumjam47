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
    //[SerializeField]private Dictionary<State, Sprite> _stateSprites = new Dictionary<State, Sprite>();

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
        this.myState = myState;
        SetSprite(this.myState);
    }
    
    public void Init(State myState, Bonus myBonus)
    {
        this.myBonus = myBonus;
        this.myState = myState;
        SetSprite(this.myState);
    }
    
    public void Init(State myState, Resource myResource)
    {
        this.myResource = myResource;
        this.myState = myState;
        SetSprite(this.myState);
    }
    #endregion
    
    public void SetSprite(State state)
    {
        switch (state)
        {
            case State.Surface:
                mainSpriteRenderer.sprite = surfaceSprite;
                break;
            case State.Transition:
                mainSpriteRenderer.sprite = transitionSprite;
                break;
                case State.Deadly:
                    mainSpriteRenderer.sprite = deadlySprite; 
                    break;
                default:    
                    break;
        }
        myState = state;
    }
    
    
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

    public void GetDamage(Character character, float damage)
    {
        if (_curHp - damage > 0)
        {
            _curHp -= damage;
        }
        else
        {
            _curHp = 0f;
            Die(character);
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
        public int Mask;
        public int ControlVal;
    }
    borderCheckElem[] _borderCheck = 
    {
        new borderCheckElem { Mask =  0b_1100_0001, ControlVal = 0b_1000_0000}, //a1
        new borderCheckElem { Mask =  0b_0111_0000, ControlVal = 0b_0010_0000}, //a3
        new borderCheckElem { Mask =  0b_0001_1100, ControlVal = 0b_0000_1000}, //a5
        new borderCheckElem { Mask =  0b_0000_0111, ControlVal = 0b_0000_0010}, //a7
                
        new borderCheckElem { Mask =  0b_0101_0101, ControlVal = 0b_0101_0000}, //c24
        new borderCheckElem { Mask =  0b_0101_0101, ControlVal = 0b_0100_0001}, //c28
        new borderCheckElem { Mask =  0b_0101_0101, ControlVal = 0b_0001_0100}, //c46
        new borderCheckElem { Mask =  0b_0101_0101, ControlVal = 0b_0000_0101}, //c68

        new borderCheckElem { Mask =  0b_0101_0101, ControlVal = 0b_0101_0100}, //d246
        new borderCheckElem { Mask =  0b_0101_0101, ControlVal = 0b_0101_0001}, //d248
        new borderCheckElem { Mask =  0b_0101_0101, ControlVal = 0b_0100_0101}, //d268
        new borderCheckElem { Mask =  0b_0101_0101, ControlVal = 0b_0001_0101}, //d468
        
        new borderCheckElem { Mask =  0b_0101_0001, ControlVal = 0b_0100_0000}, //l2
        new borderCheckElem { Mask =  0b_0101_0100, ControlVal = 0b_0001_0000}, //l4
        new borderCheckElem { Mask =  0b_0001_0101, ControlVal = 0b_0000_0100}, //l6
        new borderCheckElem { Mask =  0b_0100_0101, ControlVal = 0b_0000_0001}, //l8
    };

    public GameObject[] borders = new GameObject[8];
    public int bordersCount;

    void drawBorders()
    {
        foreach(var item in borders)
        {
            Destroy(item);
        }
        bordersCount = 0;
        int i_identifier = 0;
        foreach (KeyValuePair<Direction, Cell> item in NeighborCells)
        {
            //UnityEngine.Debug.Log(item.Value.myState);
            if (item.Value.myState == State.Resource || item.Value.myState == State.Surface || item.Value.myState == State.Bonus || item.Value.myState == State.StartingPoint)
                i_identifier += 1 << (7 - getNeighbourIndex(item.Key));
        }
        
        //GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        
        //UnityEngine.Debug.Log(i_identifier);
       
        foreach(var item in _borderCheck.Select((value, i) => new { i, value }))
        {
            if ((i_identifier & item.value.Mask) == item.value.ControlVal)
            {
                borders[bordersCount] = new GameObject();
                borders[bordersCount].transform.parent = gameObject.transform;
                borders[bordersCount].transform.position = gameObject.transform.position;
                borders[bordersCount].AddComponent<SpriteRenderer>().sprite = borderSprites[item.i];
                borders[bordersCount].GetComponent<SpriteRenderer>().sortingOrder = 1;
                //UnityEngine.Debug.Log(borders[bordersCount]);
                bordersCount++;
            }
        }
    }

    void Die(Character character) // Debug purposes
    {
        switch (myState)
        {
            //break / return to the player
            case State.Deadly:
            case State.Surface:
            case State.Transition:
                //
                break;
            case State.Bonus:
                myBonus.EnableBonus(character);
                break;
            case State.Resource:
                break;
        }
        SetSprite(State.Transition);
        drawBorders();

        foreach (var item in NeighborCells)
        {
            if (item.Value.myState == State.Transition || item.Value.myState == State.Deadly)
            {
                //UnityEngine.Debug.Log("Obj: " + item.Value.gameObject + "\nState: " + item.Value.myState);
                item.Value.drawBorders();
            }
        }
            
    }

    void OnMouseOver() // Debug purposes
    {
        if (Input.GetKey(KeyCode.Mouse0))
            Die(null);
    }


private void OnCollisionEnter2D(Collision2D other1)
    {
        if (other1.gameObject.CompareTag("Player") && myState == State.Deadly)
        {
            other1.gameObject.GetComponent<Character>().Die();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other1)
    {
        if (other1.CompareTag("Player") && myState == State.Transition)
        {
            SetSprite(State.Deadly);
        }
    }

}