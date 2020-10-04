// using System;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
// using Random = System.Random;

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

    public State myState;
    
    public Cell curCell;

    public bool immortal;
    
    public void Init(Cell.Direction baseDirection, Cell.Direction myDirection, Cell curCell)
    {
        this.baseDirection = baseDirection;
        this.myDirection = myDirection;
        this.curCell = curCell;
        curDamage = minDamage;
        myState = State.Waiting;
    }

    private bool _isValidated = false;
    private float _lastDamage = 0f;
    private float DamageDeltaTime = 2f;

    public Character(bool isValidated)
    {
        _isValidated = isValidated;
    }

    #endregion
    
    // Start is called before the first frame update
    void OnEnable()
    {
        _lastDamage = 0f;
        curDamage = minDamage;
        myState = State.Waiting;
    }

    private void ValidateAndMoveToNextCell()
    {
        if (!curCell.IsExist(myDirection) || !curCell.IsAvailable(myDirection))
        {
            Die();
        }
        
        if (curCell.myState == Cell.State.StartingPoint && curCell.IsExist(baseDirection))
        {
            if (curCell.IsExist(baseDirection)
            {
                if (baseDirection == Cell.Direction.Up)
                {
                    baseDirection = Cell.Direction.Down;
                }
                else
                {
                    baseDirection = Cell.Direction.Up;
                }
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
        }

        if (curCell.IsExist(myDirection) && curCell.IsAvailable(myDirection))
        {
            curCell = curCell.NeighborCells[myDirection];
        }
    }

    private void Move()
    {
        print(myDirection);
        //transform.Translate(-1000, -1000, -1000);
        transform.position = Vector3.MoveTowards(transform.position, curCell.transform.position, Time.deltaTime * movingSpeed);
    }

    // Update is called once per frame
    private void Update()
    {
        switch (myState)
        {
            case State.Moving:
                if (Vector3.Distance(curCell.transform.position, transform.position) > 1e-3)
                {
                    Move();
                }
                else
                {
                    myState = State.Waiting;
                }
                break;
            case State.Starting:
                break;
            case State.Mining:
                if (!_isValidated)
                {
                    ValidateAndMoveToNextCell();
                }

                if (curCell.myState != Cell.State.Transition && curCell.myState != Cell.State.StartingPoint && Time.realtimeSinceStartup - _lastDamage > DamageDeltaTime)
                {
                    curCell.GetDamage(this, curDamage);
                    _lastDamage = Time.realtimeSinceStartup;
                }
                else if(curCell.myState == Cell.State.Transition)
                {
                    myState = State.Moving;
                }
                break;
            case State.Waiting:
                myState = State.Mining;
                break;
        }
    }

    public void Die()
    {
        print("DIE   "  + this);
        Time.timeScale = 0;
        
        //Destroy(this);
    }
}
