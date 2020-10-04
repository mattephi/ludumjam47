using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Character : MonoBehaviour
{
    #region Initialization
    [SerializeField] private SpriteRenderer spriteRenderer;
    public float MinDamage
    {
        get => minDamage;
        private set => minDamage = value;
    }
    [SerializeField] private float minDamage = 2f;
    [SerializeField] private float movingSpeed = 2.0f;

    public float curDamage;
    
    public Cell.Direction myDirection;
    public Cell.Direction baseDirection;
    public enum State
    {
        Moving,
        Starting,
        Mining,
        Waiting
    }

    public State mystate;
    
    public Cell curCell;

    public bool immortal;
    
    public Character(Cell.Direction baseDirection, Cell.Direction myDirection, Cell curCell)
    {
        this.baseDirection = baseDirection;
        this.myDirection = myDirection;
        this.curCell = curCell;
        curDamage = minDamage;
        mystate = State.Waiting;
    }
    #endregion
    
    
    // Start is called before the first frame update
    void OnEnable()
    {
        curDamage = minDamage;
        mystate = State.Waiting;
    }

    void ValidateAndMoveToNextCell()
    {
        if (!curCell.IsExist(myDirection) || !curCell.IsAvailable(myDirection))
        {
            Die();
        }
        
        if (curCell.myState == Cell.State.StartingPoint)
        {
            //change the BaseDirection
            if (myDirection == Cell.Direction.Up)
            {
                baseDirection = Cell.Direction.Down;
            }
            else if (myDirection == Cell.Direction.Down)
            {
                baseDirection = Cell.Direction.Down;
            }
            
            //turn back from borders
            if (!curCell.IsExist(Cell.Direction.Left))
            {
                myDirection = Cell.Direction.Right;
            }
            else if (!curCell.IsExist(Cell.Direction.Right))
            {
                myDirection = Cell.Direction.Left;
            }
        }//if it's not a starting point
        else
        {
            curCell.myState = Cell.State.Transition;
        }
        
        curCell = curCell.NeighborCells[myDirection];
    }

    void Mine()
    {
        ValidateAndMoveToNextCell();
        while (curCell.myState != Cell.State.Transition && curCell.myState != Cell.State.StartingPoint)
        {
            curCell.GetDamage(this);
        }
        
        //NewCell state = transition, previous cell state = deadly
        
        
        // Start mining.
        mystate = State.Moving;
        Move();
    }
    
    void Move()
    {
        mystate = State.Waiting;
        transform.position = Vector3.MoveTowards(transform.position, curCell.transform.position, Time.deltaTime*movingSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        switch (mystate)
        {
            case State.Moving:
                break;
            case State.Starting:
                break;
            case State.Mining:
                break;
            case State.Waiting:
                mystate = State.Mining;
                Mine();
                break;
            default:
                break;
        }
    }

    public void Die()
    {
        print("DIE");
        Destroy(this);
    }
}
