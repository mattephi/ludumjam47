using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region Initialization
    public float MinMovingSpeed
    {
        get { return _minMiningSpeed; }
        private set { _minMiningSpeed = value; }
    }
    [SerializeField] private float _minMiningSpeed;
    [SerializeField] private float movingSpeed = 2.0f;

    public float CurDamage;
    
    public enum Direction {
        Up,
        Down,
        Left,
        Right
    };
    public Direction MyDirection;
    public Direction BaseDirection;
    public enum State
    {
        Moving,
        Starting,
        Mining,
        Waiting
    }

    public State Mystate;
    
    public Cell nextCell;
    public Cell curCell;

    public bool Immortal;
    #endregion
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Mine()
    {
        // Start mining.
        Mystate = State.Moving;
        Move();
    }
    
    void Move()
    {
        nextCell = curCell.NeighborCells[MyDirection];
        transform.Translate(nextCell.gameObject.transform.position * Time.deltaTime);
        Mystate = State.Waiting;
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
}
