using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Dictionary<Character.Direction, Cell> NeighborCells;
    
    public enum State
    {
        Free,
        Transition,
        Lava
    }
    public State MyState;
    
    public enum ObjectType
    {
        None,
        Bonus,
        Character,
        Resource,
        Obstacle 
    }
    public ObjectType MyObjectType;

    public GameObject MyObject;

    public bool IsExist(Character.Direction direction)
    {
        Cell neighbourcell;
        return (NeighborCells.TryGetValue(direction, out neighbourcell) && !ReferenceEquals(neighbourcell, null));
    }
    
    
}
