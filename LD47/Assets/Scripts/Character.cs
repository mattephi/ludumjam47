using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Character : MonoBehaviour
{
    #region Initialization
    [SerializeField] private SpriteRenderer spriteRenderer;
    public float MinMovingSpeed
    {
        get { return _minMiningSpeed; }
        private set { _minMiningSpeed = value; }
    }
    [SerializeField] private float _minMiningSpeed = 2f;
    [SerializeField] private float movingSpeed = 2.0f;

    public float CurDamage;
    
    public Cell.Direction MyDirection;
    public Cell.Direction BaseDirection;
    public enum State
    {
        Moving,
        Starting,
        Mining,
        Waiting
    }

    public State Mystate;
    
    public Cell curCell;

    public bool Immortal;
    #endregion
    
    
    // Start is called before the first frame update
    void Start()
    {
        CurDamage = _minMiningSpeed;
        movingSpeed = _minMiningSpeed;
        Mystate = State.Waiting;
    }

    void validateAndMoveToNextCell()
    {
        if (!curCell.IsExist(MyDirection) || !curCell.IsAvailable(MyDirection))
        {
            print("Die");
            Die();
        }
        
        if (curCell.MyState == Cell.State.StartingPoint)
        {
            //change the BaseDirection
            if (MyDirection == Cell.Direction.Up)
            {
                BaseDirection = Cell.Direction.Down;
            }
            else if (MyDirection == Cell.Direction.Down)
            {
                BaseDirection = Cell.Direction.Down;
            }
            
            //turn back from borders
            if (!curCell.IsExist(Cell.Direction.Left))
            {
                MyDirection = Cell.Direction.Right;
            }
            else if (!curCell.IsExist(Cell.Direction.Right))
            {
                MyDirection = Cell.Direction.Left;
            }
        }//if it's not a starting point
        else
        {
            curCell.MyState = Cell.State.Deadly;
        }
        
        curCell = curCell.NeighborCells[MyDirection];
    }

    void Mine()
    {
        validateAndMoveToNextCell();
        while (curCell.MyState != Cell.State.Transition && curCell.MyState != Cell.State.StartingPoint)
        {
            curCell.MyState = Cell.State.Transition;
            curCell.GetDamage(this);
        }
        
        // Start mining.
        Mystate = State.Moving;
        Move();
    }
    
    void Move()
    {
        print(MyDirection);
        transform.Translate(-1000, -1000, -1000);
        Mystate = State.Waiting;
        transform.position = Vector3.MoveTowards(transform.position, curCell.transform.position, Time.deltaTime*movingSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        switch (Mystate)
        {
            case State.Moving:
                break;
            case State.Starting:
                break;
            case State.Mining:
                break;
            case State.Waiting:
                Mystate = State.Mining;
                Mine();
                break;
            default:
                break;
        }
    }

    void Die()
    {
        print("DIE");
        Destroy(this);
    }
}
