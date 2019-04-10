/*
 * Daniel Kulas, 2018
*/
using UnityEngine;


public class PerlinNoiseGenerator : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public float scale = 10f;
    public float offsetX = 0f;
    public float offsetY = 0f;


    private void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        offsetX = Random.Range(0f, 65536f);
        offsetY = Random.Range(0f, 65536f);
        renderer.material.mainTexture = makeTexture();
    }

    private Texture makeTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = calculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }

    private Color calculateColor(int x, int y)
    {
        float pixelX = (float)x / width;
        float pixelY = (float)y / height;
        float pixel = Mathf.PerlinNoise(pixelX * scale + offsetX, pixelY * scale + offsetY);
        return new Color(pixel, pixel, pixel);
    }
}