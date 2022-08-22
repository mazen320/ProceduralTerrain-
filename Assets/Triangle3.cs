using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using API;

public class Triangle3 : MonoBehaviour
{
    Mesh mesh; // new Mesh
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    [SerializeField] Material material; //material that will be used
    [SerializeField] float elevationFactor;
    [SerializeField] Texture2D heightMap;

    Vector3[] vertices; // vertices we gonna assign to the Mesh
    int[] triangles; // triangles we gonna assign to the Mesh
    public int xSize = 20; // Grid X size
    public int zSize = 20; // Grid Z size   

    public int minHeight;
    public int maxHeight;
    const int rectangleIndex = 6;

    void Start()
    {
        RandomAPI aPI = new RandomAPI();
        aPI.Speak();

        mesh = new Mesh();

        meshRenderer = gameObject.AddComponent<MeshRenderer>(); //instead of doing it in the inspector
        meshFilter = gameObject.AddComponent<MeshFilter>();
        GetComponent<MeshFilter>().mesh = mesh; // Assign created Mesh to the game component
        GetComponent<MeshRenderer>().material = material; //assigning the material
        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)]; // Create the vertices for the grid.
                                                           // x and z grid vertices N are always +1 the number of squares along x and z axis,
                                                           // so if the grid is 20x20 squares big, vertices count will be (20+1)x(20+1)

        float ratioX = (float)heightMap.width / (float)xSize;
        float ratioZ = (float)heightMap.height / (float)zSize;

        // Create the grid using two cascading loops
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                // float y = Mathf.PerlinNoise(x * elevationFactor, z * elevationFactor);// * 2f; // Variable used to make the mesh surface uneven
                Color color = heightMap.GetPixel(x, z);

                vertices[i] = new Vector3(x, color.r * elevationFactor, z);
                
            }
        }

        triangles = new int[xSize * zSize * rectangleIndex]; //20 * 20 * 6 = 2400, and rectangle index is 6
        int vert = 0; // Track vertices co-ords
        int tris = 0; // Track triangles. Used to create new triagles on each new row

        // Fill the grid with quads with cascading loops
        for (int z = 0; z < zSize; z++) // Create quads along the z-axis
        {
            // Copy 1st squad along grid x-axis
            for (int x = 0; x < xSize; x++)
            {
                // Create 2 new clockwise triangle to create a quad for one square of the grid
                // Starting from 1st triangle, tris = 0, vert =0.
                triangles[tris + 0] = vert + 0; // Starting from 0 (Grid start point)
                triangles[tris + 1] = vert + xSize + 1; // Go 1 up along x-axis (clockwise), which equals to 20+1 points
                triangles[tris + 2] = vert + 1; // Goto 1 along z
                                                // 2nd triangle
                triangles[tris + 3] = vert + 1; // Same point as triangles[2]
                triangles[tris + 4] = vert + xSize + 1; // Same point as triangles[1]
                triangles[tris + 5] = vert + xSize + 2;

                vert++; // Update co-ords to create new quads
                tris += 6;
            }

            vert++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        // Assign vertices and triangles we've made to the Mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals(); // Correct coordinates system for correct lighting       // we going to do this manually
    }
}