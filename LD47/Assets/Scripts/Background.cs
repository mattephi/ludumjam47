using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

    public GameObject background;
    public GameObject mainObject;
    private GameObject back;
    private float speed = 0.1f;
    private float startx = 0;
    private float starty = 0;
    private float counter = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        float startx = mainObject.transform.position.x - 5;
        float starty = mainObject.transform.position.y - 5;
        back = Instantiate(background, new Vector3(startx, starty, 0), Quaternion.identity);
        back.transform.localScale *= 3;
    }

    // Update is called once per frame
    void Update()
    {
        back.transform.Translate(new Vector3(speed * Time.deltaTime, speed * Time.deltaTime));
        back.transform.Translate(new Vector3(speed * Time.deltaTime,  speed * Time.deltaTime));
        if (back.transform.position.x >= -5 || back.transform.position.x < -20)
        {
            speed = -speed;
        }
    }
}
