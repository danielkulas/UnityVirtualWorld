/*
 * Daniel Kulas, 2018
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshFlatListGenerator : MonoBehaviour
{
    public Vector3 startVector = new Vector3(0, 0, 0);
    public Vector2 meshSize = new Vector2(32, 32);
    public Vector2 tileSize = new Vector3(2, 2);
    public float tileHeight = 5f;
    public float noiseScale = 10f;
    private Vector2 offset = new Vector2(0, 0);
    private Mesh mesh;
    private List<Vector3> verticesL; //Lists
    private List<int> trianglesL;
    private List<Color> colorsL;


    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        offset.x = Random.Range(0, 32768);
        offset.y = Random.Range(0, 32768);
        makeMesh();
        colorMesh();
        updateMesh();
        mesh.RecalculateNormals();
    }

    private void makeMesh()
    {
        int noOfVertX = (int)meshSize.x + 1;
        int noOfVertZ = (int)meshSize.y + 1;
        int noOfVert = (int)meshSize.x * (int)meshSize.y * 2 * 3;
        verticesL = new List<Vector3>();
        trianglesL = new List<int>();

        for (int i = 0; i < noOfVertX * noOfVertZ; i++)
        {
            int x = i % noOfVertX;
            int z = (int)i / noOfVertX;
            float y;

            if (x != noOfVertX - 1 && z != noOfVertZ - 1)
            {
                y = generateHeight(x, z);
                verticesL.Add(new Vector3(x * tileSize.x, y * tileHeight, z * tileSize.y) + startVector);
                y = generateHeight(x, z + 1);
                verticesL.Add(new Vector3(x * tileSize.x, y * tileHeight, (z + 1) * tileSize.y) + startVector);
                y = generateHeight(x + 1, z);
                verticesL.Add(new Vector3((x + 1) * tileSize.x, y * tileHeight, z * tileSize.y) + startVector);
            }
            if (x != noOfVertX - 1 && z != 0)
            {
                y = generateHeight(x, z);
                verticesL.Add(new Vector3(x * tileSize.x, y * tileHeight, z * tileSize.y) + startVector);
                y = generateHeight(x + 1, z);
                verticesL.Add(new Vector3((x + 1) * tileSize.x, y * tileHeight, z * tileSize.y) + startVector);
                y = generateHeight(x + 1, z - 1);
                verticesL.Add(new Vector3((x + 1) * tileSize.x, y * tileHeight, (z - 1) * tileSize.y) + startVector);
            }
        }

        for (int i = 0; i < noOfVert; i++)
            trianglesL.Add(i);
    }

    private float generateHeight(int x1, int z1)
    {
        float x = x1 / meshSize.x;
        float z = z1 / meshSize.y;
        return Mathf.PerlinNoise(x * noiseScale + offset.x, z * noiseScale + offset.y);
    }

    private void colorMesh()
    {
        colorsL = new List<Color>();
        int noOfVert = (int)meshSize.x * (int)meshSize.y * 2 * 3;

        for (int i = 0; i < noOfVert; i++)
        {
            float colorHeight = Mathf.Lerp(0, 1, verticesL[i].y / tileHeight) / 1.5f;
            colorsL.Add(new Color(0.1f + colorHeight, 0.8f - colorHeight, 0.15f));
        }
    }

    private void updateMesh()
    {
        mesh.Clear();
        mesh.vertices = verticesL.ToArray();
        mesh.triangles = trianglesL.ToArray();
        mesh.colors = colorsL.ToArray();
    }
}