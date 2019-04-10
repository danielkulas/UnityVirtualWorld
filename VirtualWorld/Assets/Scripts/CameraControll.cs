/*
 * Daniel Kulas, 2018
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraControll : MonoBehaviour
{
    #region variables
    public Transform Environment;
    public GameObject Terrain;
    public int cameraSpeed = 10;
    public int cameraRotationSpeed = 10;
    public int cameraZoomSpeed = 10;
    private int heightScale = 1;
    private float sizeXTerrain;
    private float sizeZTerrain;
    private Vector3 minPos = Vector3.zero; //Min and max camera position
    private Vector3 maxPos = Vector3.zero;
    private Vector3 forward = Vector3.zero; //Scrolling vector
    private Vector3 position;
    private float offset = 0.125f;
    #endregion


    #region Unity callbacks
    private void Awake()
    {
        TerrainGenerator terrainG = Terrain.GetComponent<TerrainGenerator>();
        heightScale = (int)Environment.transform.localScale.y;
        sizeXTerrain = terrainG.meshSize.x * terrainG.tileSize.x;
        sizeZTerrain = terrainG.meshSize.y * terrainG.tileSize.y;
        minPos.y = (terrainG.tileHeight * heightScale) + 3;
        maxPos.y = minPos.y + (4 * heightScale);
        maxPos.x = sizeXTerrain;
        maxPos.z = sizeZTerrain;
        transform.position = new Vector3(sizeXTerrain / 2, minPos.y + 1, sizeZTerrain / 4);
    }

    private void Update()
    {
        position = transform.position;
        moving();
        scrolling();
        rotatating();
        stayInLimits();
    }
    #endregion

    #region Moving
    private void moving()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        vertical *= Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.x);
        float upCompensate = Input.GetAxis("Vertical") * Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.x);
        Vector3 nextPos = Vector3.right * horizontal + Vector3.forward * vertical + Vector3.up * upCompensate;
        nextPos = transform.TransformDirection(nextPos).normalized;
        position += nextPos * cameraSpeed * Time.deltaTime;
    }
    #endregion

    #region Scrolling
    private void scrolling()
    {
        float scrollRotation = Input.GetAxis("Mouse ScrollWheel") * cameraZoomSpeed;
        if (scrollRotation != 0)
            forward = transform.TransformDirection(Vector3.forward).normalized * scrollRotation + transform.position;

        if (Vector3.Distance(transform.position, forward) < 1)
            forward = Vector3.zero;

        if (forward != Vector3.zero)
        {
            if (forward.x < minPos.x)
                forward.x = minPos.x + offset;
            else if (forward.x > maxPos.x)
                forward.x = maxPos.x - offset;

            if (forward.y < minPos.y)
                forward.y = minPos.y + offset;
            else if (forward.y > maxPos.y)
                forward.y = maxPos.y - offset;

            if (forward.z < minPos.z)
                forward.z = minPos.z + offset;
            else if (forward.z > maxPos.z)
                forward.z = maxPos.z - offset;

            if (forward.y == minPos.y + offset || forward.y == maxPos.y - offset)
            {
                forward.x = position.x;
                forward.z = position.z;
            }

            if (!((position.y >= maxPos.y - 3 && scrollRotation < 0) || (position.y <= minPos.y + 3 && scrollRotation > 0)))
                position = Vector3.Lerp(transform.position, forward, Time.deltaTime * 8f);
        }
    }
    #endregion

    #region Rotating
    private void rotatating()
    {
        float horizontalRotation = Input.GetAxis("HorizontalRotation") * cameraRotationSpeed;
        if (horizontalRotation != 0)
            transform.Rotate(Vector3.up, Time.deltaTime * horizontalRotation, Space.World);

        //With middle mouse button pressed
        if (Input.GetKey(KeyCode.Mouse2))
        {
            float mouseXRotation = Input.GetAxis("Mouse X") * cameraRotationSpeed;
            transform.Rotate(Vector3.up, Time.deltaTime * mouseXRotation, Space.World);

            float mouseYRotation = Input.GetAxis("Mouse Y") * cameraRotationSpeed;
            transform.Rotate(Vector3.right, Time.deltaTime * -mouseYRotation, Space.Self);
        }
    }
    #endregion

    #region Limits
    //Limits - doesn't allow to go out of the limit
    private void stayInLimits()
    {
        if (position.x < minPos.x) //x
            position.x = minPos.x + offset;
        if (position.x > maxPos.x)
            position.x = maxPos.x - offset;

        if (position.y < minPos.y) //y
            position.y = minPos.y + offset;
        if (position.y > maxPos.y)
            position.y = maxPos.y - offset;

        if (position.z < minPos.z) //z
            position.z = minPos.z + offset;
        if (position.z > maxPos.z)
            position.z = maxPos.z - offset;

        transform.position = position;
    }
    #endregion
}