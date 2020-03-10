using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Generator : MonoBehaviour
{
    // Declearing varables
    Mesh mesh;    
    PerlinNoise perlin;

    Vector3[] vertices;
    int[] triangles;

    public int ChunkSize;

    public float freq;
    public float amp;
    
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        perlin = GetComponent<PerlinNoise>();
        
        Generate();
        UpdateMesh();
    }

    void Generate()
    {
        vertices = new Vector3[(ChunkSize + 1) * (ChunkSize + 1)];

        int i = 0;

        for (int z = 0; z < ChunkSize + 1; z++){
            for (int x = 0; x < ChunkSize + 1; x++)
            {

                vertices[i] = new Vector3(x, perlin.PerlinMap(x, z, freq, amp), z);
                i++;
            }
        }

        triangles = new int[ChunkSize * ChunkSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < ChunkSize; z++){
            for (int x = 0; x < ChunkSize; x++)
            {                
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + ChunkSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + ChunkSize + 1;
                triangles[tris + 5] = vert + ChunkSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }        
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}
