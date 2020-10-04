using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

    public GameObject background;
    public GameObject mainObject;
    private GameObject back1;
    private GameObject back2;
    private const float speed = 0.5f;
    private float dx = 0;
    private float dy = 0;
    private float counter = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        back1 = Instantiate(background, new Vector3(mainObject.transform.position.x - 3, mainObject.transform.position.y - 3, 0), Quaternion.identity);
        back2 = Instantiate(background, back1.transform.position + new Vector3(background.GetComponent<Renderer>().bounds.size.x, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter >= 0.5)
        {
            dx = Random.Range(-0.1f, 0.3f);
            dy = Random.Range(-0.3f, 0.3f);
            counter = 0;
        }
        back1.transform.Translate(new Vector3(dx * speed * Time.deltaTime, dy * speed * Time.deltaTime));
        back2.transform.Translate(new Vector3(dx * speed * Time.deltaTime, dy * speed * Time.deltaTime));
        if (back2.transform.position.x >= 3)
        {
            var temp = back1;
            back1 = back2;
            back2 = temp;
            back1.transform.position = new Vector3(back1.transform.position.x - 2 * background.GetComponent<Renderer>().bounds.size.x, back2.transform.position.z);
        }
    }
}
