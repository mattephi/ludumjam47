using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chooseOneButtonBehaviour : MonoBehaviour
{
    
    public GameObject buttonPointer;
    private RectTransform buttonTransform;
    public chooseOneButtonBehaviour otherButton1;
    public chooseOneButtonBehaviour otherButton2;
    public bool ifChosed;

    public void buttonPushed()
    {
        buttonTransform = GetComponent<RectTransform>();
        buttonPointer.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + this.buttonTransform.rect.height / 3, buttonPointer.transform.position.z);

        
        ifChosed = true;
        otherButton1.ifChosed = true;
        otherButton2.ifChosed = true;
        Debug.Log("1");
    }
    public void PointerEnter()
    {
        Debug.Log("1");
        if (!ifChosed)
        {
            buttonTransform = GetComponent<RectTransform>();
            buttonPointer.SetActive(true);
            buttonPointer.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + this.buttonTransform.rect.height / 3, buttonPointer.transform.position.z);
        }
    }

    public void PointerOut()
    {
        Debug.Log("1");
        if (!ifChosed)
        {
            buttonPointer.SetActive(false);
        }
    }
}
