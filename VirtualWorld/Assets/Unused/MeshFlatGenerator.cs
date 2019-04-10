/*
 * Daniel Kulas, 2018
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class MeshFlatGenerator : MonoBehaviour
{
    public Vector3 startVector = new Vector3(0, 0, 0);
    public Vector2 meshSize = new Vector2(32, 32);
    public Vector2 tileSize = new Vector3(2, 2);
    public float tileHeight = 5f;
    public float noiseScale = 10f;
    private Vector2 offset = new Vector2(0, 0);
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Color[] colors;


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
        vertices = new Vector3[noOfVert];
        triangles = new int[noOfVert];

        for (int i = 0, current = 0; i < noOfVertX * noOfVertZ; i++)
        { 
            int x = i % noOfVertX;
            int z = (int)i / noOfVertX;
            float y;

            if (x != noOfVertX - 1 && z != noOfVertZ - 1)
            {
                y = generateHeight(x, z);
                vertices[current] = new Vector3(x * tileSize.x, y * tileHeight, z * tileSize.y) + startVector;
                y = generateHeight(x, z + 1);
                vertices[current + 1] = new Vector3(x * tileSize.x, y * tileHeight, (z + 1) * tileSize.y) + startVector;
                y = generateHeight(x + 1, z);
                vertices[current + 2] = new Vector3((x + 1) * tileSize.x, y * tileHeight, z * tileSize.y) + startVector;
                current += 3;
            }
            if (x != noOfVertX - 1 && z != 0)
            {
                y = generateHeight(x, z);
                vertices[current] = new Vector3(x * tileSize.x, y * tileHeight, z * tileSize.y) + startVector;
                y = generateHeight(x + 1, z);
                vertices[current + 1] = new Vector3((x + 1) * tileSize.x, y * tileHeight, z * tileSize.y) + startVector;
                y = generateHeight(x + 1, z - 1);
                vertices[current + 2] = new Vector3((x + 1) * tileSize.x, y * tileHeight, (z - 1) * tileSize.y) + startVector;
                current += 3;
            }
        }

        for(int i = 0; i < triangles.Length; i++)
            triangles[i] = i;
    }

    private float generateHeight(int x1, int z1)
    {
        float x = x1 / meshSize.x;
        float z = z1 / meshSize.y;
        return Mathf.PerlinNoise(x * noiseScale + offset.x, z * noiseScale + offset.y);
    }

    private void colorMesh()
    {
        colors = new Color[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            float colorHeight = Mathf.Lerp(0, 1, vertices[i].y / tileHeight) / 1.5f;
            colors[i] = new Color(0.1f + colorHeight, 0.8f - colorHeight, 0.15f);
        }
    }

    private void updateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
    }
}