using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshGen : MonoBehaviour
{
    //Declare varibles
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    public int chunkSize;

    public float freq;
    public float amp;

    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    // Start is called before the first frame update
    void Start()
    {
        MakeMeshData();
        FlatShaing();
        CreateMesh();
    }

    void MakeMeshData()
    {
        vertices = new Vector3[chunkSize * chunkSize * 4];
        triangles = new int[chunkSize * chunkSize * 6];

        int vert = 0;
        int tris = 0;

        int i = 0;

        for (int z = 0; z < chunkSize + 1; z++)
        {
            for (int x = 0; x < chunkSize + 1; x++)
            {
                float y = Mathf.PerlinNoise(x / freq, z / freq) * amp;
                vertices[i] = new Vector3(x, y, z);
                //Debug.Log("vertice " + i + ": " + vertices[i]);
                i++;
            }
        }

        for (int z = 0; z < chunkSize; z++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + chunkSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + chunkSize + 1;
                triangles[tris + 5] = vert + chunkSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    void FlatShaing()
    {
        Vector3[] flatShadedVertices = new Vector3[triangles.Length];
        for (int i = 0; i < triangles.Length; i++)
        {
            flatShadedVertices[i] = vertices[triangles[i]];
            triangles[i] = i;
        }
        vertices = flatShadedVertices;
    }

    void CreateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}
