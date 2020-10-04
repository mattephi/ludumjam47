using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starter : MonoBehaviour
{
    public GameObject generator;

    void Start()
    {
        generator.GetComponent<Generator>().GenerateMatrix(10, 20);
        generator.GetComponent<Generator>().SpawnChar();
    }

}
