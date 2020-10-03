using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {    
            print("Works W");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {    
            
        }
        if (Input.GetKeyDown(KeyCode.S))
        {    
            
        }
        if (Input.GetKeyDown(KeyCode.D))
        {    
            
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {    
            
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {    
            
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {    
            
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {    
            print("Works ->");
        }
    }
}
