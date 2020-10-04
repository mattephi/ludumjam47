using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
    [SerializeField] private Generator _generator;
    public Character char1;
    public Character char2;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void drawCaves(Cell[,] CellMatrix, int n, int m)
    {
        for (int i = 0; i < n; ++i)
        {
            for (int j = 0; j < m; ++j)
            {
                int sum = 0;
                for (int ti = i - 1; ti < i + 1; ++ti)
                {
                    for (int tj = j - 1; tj < j + 1; ++j)
                    {
                        if (ti >= 0 && ti <= n && tj >= 0 && tj <= m)
                        {
                            sum += (1 << (3 * ti + tj));
                        }
                    }
                }

                int mask = (1 << 4);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //drawCaves(_generator.CellMatrix, _generator.rows, _generator.columns);
    }
}
