/*
 * Daniel Kulas, 2018
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SavingLoadingManager : MonoBehaviour
{
    #region Structs and variables
    [System.Serializable]
    public struct ObjData
    {
        public Transform[] objectPrefabs;
        public Material[] objectMaterials;
        public string objName;
        public bool staticObj;
    }


    public Transform Environment;
    public ObjData[] objData;
    #endregion


    #region Saving
    public void saveGame()
    {
        saveTerrain();

        for (int i = 0; i < Environment.childCount; i++)
        {
            if (Environment.GetChild(i).name != "Terrain" && Environment.GetChild(i).name != "Water")
                saveObject(Environment.GetChild(i));
        }
    }

    private void saveObject(Transform obj)
    {
        PlayerPrefs.SetInt(obj.name, obj.childCount);

        for (int i = 0; i < obj.childCount; i++)
        {
            Transform child = obj.GetChild(i);
            PlayerPrefs.SetString(obj.name + i, child.name);
            PlayerPrefs.SetFloat(child.name + "PosX", child.transform.position.x);
            PlayerPrefs.SetFloat(child.name + "PosY", child.transform.position.y);
            PlayerPrefs.SetFloat(child.name + "PosZ", child.transform.position.z);
            PlayerPrefs.SetFloat(child.name + "RotX", child.transform.rotation.x);
            PlayerPrefs.SetFloat(child.name + "RotY", child.transform.rotation.y);
            PlayerPrefs.SetFloat(child.name + "RotZ", child.transform.rotation.z);
            PlayerPrefs.SetFloat(child.name + "RotW", child.transform.rotation.w);
            PlayerPrefs.SetFloat(child.name + "ScaleX", child.transform.localScale.x);
            PlayerPrefs.SetFloat(child.name + "ScaleY", child.transform.localScale.y);
            PlayerPrefs.SetFloat(child.name + "ScaleZ", child.transform.localScale.z);
        }
    }

    private void saveTerrain()
    {
        TerrainGenerator terrainGen = Environment.Find("Terrain").GetComponent<TerrainGenerator>();
        PlayerPrefs.SetFloat("TerrainMeshSizeX", terrainGen.meshSize.x);
        PlayerPrefs.SetFloat("TerrainMeshSizeY", terrainGen.meshSize.y);
        PlayerPrefs.SetFloat("TerrainTileSizeX", terrainGen.tileSize.x);
        PlayerPrefs.SetFloat("TerrainTileSizeY", terrainGen.tileSize.y);
        PlayerPrefs.SetFloat("TerrainTileHeight", terrainGen.tileHeight);
        PlayerPrefs.SetFloat("TerrainNoiseScale", terrainGen.noiseScale);
        PlayerPrefs.SetFloat("TerrainOffsetX", terrainGen.GetOffsetX());
        PlayerPrefs.SetFloat("TerrainOffsetY", terrainGen.GetOffsetY());
    }
    #endregion


    #region Loading
    private void Start()
    {
        if (PlayerPrefs.GetInt("GameToLoad") == 1)
        {
            loadTerrain();

            for (int i = 0; i < objData.Length; i++)
                loadObjects(i);
        }
    }

    private void loadTerrain()
    {
        Vector2 meshSize = new Vector2(PlayerPrefs.GetFloat("TerrainMeshSizeX"), PlayerPrefs.GetFloat("TerrainMeshSizeY"));
        Vector2 tileSize = new Vector2(PlayerPrefs.GetFloat("TerrainTileSizeX"), PlayerPrefs.GetFloat("TerrainTileSizeY"));
        float tileHeight = PlayerPrefs.GetFloat("TerrainTileHeight");
        float noiseScale = PlayerPrefs.GetFloat("TerrainNoiseScale");
        Vector2 offset = new Vector2(PlayerPrefs.GetFloat("TerrainOffsetX"), PlayerPrefs.GetFloat("TerrainOffsetY"));

        TerrainGenerator terrainGen = Environment.Find("Terrain").GetComponent<TerrainGenerator>();
        terrainGen.loadTerrain(meshSize, tileSize, tileHeight, noiseScale, offset);
    }

    private void loadObjects(int i)
    {
        int noOfObj = PlayerPrefs.GetInt(objData[i].objName);
        createEmptyObject(objData[i].objName);

        for (int j = 0; j < noOfObj; j++)
        {
            string name = PlayerPrefs.GetString(objData[i].objName + j);
            int prefabNumber = name[objData[i].objName.Length] - '0';
            int materialNumber = name[objData[i].objName.Length + 1] - '0';

            if (objData[i].objectPrefabs.Length > prefabNumber)
            {
                Transform it = GameObject.Instantiate(objData[i].objectPrefabs[prefabNumber], setPosition(name), setRotation(name)) as Transform;
                it.name = name;
                it.localScale = setScale(name);
                it.transform.parent = Environment.transform.Find(objData[i].objName);
                it.transform.SetAsFirstSibling();
                setMaterial(it, i, materialNumber);
                if (objData[i].staticObj == true && GameObject.Find(name))
                    GameObject.Find(name).isStatic = true;
            }
        }
    }

    private void createEmptyObject(string name)
    {
        if (!Environment.Find(name))
        {
            GameObject newObject = new GameObject();
            newObject.transform.parent = Environment;
            newObject.transform.SetAsFirstSibling();
            newObject.transform.name = name;
        }
    }

    private Vector3 setPosition(string name)
    {
        Vector3 pos;
        pos.x = PlayerPrefs.GetFloat(name + "PosX");
        pos.y = PlayerPrefs.GetFloat(name + "PosY");
        pos.z = PlayerPrefs.GetFloat(name + "PosZ");

        return pos;
    }

    private Quaternion setRotation(string name)
    {
        Quaternion rot;
        rot.x = PlayerPrefs.GetFloat(name + "RotX");
        rot.y = PlayerPrefs.GetFloat(name + "RotY");
        rot.z = PlayerPrefs.GetFloat(name + "RotZ");
        rot.w = PlayerPrefs.GetFloat(name + "RotW");

        return rot;
    }

    private Vector3 setScale(string name)
    {
        Vector3 scale;
        scale.x = PlayerPrefs.GetFloat(name + "ScaleX");
        scale.y = PlayerPrefs.GetFloat(name + "ScaleY");
        scale.z = PlayerPrefs.GetFloat(name + "ScaleZ");

        return scale;
    }

    private void setMaterial(Transform obj, int i, int materialNumber)
    {
        if (objData[i].objectMaterials.Length > materialNumber)
        {
            if (obj.childCount == 0)
                obj.GetComponent<MeshRenderer>().material = objData[i].objectMaterials[materialNumber];
            else
                obj.GetChild(0).GetComponent<MeshRenderer>().material = objData[i].objectMaterials[materialNumber];
        }
    }
    #endregion
}