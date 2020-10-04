using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
    [SerializeField] private Generator generator;
    public Character char1;
    public Character char2;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        //drawCaves(_generator.CellMatrix, _generator.rows, _generator.columns);
    }
}
