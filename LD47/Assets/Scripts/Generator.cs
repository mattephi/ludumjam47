using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] private float cellSize = 1f;
    public GlobalController GlobalController;

    [SerializeField] private GameObject CellPrefab;
    [SerializeField] private GameObject CharacterPrefab;
    
    public int rows, columns;
    public Sprite StartCellSprite;
    public Character[] Characters;
    public Cell[,] CellMatrix;

    private void OnEnable()
    {
        GenerateMatrix();
        SpawnChar();
        GlobalController.char1 = Characters[0];
        GlobalController.char2 = Characters[1];
    }

    private void GenerateMatrix()
    {
        CellMatrix = new Cell[rows + 2, columns];
        //just for starting el-s
        var thisTransformPosition = this.transform.position;
        for (var j = 0; j < columns; j++)
        {
            Vector3 spawnPoint = thisTransformPosition + new Vector3(j * cellSize, 0, 0);
            CellMatrix[0, j] = Instantiate(CellPrefab, spawnPoint, Quaternion.identity).GetComponent<Cell>();
            CellMatrix[0, j].MyState = Cell.State.StartingPoint;
            //Debug.Log("" + 0 + " " + j + " " + CellMatrix[0, j]);

            spawnPoint = thisTransformPosition + new Vector3(j * cellSize, -cellSize * (rows + 1), 0);
            CellMatrix[rows + 1, j] = Instantiate(CellPrefab, spawnPoint, Quaternion.identity).GetComponent<Cell>();
            CellMatrix[rows + 1, j].MyState = Cell.State.StartingPoint;
            //Debug.Log("" + (rows + 1) + " " + j + " " + CellMatrix[rows + 1, j]);

            CellMatrix[rows + 1, j].GetComponent<SpriteRenderer>().sprite = StartCellSprite;
            CellMatrix[0, j].GetComponent<SpriteRenderer>().sprite = StartCellSprite;
        }
        
        for (var i = 1; i < rows + 1; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                Vector3 spawnPoint = thisTransformPosition + new Vector3(j * cellSize, -i * cellSize, 0);
                CellMatrix[i, j] = Instantiate(CellPrefab, spawnPoint, Quaternion.identity).GetComponent<Cell>();
                CellMatrix[i, j].MyState = Cell.State.Surface;
                Debug.Log("" + i + " " + j + " " + CellMatrix[i, j]);
            }
        }

        for (var i1 = 0; i1 < rows + 2; i1++) // writing nighbours for each cell
            for (var i2 = 0; i2 < columns; i2++)
            {
                Debug.Log("Writing neighbours" + i1 + " " + i2 + " " + CellMatrix[i1, i2]);
                if (i1 > 0)
                    CellMatrix[i1, i2].NeighborCells[Cell.Direction.Up] = CellMatrix[i1 - 1, i2];
                if (i2 + 1 < columns)
                    CellMatrix[i1, i2].NeighborCells[Cell.Direction.Right] = CellMatrix[i1, i2 + 1];
                if (i1 + 1 < rows)
                    CellMatrix[i1, i2].NeighborCells[Cell.Direction.Down] = CellMatrix[i1 + 1, i2];
                if (i2 > 0)
                    CellMatrix[i1, i2].NeighborCells[Cell.Direction.Left] = CellMatrix[i1, i2 - 1];

                if (i1 > 0 && i2 > 0)
                    CellMatrix[i1, i2].NeighborCells[Cell.Direction.UpLeft] = CellMatrix[i1 - 1, i2 - 1];
                if (i1 > 0 && i2 + 1 < columns)
                    CellMatrix[i1, i2].NeighborCells[Cell.Direction.UpRight] = CellMatrix[i1 - 1, i2 + 1];
                if (i1 + 1 < rows && i2 + 1 < columns)
                    CellMatrix[i1, i2].NeighborCells[Cell.Direction.DownRight] = CellMatrix[i1 + 1, i2 + 1];
                if (i1 + 1 < rows && i2 > 0)
                    CellMatrix[i1, i2].NeighborCells[Cell.Direction.DownLeft] = CellMatrix[i1 + 1, i2 - 1];
                Debug.Log(CellMatrix[i1, i2].NeighborCells);
            }
    }
    
    void SpawnChar()
    {
        Characters = new Character[2];
        var pointA = UnityEngine.Random.Range(0, columns);
        var pointB = UnityEngine.Random.Range(0, columns);
        var thisTransformPosition = this.transform.position;
        var spawnA = thisTransformPosition + new Vector3(pointA * cellSize, 0, 0);
        var spawnB = thisTransformPosition +  new Vector3(pointB *cellSize, -cellSize * (rows + 1),0) ; 
        Characters[0] = Instantiate(CharacterPrefab,spawnA , Quaternion.identity).GetComponent<Character>();
        Characters[0].BaseDirection = Cell.Direction.Down;
        Characters[0].MyDirection = Cell.Direction.Down;
        Characters[0].curCell = CellMatrix[0, pointA];
        
        Characters[1] = Instantiate(CharacterPrefab,spawnB , Quaternion.identity).GetComponent<Character>();
        Characters[1].BaseDirection = Cell.Direction.Up;
        Characters[1].MyDirection = Cell.Direction.Up;
        Characters[1].curCell = CellMatrix[rows + 1, pointB];
        
    }
}