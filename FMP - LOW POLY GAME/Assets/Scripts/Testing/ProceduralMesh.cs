using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralMesh : MonoBehaviour
{
    //Declare varibles
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    [SerializeField]
    bool isFlatShaded;

    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }
    // Start is called before the first frame update
    void Start()
    {
        MakeMeshData();
        if (isFlatShaded) FlatShaing();
        CreateMesh();
    }

    void MakeMeshData()
    {
        //Create an array of vertices
        vertices = new Vector3[] {
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 1)
        };

        //Create an array of interger
        triangles = new int[]
        {
            0, 1, 2,
            2, 1, 3
        };
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

        this.gameObject.AddComponent<MeshCollider>();
    }
}
