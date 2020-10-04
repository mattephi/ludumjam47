// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]private GlobalController globalController;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            print("Swap made");
            globalController.SwapCharacters();
        }
        
        if (globalController.char1)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {       
                if (globalController.char1.baseDirection == Cell.Direction.Up)
                {
                    globalController.char1.myDirection = Cell.Direction.Up;
                }
            }
            if (Input.GetKeyDown(KeyCode.A))
            {    
                globalController.char1.transform.Rotate(new Vector3(0, 1, 0), 180);
                globalController.char1.myDirection = Cell.Direction.Left;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {    
                if (globalController.char1.baseDirection == Cell.Direction.Down)
                {
                    globalController.char1.myDirection = Cell.Direction.Down;
                }
            }
            if (Input.GetKeyDown(KeyCode.D))
            {    
                globalController.char1.transform.Rotate(new Vector3(0, 1, 0), 180);
                globalController.char1.myDirection = Cell.Direction.Right;
            }
        }

        if (globalController.char2)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {    
                if (globalController.char2.baseDirection == Cell.Direction.Up)
                {
                    globalController.char2.myDirection = Cell.Direction.Up;
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {    
                if (globalController.char2.baseDirection == Cell.Direction.Down)
                {
                    globalController.char2.myDirection = Cell.Direction.Down;
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {    
                globalController.char2.transform.Rotate(new Vector3(0, 1, 0), 180);
                globalController.char2.myDirection = Cell.Direction.Left;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {    
                globalController.char2.transform.Rotate(new Vector3(0, 1, 0), 180);
                globalController.char2.myDirection = Cell.Direction.Right;
            }
        }
    }
}
