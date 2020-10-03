using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] private float cellSize = 1f;

    [SerializeField] private GameObject CellPrefab;
    public int rows, columns;
    public Character[] Characters;
    public Cell[,] CellMatrix;

    private void OnEnable()
    {
        CellMatrix = new Cell[rows + 2, columns];
        //just for starting el-s
        for (int j = 0; j < columns; j++)
        {
            Vector3 spawnPoint = new Vector3(0, j*cellSize,0); 
            CellMatrix[0, j] = Instantiate(CellPrefab, spawnPoint, Quaternion.identity).GetComponent<Cell>();
            CellMatrix[0, j].MyState = Cell.State.StartingPoint;
            
            spawnPoint = new Vector3(cellSize * (rows + 1), j*cellSize,0); 
            CellMatrix[rows + 1, j] = Instantiate(CellPrefab, spawnPoint, Quaternion.identity).GetComponent<Cell>();
            CellMatrix[rows + 1, j].MyState = Cell.State.StartingPoint;
        }

        for (int i = 1; i < rows + 1; i++)
        {
            for (int j = 0; j < columns; j++)
                i = 0;
        }

    }
}
