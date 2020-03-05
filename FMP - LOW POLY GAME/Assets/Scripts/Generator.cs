using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Generator : MonoBehaviour
{
    // Declearing varables
    Mesh mesh;
    PerlinNoise pn;

    Vector3[] verticies;
    int[] triangles;

    public int ChunkSize;
    
    
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        pn = GetComponent<PerlinNoise>();

        Generate();
        UpdateMesh();
    }

    void Generate()
    {
        verticies = new Vector3[(ChunkSize + 1) * (ChunkSize + 1)];

        int i = 0;

        for (int z = 0; z < ChunkSize + 1; z++){
            for (int x = 0; x < ChunkSize + 1; x++)
            {
                verticies[i] = new Vector3(x, pn.PerlinMap(x, z, pn.freq, pn.amp), z);
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

        mesh.vertices = verticies;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        //Vector3[] oldVerts = mesh.vertices;
        //int[] triangles = mesh.triangles;
        //Vector3[] vertices = new Vector3[triangles.Length];
        //for (int i = 0; i < triangles.Length; i++)
        //{
        //    vertices[i] = oldVerts[triangles[i]];
        //    triangles[i] = i;
        //}
        //mesh.vertices = vertices;
        //mesh.triangles = triangles;
        //mesh.RecalculateBounds();
        //mesh.RecalculateNormals();
    }

    //void FlatShader()
    //{
    //    Vector3[] flatShadedVertices = new Vector3[triangles.Length];
    //    Vector2[] flatShadedUvs = new Vector2[triangles.Length];

    //    for (int i = 0; i < triangles.Length; i++){
    //        flatShadedVertices[i] = verticies[triangles[i]];
    //        flatShadedUvs[i] = uvs[triangles[i]];  
    //        triangles[i] = i;
    //    }
    //    verticies = flatShadedVertices;
    //    uvs = flatShadedUvs;
    //}
}
