using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    sw#region Initialization
    public float MinMovingSpeed
    {
        get { return _minMiningSpeed; }
        private set { _minMiningSpeed = value; }
    }
    [SerializeField] private float _minMiningSpeed;
    [SerializeField] private float movingSpeed = 2f;

    public float CurMiningSpeed;
    
    public enum Direction {
        Up,
        Down,
        Left,
        Right
    };
    public Enum MyDirection;
    public Enum BaseDirection;
    public enum State
    {
        Moving,
        Starting,
        Mining,
        Waiting
    }

    public Cell nextCell;
    public Cell curCell;

    public bool Immortal;
    #endregion
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
