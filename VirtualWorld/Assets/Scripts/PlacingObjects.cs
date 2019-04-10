/*
 * Daniel Kulas, 2018
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlacingObjects : MonoBehaviour
{
    #region Structs
    [System.Serializable]
    public struct Objects
    {
        public Transform[] objectPrefabs;
        public Material[] objectMaterials;
        public int numberOfObjects; //Number of objects of each prefab
        public string objectName;
        public bool onlyOnLand; //Can the object be in the water(true - yes)
        public bool staticObj;
        public bool animal;
    }
    #endregion  

    #region Variables
    public GameObject Terrain;
    public LayerMask myLayerMask;
    public Objects[] objects;
    private Vector3 terrainSize;
    #endregion


    #region Unity callbacks
    private void Awake()
    {
        if (PlayerPrefs.GetInt("GameToLoad") == 1)
            this.GetComponent<PlacingObjects>().enabled = false;
    }

    private void Start()
    {
        TerrainGenerator terrain = Terrain.GetComponent<TerrainGenerator>();
        terrainSize.x = terrain.meshSize.x * terrain.tileSize.x;
        terrainSize.y = terrain.tileHeight * transform.localScale.y;
        terrainSize.z = terrain.meshSize.y * terrain.tileSize.y;

        placeObjects();
    }
    #endregion

    #region Placing objects
    private void placeObjects()
    {
        for (int i = 0; i < objects.Length; i++) //Number of objects types(tree, flower, grass etc)
        {
            for (int j = 0; j < objects[i].objectPrefabs.Length; j++) //Number of object 3d models(first tree model, second tree model etc.)
            {
                for (int k = 0; k < objects[i].numberOfObjects; k++) //(e.g. how many trees)
                {
                    Transform it = GameObject.Instantiate(objects[i].objectPrefabs[j], setPosition(), setRandomRotation()) as Transform;

                    string objectName = objects[i].objectName;
                    int materialNumber = setMaterial(it, objects[i].objectMaterials);
                    it.name = objectName + j + materialNumber + k; //Setting object parameters
                    it.transform.localScale = setRandomScale((int)it.transform.localScale.x); //Setting object parameters
                    checkIfChildrenExist(objectName); //To group. Appends as child(to clean up in inspector)
                    it.transform.parent = transform.Find(objectName);
                    setStatic(it, objectName, i, j, k, materialNumber);

                    if (objects[i].onlyOnLand == true)
                        deleteObjectFromWater(it, objectName, j, k, materialNumber);
                    else
                        setRotation(it);
                }
            }
        }
    }

    private void deleteObjectFromWater(Transform obj, string objectName, int j, int k, int materialNumber)
    {
        //Checking if plant is in the water
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(obj.transform.position.x, obj.transform.position.y + terrainSize.y, obj.transform.position.z), -Vector3.up, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.tag == "Water")
                Destroy(GameObject.Find(objectName + j + materialNumber + k));
        }
    }
    #endregion

    #region Methods. Setting object parameters
    private Vector3 setPosition()
    {
        float x = Random.Range(0, terrainSize.x);
        float y = terrainSize.y + 1;
        float z = Random.Range(0, terrainSize.z);

        RaycastHit hit;
        if (Physics.Raycast(new Vector3(x, y, z), -Vector3.up, out hit, Mathf.Infinity, myLayerMask))
            y = y - hit.distance;

        return new Vector3(x, y, z);
    }

    private void setRotation(Transform obj)
    {
        obj.transform.Rotate(Vector3.up * Random.Range(0, 359), Space.Self);
    }

    /*private void setAngle(Transform obj)
    {
        Vector3 rot = transform.rotation.eulerAngles;

        RaycastHit hit;
        Vector3 raycastPos = new Vector3(obj.transform.position.x, obj.transform.position.y + 5, obj.transform.position.z);
        if (Physics.Raycast(raycastPos, -Vector3.up, out hit, Mathf.Infinity))
        {
            Quaternion reflection = Quaternion.FromToRotation(Vector3.up, hit.normal);
            Quaternion rotation = Quaternion.Euler(reflection.eulerAngles.x, reflection.eulerAngles.y, reflection.eulerAngles.z);

            obj.transform.rotation = rotation;
        }
    }*/

    private Quaternion setRandomRotation()
    {
        return Quaternion.Euler(0, Random.Range(0, 359), 0);
    }

    private Vector3 setRandomScale(int baseScale)
    {
        return new Vector3(Random.Range(baseScale / 1.2f, baseScale * 1.2f), Random.Range(baseScale / 1.2f, baseScale * 1.2f), Random.Range(baseScale / 1.2f, baseScale * 1.2f));
    }

    private Vector3 setYScale(int baseScale)
    {
        float scale = Random.Range(baseScale / 1.2f, baseScale * 1.2f);
        float scaleY = Random.Range(baseScale / 1.5f, baseScale * 1.5f);

        return new Vector3(scale, scaleY, scale);
    }

    private int setMaterial(Transform obj, Material[] objectMaterials)
    {
        if (objectMaterials.Length != 0)
        {
            int materialNumber = Random.Range(0, objectMaterials.Length);
            if (obj.childCount == 0)
                obj.GetComponent<MeshRenderer>().material = objectMaterials[materialNumber];
            else
                obj.GetChild(0).GetComponent<MeshRenderer>().material = objectMaterials[materialNumber];

            return materialNumber;
        }

        return 0;
    }

    private void setStatic(Transform obj, string objectName, int i, int j, int k, int materialNumber)
    {
        if (objects[i].staticObj == true)
            GameObject.Find(objectName + j + materialNumber + k).isStatic = true;
    }

    private void checkIfChildrenExist(string name)
    {
        if (!transform.Find(name))
        {
            GameObject newObject = new GameObject();
            newObject.transform.parent = transform;
            newObject.transform.SetAsFirstSibling();
            newObject.transform.name = name;
        }
    }
    #endregion
}