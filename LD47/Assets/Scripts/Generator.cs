// using System;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] private float cellSize = 1f;
    public GlobalController globalController;

    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject character1Prefab;
    [SerializeField] private GameObject character2Prefab;
    public bool generateNow = false;
    
    public int rows, columns;
    public Sprite upGrass;
    public Sprite downGrass;
    public Character[] characters;
    private Cell[,] _cellMatrix;

    [SerializeField] private GameObject Ore1;
    [SerializeField] private GameObject Ore2;
    [SerializeField] private GameObject Ore3;
    [SerializeField] private GameObject DamageBoost;
    [SerializeField] private GameObject SplashBomb;
    [SerializeField] private GameObject CrossBomb;
    [SerializeField] private GameObject Swap;
    [SerializeField] private GameObject Immortality;

    private void OnEnable()
    {
        if (generateNow)
        { 
            GenerateMatrix(rows, columns);
            SpawnChar();
        }
    }

    public void GenerateMatrix(int rows, int columns)
    {
        _cellMatrix = new Cell[rows + 2, columns];
        //just for starting el-s
        var thisTransformPosition = this.transform.position;
        for (var j = 0; j < columns; j++)
        {
            Vector3 spawnPoint = thisTransformPosition + new Vector3(j * cellSize, 0, 0);
            _cellMatrix[0, j] = Instantiate(cellPrefab, spawnPoint, Quaternion.identity).GetComponent<Cell>();
            _cellMatrix[0, j].Init(Cell.State.StartingPoint);
            _cellMatrix[0, j].surfaceSprite = upGrass;
            var dx = _cellMatrix[0, j].GetComponent<Renderer>().bounds.size.x;
            _cellMatrix[0, j].reachMe = spawnPoint + new Vector3(0, dx/2, 0);
            //Debug.Log("" + 0 + " " + j + " " + CellMatrix[0, j]);

            spawnPoint = thisTransformPosition + new Vector3(j * cellSize, -cellSize * (rows + 1), 0);
            _cellMatrix[rows + 1, j] = Instantiate(cellPrefab, spawnPoint, Quaternion.identity).GetComponent<Cell>();
            _cellMatrix[rows + 1, j].Init(Cell.State.StartingPoint);
            //Debug.Log("" + (rows + 1) + " " + j + " " + CellMatrix[rows + 1, j]);
            _cellMatrix[rows + 1, j].surfaceSprite = downGrass;
            _cellMatrix[rows + 1, j].reachMe = spawnPoint + new Vector3(0, -dx/2, 0);
            
            _cellMatrix[rows + 1, j].GetComponent<SpriteRenderer>().sprite = downGrass;
            _cellMatrix[0, j].GetComponent<SpriteRenderer>().sprite = upGrass;
        }
        
        for (var i = 1; i < rows + 1; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                Vector3 spawnPoint = thisTransformPosition + new Vector3(j * cellSize, -i * cellSize, 0);
                _cellMatrix[i, j] = Instantiate(cellPrefab, spawnPoint, Quaternion.identity).GetComponent<Cell>();
                _cellMatrix[i, j].Init(Cell.State.Surface);
                var dx = _cellMatrix[0, j].GetComponent<Renderer>().bounds.size.x;
                _cellMatrix[i, j].reachMe = spawnPoint;
                
                SpawnBonRes(_cellMatrix[i, j]);
                //Debug.Log("" + i + " " + j + " " + _cellMatrix[i, j]);
            }
        }

        for (var i1 = 0; i1 < rows + 2; i1++) // writing neighbours for each cell
            for (var i2 = 0; i2 < columns; i2++)
            {
                //Debug.Log("Writing neighbours" + i1 + " " + i2 + " " + _cellMatrix[i1, i2]);
                if (i1 > 0)
                    _cellMatrix[i1, i2].NeighborCells[Cell.Direction.Up] = _cellMatrix[i1 - 1, i2];
                if (i2 + 1 < columns)
                    _cellMatrix[i1, i2].NeighborCells[Cell.Direction.Right] = _cellMatrix[i1, i2 + 1];
                if (i1 < rows + 1)
                    _cellMatrix[i1, i2].NeighborCells[Cell.Direction.Down] = _cellMatrix[i1 + 1, i2];
                if (i2 > 0)
                    _cellMatrix[i1, i2].NeighborCells[Cell.Direction.Left] = _cellMatrix[i1, i2 - 1];

                if (i1 > 0 && i2 > 0)
                    _cellMatrix[i1, i2].NeighborCells[Cell.Direction.UpLeft] = _cellMatrix[i1 - 1, i2 - 1];
                if (i1 > 0 && i2 + 1 < columns)
                    _cellMatrix[i1, i2].NeighborCells[Cell.Direction.UpRight] = _cellMatrix[i1 - 1, i2 + 1];
                if (i1 < rows + 1 && i2 + 1 < columns)
                    _cellMatrix[i1, i2].NeighborCells[Cell.Direction.DownRight] = _cellMatrix[i1 + 1, i2 + 1];
                if (i1 < rows + 1 && i2 > 0)
                    _cellMatrix[i1, i2].NeighborCells[Cell.Direction.DownLeft] = _cellMatrix[i1 + 1, i2 - 1];
                //Debug.Log(_cellMatrix[i1, i2].NeighborCells);         
            }
    }

    public void SpawnChar()
    {
        characters = new Character[2];
        var pointA = Random.Range(0, columns);
        var pointB = Random.Range(0, columns);
        var thisTransformPosition = this.transform.position;
        var spawnA = thisTransformPosition + new Vector3(pointA * cellSize, 0, 0);
        var spawnB = thisTransformPosition +  new Vector3(pointB *cellSize, -cellSize * (rows + 1),0) ; 
        characters[0] = Instantiate(character1Prefab, spawnA , Quaternion.identity).GetComponent<Character>();
        characters[0].Init(Cell.Direction.Down, Cell.Direction.Down, _cellMatrix[0, pointA]);
        characters[0].GlobalController = globalController;
        
        characters[1] = Instantiate(character2Prefab, spawnB , Quaternion.identity).GetComponent<Character>();
        characters[1].transform.Rotate(0, 0, 180);
        characters[1].Init(Cell.Direction.Up, Cell.Direction.Up, _cellMatrix[rows + 1, pointB]);
        characters[1].GlobalController = globalController;

        globalController.char1 = characters[0];
        globalController.char2 = characters[1];
    }

    private void SpawnBonRes(Cell cell)
    {
        GameObject obj = null;
        float prob = UnityEngine.Random.Range(0.0f, 100.0f);
        if (prob <= 5f)
        {
            obj = Instantiate(Ore1, cell.reachMe, Quaternion.identity);
            cell.myState = Cell.State.Resource;
            cell.myResource = obj.GetComponent<Resource>();
        }
        else if (prob <= 8f)
        {
            obj = Instantiate(Ore2, cell.reachMe, Quaternion.identity);
            cell.myState = Cell.State.Resource;
            cell.myResource = obj.GetComponent<Resource>();
        }
        else if(prob <= 9f)
        {
            obj = Instantiate(Ore3, cell.reachMe, Quaternion.identity);
            cell.myState = Cell.State.Resource;
            cell.myResource = obj.GetComponent<Resource>();
        }
        else if(prob <= 13f)
        {
            obj = Instantiate(DamageBoost, cell.reachMe, Quaternion.identity);
            cell.myState = Cell.State.Bonus;
            cell.myBonus = obj.GetComponent<Bonus>();
        }
        else if(prob <= 15.5f)
        {
            obj = Instantiate(SplashBomb, cell.reachMe, Quaternion.identity);
            cell.myState = Cell.State.Bonus;
            cell.myBonus = obj.GetComponent<Bonus>();
        }
        else if(prob <= 18f)
        {
            obj = Instantiate(CrossBomb, cell.reachMe, Quaternion.identity);
            cell.myState = Cell.State.Bonus;
            cell.myBonus = obj.GetComponent<Bonus>();
        }
        else if(prob <= 19f)
        {
            obj = Instantiate(Swap, cell.reachMe, Quaternion.identity);
            cell.myState = Cell.State.Bonus;
            cell.myBonus = obj.GetComponent<Bonus>();
        }
        else if(prob <= 20f)
        {
            obj = Instantiate(Immortality, cell.reachMe, Quaternion.identity);
            cell.myState = Cell.State.Bonus;
            cell.myBonus = obj.GetComponent<Bonus>();
        }
        if(obj != null)
            cell.AddChild(obj);
    }
    
}