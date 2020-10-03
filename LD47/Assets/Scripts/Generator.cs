using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] private float cellSize;

    [SerializeField] private GameObject CellPrefab;
    private int rows, collums;
    private Cell[,] CellMatrix;

    private void OnEnable()
    {
        CellMatrix = new Cell[rows + 2, collums];
        for (int j = 0; j < collums; j++)
        {
            Vector3 spawnPoint = new Vector3(0, j*cellSize,0); 
            CellMatrix[0, j] = Instantiate(CellPrefab, spawnPoint, Quaternion.identity).GetComponent<Cell>();
            CellMatrix[0, j].MyState = Cell.State.StartingPoint;
            
            spawnPoint = new Vector3(cellSize * (rows + 1), j*cellSize,0); 
            CellMatrix[rows + 1, j] = Instantiate(CellPrefab, spawnPoint, Quaternion.identity).GetComponent<Cell>();
            CellMatrix[rows + 1, j].MyState = Cell.State.StartingPoint;
        }

    }
}
