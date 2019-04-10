/*
 * Daniel Kulas, 2018
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimalScript : MonoBehaviour
{
    #region Variables
    public int jumpForce;
    public int speed;
    private Rigidbody rb;
    //private int health;
    //private int force;
    //private short age;
    private bool grounded;
    public short dir;
    private int layerMask = 1 << 8; //Terrain
    #endregion


    #region Unity callbacks
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, 0, 0);
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        move();
    }

    private void Update()
    {
        if (transform.position.y < -1)
            Destroy(this.gameObject);

        checkTerrainCollision();
    }
    #endregion

    #region Moving methods
    private void move()
    {
        rb.AddRelativeForce(Vector3.up * jumpForce, ForceMode.Impulse);
        rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Impulse);
        changeDir();
        grounded = false;
        rb.useGravity = true;
    }

    private void checkTerrainCollision()
    {
        checkLimits();
        rotateToDir();

        if (grounded == false)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, rb.velocity, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.distance < 0.2f)
                {
                    rb.useGravity = false;
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    grounded = true;
                    setAngle();
                    move();
                }
            }
        }
    }

    private void rotateToDir()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        short rotation = (short)Random.Range(8, 32); //Direction change

        if (dir >= 0 && dir < 180 + transform.rotation.eulerAngles.y)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rot.x, rot.y - rotation, rot.z), Time.deltaTime * 0.5f);
        else
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rot.x, rot.y + rotation, rot.z), Time.deltaTime * 0.5f);
    }

    private void setAngle()
    {
        Vector3 rot = transform.rotation.eulerAngles;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity))
        {
            Quaternion reflection = Quaternion.FromToRotation(Vector3.up, hit.normal);
            Quaternion rotation = Quaternion.Euler(reflection.eulerAngles.x, rot.y, reflection.eulerAngles.z);

            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 8);
        }
    }

    private void checkLimits()
    {
        Vector3 rot = transform.rotation.eulerAngles;

        if (transform.position.x < 5 || transform.position.x > 95 || transform.position.z < 5 || transform.position.z > 95)
        {
            if (transform.position.x < 5)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rot.x, 90, rot.z), Time.deltaTime * 0.5f);
            else if (transform.position.x > 95)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rot.x, 270, rot.z), Time.deltaTime * 0.5f);

            if (transform.position.z < 5)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rot.x, 0, rot.z), Time.deltaTime * 0.5f);
            else if (transform.position.z > 95)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rot.x, 180, rot.z), Time.deltaTime * 0.5f);
        }
    }

    private void changeDir()
    {
        int random = Random.Range(0, 100);

        if (random > 90)
            dir = (short)Random.Range(0, 359);
    }
    #endregion
}