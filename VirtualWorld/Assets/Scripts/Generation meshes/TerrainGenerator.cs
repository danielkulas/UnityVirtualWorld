/*
 * Daniel Kulas, 2018
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TerrainGenerator : MeshGenerator
{
    public AnimationCurve terrainColorRed; //Color of mesh depends on its height
    public AnimationCurve terrainColorGreen;
    public AnimationCurve terrainColorBlue;
    private Color[] colors;


    public override void Awake()
    {
        base.Awake();   
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    private void colorMesh()
    {
        colors = new Color[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            float offset = 0.04f;

            float colorHeight = Mathf.Lerp(0, 1, vertices[i].y / tileHeight);
            float red = Random.Range(terrainColorRed.Evaluate(colorHeight) - offset, terrainColorRed.Evaluate(colorHeight) + offset);
            float green = Random.Range(terrainColorGreen.Evaluate(colorHeight) - offset, terrainColorGreen.Evaluate(colorHeight) + offset);
            float blue = Random.Range(terrainColorBlue.Evaluate(colorHeight) - offset, terrainColorBlue.Evaluate(colorHeight) + offset);
            colors[i] = new Color(red, green, blue);
        }
    }

    public override void updateMesh()
    {
        colorMesh();
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.RecalculateNormals();
    }

    public void loadTerrain(Vector2 meshSize, Vector2 tileSize, float tileHeight, float noiseScale, Vector2 offset)
    {
        this.meshSize = meshSize;
        this.tileSize = tileSize;
        this.tileHeight = tileHeight;
        this.noiseScale = noiseScale;
        this.offset = offset;
        makeMesh();
        updateMesh();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}