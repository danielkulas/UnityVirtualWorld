/*
 * Daniel Kulas, 2018
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaterGenerator : MeshGenerator
{
    public float waterSpeed = 0.02f;
    public int waveSize = 8; //Difference between max and min "noiseScale"
    private float noiseScaleAtStart;
    private short flag = 1; //if (flag == 1) "noiseScale" grows, if (flag == -1) "noiseScale" decreases


    public override void Awake()
    {
        base.Awake();
        noiseScaleAtStart = noiseScale;
    }

    private void Update()
    {
        if (noiseScale - noiseScaleAtStart < -waveSize)
            flag = 1;
        else if (noiseScale - noiseScaleAtStart > waveSize)
            flag = -1;

        noiseScale = noiseScale + waterSpeed * flag;
        updateVertices();
    }

    private void updateVertices()
    {
        for (int i = 0, current = 0; i < noOfVertX * noOfVertZ; i++)
        {
            int x = i % noOfVertX;
            int z = (int)i / noOfVertX;

            if (x != noOfVertX - 1 && z != noOfVertZ - 1)
            {
                vertices[current].y = generateHeight(x, z) * tileHeight;
                vertices[current + 1].y = generateHeight(x, z + 1) * tileHeight;
                vertices[current + 2].y = generateHeight(x + 1, z) * tileHeight;
                current += 3;
            }
            if (x != noOfVertX - 1 && z != 0)
            {
                vertices[current].y = generateHeight(x, z) * tileHeight;
                vertices[current + 1].y = generateHeight(x + 1, z) * tileHeight;
                vertices[current + 2].y = generateHeight(x + 1, z - 1) * tileHeight;
                current += 3;
            }
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }
}