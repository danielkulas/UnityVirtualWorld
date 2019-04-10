/*
 * Daniel Kulas, 2018
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InsertionObjectOnClick : MonoBehaviour
{
    public GameObject prefabToInsert;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100))
                Instantiate(prefabToInsert, hit.point, Quaternion.Euler(0, Random.Range(0, 359), 0));
        }
    }
}