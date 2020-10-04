using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] private float cellSize = 1f;
    public GlobalController globalController;

    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject characterPrefab;
    
    public int rows, columns;
    public Sprite startCellSprite;
    public Character[] characters;
    private Cell[,] _cellMatrix;

    private void OnEnable()
    {
        GenerateMatrix();
        SpawnChar();
        globalController.char1 = characters[0];
        globalController.char2 = characters[1];
    }

    void GenerateMatrix()
    {
        _cellMatrix = new Cell[rows + 2, columns];
        //just for starting el-s
        for (int j = 0; j < columns; j++)
        {
            var position = this.transform.position;
            Vector3 spawnPoint = position + new Vector3(j * cellSize, 0, 0);
            _cellMatrix[0, j] = Instantiate(cellPrefab, spawnPoint, Quaternion.identity).GetComponent<Cell>();
            _cellMatrix[0, j].MyState = Cell.State.StartingPoint;
            //Debug.Log("" + 0 + " " + j + " " + CellMatrix[0, j]);

            spawnPoint = position + new Vector3(j * cellSize, -cellSize * (rows + 1), 0);
            _cellMatrix[rows + 1, j] = Instantiate(cellPrefab, spawnPoint, Quaternion.identity).GetComponent<Cell>();
            _cellMatrix[rows + 1, j].MyState = Cell.State.StartingPoint;
            //Debug.Log("" + (rows + 1) + " " + j + " " + CellMatrix[rows + 1, j]);

            _cellMatrix[rows + 1, j].GetComponent<SpriteRenderer>().sprite = startCellSprite;
            _cellMatrix[0, j].GetComponent<SpriteRenderer>().sprite = startCellSprite;
        }
        
        for (int i = 1; i < rows + 1; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 spawnPoint = this.transform.position + new Vector3(j * cellSize, -i * cellSize, 0);
                _cellMatrix[i, j] = Instantiate(cellPrefab, spawnPoint, Quaternion.identity).GetComponent<Cell>();
                _cellMatrix[i, j].MyState = Cell.State.Surface;
                //Debug.Log("" + i + " " + j + " " + CellMatrix[i, j]);
            }
        }

        for (int i_1 = 0; i_1 < rows + 2; i_1++) // writing nighbours for each cell
            for (int i_2 = 0; i_2 < columns; i_2++)
            {
                //Debug.Log("Writing neighbours" + i_1 + " " + i_2 + " " + CellMatrix[i_1, i_2]);
                if (i_1 > 0)
                    _cellMatrix[i_1, i_2].NeighborCells[Cell.Direction.Up] = _cellMatrix[i_1 - 1, i_2];
                if (i_2 + 1 < columns)
                    _cellMatrix[i_1, i_2].NeighborCells[Cell.Direction.Right] = _cellMatrix[i_1, i_2 + 1];
                if (i_1 + 1 < rows)
                    _cellMatrix[i_1, i_2].NeighborCells[Cell.Direction.Down] = _cellMatrix[i_1 + 1, i_2];
                if (i_2 > 0)
                    _cellMatrix[i_1, i_2].NeighborCells[Cell.Direction.Left] = _cellMatrix[i_1, i_2 - 1];

                if (i_1 > 0 && i_2 > 0)
                    _cellMatrix[i_1, i_2].NeighborCells[Cell.Direction.UpLeft] = _cellMatrix[i_1 - 1, i_2 - 1];
                if (i_1 > 0 && i_2 + 1 < columns)
                    _cellMatrix[i_1, i_2].NeighborCells[Cell.Direction.UpRight] = _cellMatrix[i_1 - 1, i_2 + 1];
                if (i_1 + 1 < rows && i_2 + 1 < columns)
                    _cellMatrix[i_1, i_2].NeighborCells[Cell.Direction.DownRight] = _cellMatrix[i_1 + 1, i_2 + 1];
                if (i_1 + 1 < rows && i_2 > 0)
                    _cellMatrix[i_1, i_2].NeighborCells[Cell.Direction.DownLeft] = _cellMatrix[i_1 + 1, i_2 - 1];
                //Debug.Log(CellMatrix[i_1, i_2].NeighborCells);
            }
    }
    
    void SpawnChar()
    {
        characters = new Character[2];
        int pointA = UnityEngine.Random.Range(0, columns);
        int pointB = UnityEngine.Random.Range(0, columns);
        Vector3 spawnA = this.transform.position + new Vector3(pointA * cellSize, 0, 0);
        Vector3 spawnB = this.transform.position +  new Vector3(pointB *cellSize, -cellSize * (rows + 1),0) ; 
        characters[0] = Instantiate(characterPrefab,spawnA , Quaternion.identity).GetComponent<Character>();

        characters[1] = Instantiate(characterPrefab,spawnB , Quaternion.identity).GetComponent<Character>();
        characters[1].BaseDirection = Cell.Direction.Up;
        characters[1].MyDirection = Cell.Direction.Up;
        characters[1].curCell = _cellMatrix[rows + 1, pointB];
        
    }
}