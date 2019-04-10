/*
 * Daniel Kulas, 2018
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
abstract public class MeshGenerator : MonoBehaviour
{
    #region Variables
    public Vector2 meshSize = new Vector2(100, 100); //Number of edges(x, y)
    public Vector2 tileSize = new Vector3(1, 1); //Distance from each two vertices
    public float tileHeight = 4; 
    public float noiseScale = 8f; //Scale of perlin noise
    protected Vector2 offset; //Offset of perlin noise
    protected Mesh mesh; //Genereted mesh
    protected Vector3[] vertices;
    protected int[] triangles;
    protected int noOfVertX;
    protected int noOfVertZ;
    #endregion


    public virtual void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        offset.x = Random.Range(0, 32768);
        offset.y = Random.Range(0, 32768);
        makeMesh();
        updateMesh();
    }

    protected void makeMesh()
    {
        noOfVertX = (int)meshSize.x + 1;
        noOfVertZ = (int)meshSize.y + 1;
        int noOfVert = (int)meshSize.x * (int)meshSize.y * 2 * 3; //2 * 3 - number of vertices in every square(two triangles)
        vertices = new Vector3[noOfVert];
        triangles = new int[noOfVert];

        for (int i = 0, current = 0; i < noOfVertX * noOfVertZ; i++)
        {
            int x = i % noOfVertX;
            int z = i / noOfVertX;
            float y;

            if (x != noOfVertX - 1 && z != noOfVertZ - 1)
            {
                y = generateHeight(x, z);
                vertices[current] = new Vector3(x * tileSize.x, y * tileHeight, z * tileSize.y);
                y = generateHeight(x, z + 1);
                vertices[current + 1] = new Vector3(x * tileSize.x, y * tileHeight, (z + 1) * tileSize.y);
                y = generateHeight(x + 1, z);
                vertices[current + 2] = new Vector3((x + 1) * tileSize.x, y * tileHeight, z * tileSize.y);
                current += 3;
            }
            if (x != noOfVertX - 1 && z != 0)
            {
                y = generateHeight(x, z);
                vertices[current] = new Vector3(x * tileSize.x, y * tileHeight, z * tileSize.y);
                y = generateHeight(x + 1, z);
                vertices[current + 1] = new Vector3((x + 1) * tileSize.x, y * tileHeight, z * tileSize.y);
                y = generateHeight(x + 1, z - 1);
                vertices[current + 2] = new Vector3((x + 1) * tileSize.x, y * tileHeight, (z - 1) * tileSize.y);
                current += 3;
            }
        }

        for (int i = 0; i < triangles.Length; i++)
            triangles[i] = i;
    }

    protected float generateHeight(int x1, int z1)
    {
        float x = x1 / meshSize.x;
        float z = z1 / meshSize.y;
        return Mathf.PerlinNoise(x * noiseScale + offset.x, z * noiseScale + offset.y);
    }

    virtual public void updateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    public float GetOffsetX()
    {
        return offset.x;
    }

    public float GetOffsetY()
    {
        return offset.y;
    }
}