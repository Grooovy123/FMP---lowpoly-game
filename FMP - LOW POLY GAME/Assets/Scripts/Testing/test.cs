using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
   
    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }
    // Start is called before the first frame update
    void Start()
    {
        vertices = new Vector3[mesh.vertexCount * mesh.vertexCount];
        int i = 0;
        for (int z = 0; z < mesh.vertexCount; z++)
        {
            for (int x = 0; x < mesh.vertexCount; x++)
            {
                float y = Mathf.PerlinNoise(x / 4, z / 4) * 6;
                y+= Mathf.PerlinNoise(x / 2, z / 2) * 2;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        mesh.Clear();

        mesh.vertices = vertices;
        
        mesh.RecalculateNormals();
    }
}
