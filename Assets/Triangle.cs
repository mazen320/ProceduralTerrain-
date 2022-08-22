using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour
{
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;

    int mapWidth = 500;
    int mapHeight = 100;


    void Start()
    {
        meshRenderer = gameObject.AddComponent<MeshRenderer>(); //instead of doing it in the inspector
        meshFilter = gameObject.AddComponent<MeshFilter>();
        /*
        meshFilter.mesh.vertices = new Vector3[]
        {
            /*
               new Vector3(-1, 0 , -1),   //0 
               new Vector3(-1, 0 , 1), //1
               new Vector3(1, 0 , 1),//2
               new Vector3(1, 0 , -1),//3
               new Vector3(-1, 0 , -1),//4
               new Vector3(1, 0 , 1),//5
            */ /*
            new Vector3(-1, 0 , -1),
            new Vector3(-1, 0 , 1),
            new Vector3(1, 0 , 1),
            new Vector3(1, 0 , -1),

        };

        meshFilter.mesh.triangles = new int[]
{   
        0,
        1,
        2,
        2,
        3,
        0,
};
*/
        int height = 10;
        int width = 500;
        
        Vector3[] verts = new Vector3[mapWidth * mapHeight * 6];
        Vector2[] uvs = new Vector2[mapWidth * mapHeight * 6];
        int[] triangles = new int[mapWidth * mapHeight * 6];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {

                int tileIndex = x + y * width;

                verts[tileIndex * 6 + 0] = new Vector3(x, 0, y);
                verts[tileIndex * 6 + 1] = new Vector3(x + 1, 0, y);
                verts[tileIndex * 6 + 2] = new Vector3(x + 1, 0, y + 1);

                verts[tileIndex * 6 + 3] = new Vector3(x, 0, y);
                verts[tileIndex * 6 + 4] = new Vector3(x + 1, 0, y + 1);
                verts[tileIndex * 6 + 5] = new Vector3(x, 0, y + 1);

                uvs[tileIndex * 6 + 0] = new Vector2(0, 0);
                uvs[tileIndex * 6 + 1] = new Vector2(1, 0);
                uvs[tileIndex * 6 + 2] = new Vector2(1, 1);

                uvs[tileIndex * 6 + 3] = new Vector2(0, 0);
                uvs[tileIndex * 6 + 4] = new Vector2(1, 1);
                uvs[tileIndex * 6 + 5] = new Vector2(0, 1);

                for (int t = 0; t < 6; t++)
                {
                    triangles[tileIndex * 6 + t] = tileIndex * 6 + 5 - t;
                }
            }
        }

        meshFilter.mesh.vertices = verts;
        meshFilter.mesh.uv = uvs;
        meshFilter.mesh.triangles = triangles;
       // meshFilter.mesh.RecalculateNormals();
    }
}
