using System;
using UnityEngine;

public class abc : MonoBehaviour
{
    int [] array = new int[5];
    void fillArray()
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = UnityEngine.Random.Range(0, 5);
        }
    }

    private void Awake()
    {
        fillArray();
        foreach (int i in array)
        {
            Debug.Log(i);
        }
    }
}