// using System;
// using System.Collections;
// using System.Collections.Generic;
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

    private void GenerateMatrix()
    {
        _cellMatrix = new Cell[rows + 2, columns];
        //just for starting el-s
        var thisTransformPosition = this.transform.position;
        for (var j = 0; j < columns; j++)
        {
            Vector3 spawnPoint = thisTransformPosition + new Vector3(j * cellSize, 0, 0);
            _cellMatrix[0, j] = Instantiate(cellPrefab, spawnPoint, Quaternion.identity).GetComponent<Cell>();
            _cellMatrix[0, j].Init(Cell.State.StartingPoint);
            //Debug.Log("" + 0 + " " + j + " " + CellMatrix[0, j]);

            spawnPoint = thisTransformPosition + new Vector3(j * cellSize, -cellSize * (rows + 1), 0);
            _cellMatrix[rows + 1, j] = Instantiate(cellPrefab, spawnPoint, Quaternion.identity).GetComponent<Cell>();
            _cellMatrix[rows + 1, j].Init(Cell.State.StartingPoint);
            //Debug.Log("" + (rows + 1) + " " + j + " " + CellMatrix[rows + 1, j]);

            _cellMatrix[rows + 1, j].GetComponent<SpriteRenderer>().sprite = startCellSprite;
            _cellMatrix[0, j].GetComponent<SpriteRenderer>().sprite = startCellSprite;
        }
        
        for (var i = 1; i < rows + 1; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                Vector3 spawnPoint = thisTransformPosition + new Vector3(j * cellSize, -i * cellSize, 0);
                _cellMatrix[i, j] = Instantiate(cellPrefab, spawnPoint, Quaternion.identity).GetComponent<Cell>();
                _cellMatrix[i, j].Init(Cell.State.Surface);
                Debug.Log("" + i + " " + j + " " + _cellMatrix[i, j]);
            }
        }

        for (var i1 = 0; i1 < rows + 2; i1++) // writing neighbours for each cell
            for (var i2 = 0; i2 < columns; i2++)
            {
                Debug.Log("Writing neighbours" + i1 + " " + i2 + " " + _cellMatrix[i1, i2]);
                if (i1 > 0)
                    _cellMatrix[i1, i2].NeighborCells[Cell.Direction.Up] = _cellMatrix[i1 - 1, i2];
                if (i2 + 1 < columns)
                    _cellMatrix[i1, i2].NeighborCells[Cell.Direction.Right] = _cellMatrix[i1, i2 + 1];
                if (i1 + 1 < rows)
                    _cellMatrix[i1, i2].NeighborCells[Cell.Direction.Down] = _cellMatrix[i1 + 1, i2];
                if (i2 > 0)
                    _cellMatrix[i1, i2].NeighborCells[Cell.Direction.Left] = _cellMatrix[i1, i2 - 1];

                if (i1 > 0 && i2 > 0)
                    _cellMatrix[i1, i2].NeighborCells[Cell.Direction.UpLeft] = _cellMatrix[i1 - 1, i2 - 1];
                if (i1 > 0 && i2 + 1 < columns)
                    _cellMatrix[i1, i2].NeighborCells[Cell.Direction.UpRight] = _cellMatrix[i1 - 1, i2 + 1];
                if (i1 + 1 < rows && i2 + 1 < columns)
                    _cellMatrix[i1, i2].NeighborCells[Cell.Direction.DownRight] = _cellMatrix[i1 + 1, i2 + 1];
                if (i1 + 1 < rows && i2 > 0)
                    _cellMatrix[i1, i2].NeighborCells[Cell.Direction.DownLeft] = _cellMatrix[i1 + 1, i2 - 1];
                Debug.Log(_cellMatrix[i1, i2].NeighborCells);
            }
    }
    
    private void SpawnChar()
    {
        characters = new Character[2];
        var pointA = Random.Range(0, columns);
        var pointB = Random.Range(0, columns);
        var thisTransformPosition = this.transform.position;
        var spawnA = thisTransformPosition + new Vector3(pointA * cellSize, 0, 0);
        var spawnB = thisTransformPosition +  new Vector3(pointB *cellSize, -cellSize * (rows + 1),0) ; 
        characters[0] = Instantiate(characterPrefab,spawnA , Quaternion.identity).GetComponent<Character>();
        characters[0].Init(Cell.Direction.Down, Cell.Direction.Down, _cellMatrix[0, pointA]);
        /*characters[0].baseDirection = Cell.Direction.Down;
        characters[0].myDirection = Cell.Direction.Down;
        characters[0].curCell = _cellMatrix[0, pointA];*/
        
        //characters[1] = Instantiate(characterPrefab,spawnB , Quaternion.identity).GetComponent<Character>();
        //characters[1].Init(Cell.Direction.Up, Cell.Direction.Up, _cellMatrix[rows + 1, pointB]);
        characters[1] = null;
        
        
        
        /*characters[1].baseDirection = Cell.Direction.Up;
        characters[1].myDirection = Cell.Direction.Up;
        characters[1].curCell = _cellMatrix[rows + 1, pointB];*/
        
    }
}