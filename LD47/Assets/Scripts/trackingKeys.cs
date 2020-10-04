using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trackingKeys : MonoBehaviour
{
    private bool ifFirstPlayerReady = false;
    private bool ifSecondPlayerReady = false;
    public GameObject wButtonDisabled;
    public GameObject wButtonAbled;
    public GameObject arrowAbled;
    public GameObject arrowDisabled;
    public GameObject enabledCanvas;
    public float delayTime;
    private float startingGameTime;
    // Start is called before the first frame update
    void Start()
    {
        startingGameTime = -1f;
    }

    private void startGame()
    {

        enabledCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {

            ifFirstPlayerReady = true;
            wButtonAbled.SetActive(true);
            wButtonDisabled.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ifSecondPlayerReady = true;
            arrowAbled.SetActive(true);
            arrowDisabled.SetActive(false);
        }
        if(startingGameTime != -1f && startingGameTime < Time.time)
        {
            startGame();
        }
        if (ifFirstPlayerReady && ifSecondPlayerReady && startingGameTime == -1f)
        {
            startingGameTime = Time.time + delayTime;
        }
    }
}
