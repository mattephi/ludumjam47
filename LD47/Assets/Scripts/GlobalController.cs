using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
    private Character char1;
    private Character char2;
    
    // Start is called before the first frame update
    void Start()
    {
        char1 = this.gameObject.AddComponent<Character>();
        char2 = this.gameObject.AddComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
