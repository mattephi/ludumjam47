using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Generator : MonoBehaviour
{
    [SerializeField] private float cellSize = 1f;
    public Generator Gen;

    [SerializeField] private GameObject CellPrefab;
    [SerializeField] private GameObject CharacterPrefab;
    public int rows, columns;
    public Sprite StartCellSprite;
    public Character[] Characters;
    public Cell[,] CellMatrix;

    private void OnEnable()
    {
        CellMatrix = new Cell[rows + 2, columns];
        //just for starting el-s
        for (int j = 0; j < columns; j++)
        {
            Vector3 spawnPoint = new Vector3(j*cellSize, 0,0); 
            CellMatrix[0, j] = Instantiate(CellPrefab, spawnPoint, Quaternion.identity).GetComponent<Cell>();
            CellMatrix[0, j].MyState = Cell.State.StartingPoint;
            
            spawnPoint = new Vector3(j*cellSize, -cellSize * (rows + 1),0); 
            CellMatrix[rows + 1, j] = Instantiate(CellPrefab, spawnPoint, Quaternion.identity).GetComponent<Cell>();
            CellMatrix[rows + 1, j].MyState = Cell.State.StartingPoint;
            
            CellMatrix[rows + 1, j].GetComponent<SpriteRenderer>().sprite = StartCellSprite;
            CellMatrix[0, j].GetComponent<SpriteRenderer>().sprite = StartCellSprite;
        }

        for (int i = 1; i < rows + 1; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 spawnPoint = new Vector3(j *cellSize, -i*cellSize,0); 
                CellMatrix[rows + 1, j] = Instantiate(CellPrefab, spawnPoint, Quaternion.identity).GetComponent<Cell>();
                CellMatrix[rows + 1, j].MyState = Cell.State.Surface;
            }
        }
        SpawnChar();
    }

    void SpawnChar()
    {
        Characters = new Character[2];
        int pointA = UnityEngine.Random.Range(0, columns), pointB = UnityEngine.Random.Range(0, columns);
        Vector3 spawnA = new Vector3(pointA *cellSize, 0,0), spawnB = new Vector3(pointB *cellSize, -cellSize * (rows + 1),0) ; 
        Characters[0] = Instantiate(CharacterPrefab,spawnA , Quaternion.identity).GetComponent<Character>();
        Characters[0].BaseDirection = Character.Direction.Down;
        Characters[0].MyDirection = Character.Direction.Down;
        
        Characters[1] = Instantiate(CharacterPrefab,spawnB , Quaternion.identity).GetComponent<Character>();
        Characters[1].BaseDirection = Character.Direction.Down;
        Characters[1].MyDirection = Character.Direction.Down;
    }
}
