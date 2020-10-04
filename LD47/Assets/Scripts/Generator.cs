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
        generateMatrix();
        SpawnChar();
        GlobalController.char1 = Characters[0];
        GlobalController.char2 = Characters[1];
    }

    void generateMatrix()
    {
        CellMatrix = new Cell[rows + 2, columns];
        //just for starting el-s
        for (int j = 0; j < columns; j++)
        {
            Vector3 spawnPoint = this.transform.position + new Vector3(j * cellSize, 0, 0);
            CellMatrix[0, j] = Instantiate(CellPrefab, spawnPoint, Quaternion.identity).GetComponent<Cell>();
            CellMatrix[0, j].MyState = Cell.State.StartingPoint;
            //Debug.Log("" + 0 + " " + j + " " + CellMatrix[0, j]);

            spawnPoint = this.transform.position + new Vector3(j * cellSize, -cellSize * (rows + 1), 0);
            CellMatrix[rows + 1, j] = Instantiate(CellPrefab, spawnPoint, Quaternion.identity).GetComponent<Cell>();
            CellMatrix[rows + 1, j].MyState = Cell.State.StartingPoint;
            //Debug.Log("" + (rows + 1) + " " + j + " " + CellMatrix[rows + 1, j]);

            CellMatrix[rows + 1, j].GetComponent<SpriteRenderer>().sprite = StartCellSprite;
            CellMatrix[0, j].GetComponent<SpriteRenderer>().sprite = StartCellSprite;
        }
        
        for (int i = 1; i < rows + 1; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 spawnPoint = this.transform.position + new Vector3(j * cellSize, -i * cellSize, 0);
                CellMatrix[i, j] = Instantiate(CellPrefab, spawnPoint, Quaternion.identity).GetComponent<Cell>();
                CellMatrix[i, j].MyState = Cell.State.Surface;
                Debug.Log("" + i + " " + j + " " + CellMatrix[i, j]);
            }
        }

        for (int i_1 = 0; i_1 < rows + 2; i_1++) // writing nighbours for each cell
        for (int i_2 = 0; i_2 < columns; i_2++)
        {
            Debug.Log("Writing neighbours" + i_1 + " " + i_2 + " " + CellMatrix[i_1, i_2]);
            if (i_1 > 0 && i_2 > 0)
                CellMatrix[i_1, i_2].NeighborCells[Cell.Direction.UpLeft] = CellMatrix[i_1 - 1, i_2 - 1];
            if (i_1 > 0)
                CellMatrix[i_1, i_2].NeighborCells[Cell.Direction.Up] = CellMatrix[i_1 - 1, i_2];
            if (i_1 > 0 && i_2 + 1 < columns)
                CellMatrix[i_1, i_2].NeighborCells[Cell.Direction.UpRight] = CellMatrix[i_1 - 1, i_2 + 1];
            if (i_2 + 1 < columns)
                CellMatrix[i_1, i_2].NeighborCells[Cell.Direction.Right] = CellMatrix[i_1, i_2 + 1];
            if (i_1 + 1 < rows && i_2 + 1 < columns)
                CellMatrix[i_1, i_2].NeighborCells[Cell.Direction.DownRight] = CellMatrix[i_1 + 1, i_2 + 1];
            if (i_1 + 1 < rows)
                CellMatrix[i_1, i_2].NeighborCells[Cell.Direction.Down] = CellMatrix[i_1 + 1, i_2];
            if (i_1 + 1 < rows && i_2 > 0)
                CellMatrix[i_1, i_2].NeighborCells[Cell.Direction.DownLeft] = CellMatrix[i_1 + 1, i_2 - 1];
            if (i_2 > 0)
                CellMatrix[i_1, i_2].NeighborCells[Cell.Direction.Left] = CellMatrix[i_1, i_2 - 1];

            Debug.Log(CellMatrix[i_1, i_2].NeighborCells);
        }
        
    }
    
    void SpawnChar()
    {
        Characters = new Character[2];
        int pointA = UnityEngine.Random.Range(0, columns);
        int pointB = UnityEngine.Random.Range(0, columns);
        Vector3 spawnA = this.transform.position + new Vector3(pointA * cellSize, 0, 0);
        Vector3 spawnB = this.transform.position +  new Vector3(pointB *cellSize, -cellSize * (rows + 1),0) ; 
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