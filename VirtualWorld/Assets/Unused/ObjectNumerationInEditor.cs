/*
 * Daniel Kulas, 2018
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectNumerationInEditor : MonoBehaviour
{
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).name = transform.name + i;
    }
}