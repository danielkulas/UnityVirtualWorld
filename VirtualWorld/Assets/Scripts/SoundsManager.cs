/*
 * Daniel Kulas 2018     
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundsManager : MonoBehaviour
{
    public Transform MainCamera;


    private void Update()
    {
        transform.position = new Vector3(MainCamera.transform.position.x, transform.position.y, MainCamera.transform.position.z);
    }
}
