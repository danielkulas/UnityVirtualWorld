/*
 * Daniel Kulas, 2018
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class MeshSmoothGenerator : MonoBehaviour
{
    public Vector3 startVector = new Vector3(0, 0, 0);
    public Vector2 meshSize = new Vector2(32, 32);
    public Vector2 tileSize = new Vector3(2, 2);
    public float heightScale = 5f;
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
        vertices = new Vector3[noOfVertX * noOfVertZ];
        triangles = new int[(int)meshSize.x * (int)meshSize.y * 2 * 3];


        for (int i = 0; i < vertices.Length; i++)
        {
            int x = i % noOfVertX;
            int z = (int)i / noOfVertX;
            float y = generateHeight(x, z);
            vertices[i] = new Vector3(x * tileSize.x, y * heightScale, z * tileSize.y) + startVector;
        }

        int current = 0;
        for (int i = 0; i < triangles.Length; i += 6, current++)
        {
            triangles[i] = current;
            triangles[i + 1] = current + noOfVertX;
            triangles[i + 2] = current + 1;
            triangles[i + 3] = current + 1;
            triangles[i + 4] = current + noOfVertX;
            triangles[i + 5] = current + noOfVertX + 1;

            if ((current + 1) % noOfVertX == noOfVertX - 1)
                current++;

            if ((current + 1) / noOfVertX == noOfVertZ - 1)
                break;
        }
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
            float colorHeight = Mathf.Lerp(0, 1, vertices[i].y / heightScale) / 1.5f;
            colors[i] = new Color(colorHeight, 1 - colorHeight, 0);
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