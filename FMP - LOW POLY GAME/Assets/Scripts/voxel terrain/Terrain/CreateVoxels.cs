using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateVoxels : MonoBehaviour
{    
    enum Cubeside { BOTTOM, TOP, LEFT, RIGHT, FRONT, BACK};
    public enum BlockType { GRASS, DIRT, STONE, PURPLE_WOOL};

    BlockType btype;
    GameObject parent;
    Vector3 position;
    Material cubeMaterial;
    

    Vector2[,] blockUVs = {
        /*Grass top*/     { new Vector2(0.125f, 0.9375f), new Vector2(0.1875f, 0.9375f),
                            new Vector2(0.125f, 1f), new Vector2(0.1875f, 1f)}

        /**/
        /**/
        /*Purple wool*/

    };

    void Start()
    {
        CreateCube();
    }

    void CreateQuad(Cubeside side)
    {
        Mesh mesh = new Mesh();
        mesh.name = "ScriptedMesh";

        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] uvs = new Vector2[4];
        int[] triangles = new int[6];

        //all possible UVs
        Vector2 uv00;
        Vector2 uv10;
        Vector2 uv01;
        Vector2 uv11;

        if(btype == BlockType.GRASS) //&& side == Cubeside.TOP
        {
            uv00 = blockUVs[0, 0];
            uv10 = blockUVs[0, 1];
            uv01 = blockUVs[0, 2];
            uv11 = blockUVs[0, 3];
        }else
        {
            uv00 = blockUVs[0, 0];
            uv10 = blockUVs[0, 1];
            uv01 = blockUVs[0, 2];
            uv11 = blockUVs[0, 3];
            //uv00 = blockUVs[(int)(btype + 1), 0];
            //uv10 = blockUVs[(int)(btype + 1), 1];
            //uv01 = blockUVs[(int)(btype + 1), 2];
            //uv11 = blockUVs[(int)(btype + 1), 3];
        }

        //all possible vertices
        Vector3 p0 = new Vector3(-0.5f, -0.5f, 0.5f);
        Vector3 p1 = new Vector3(0.5f, -0.5f, 0.5f);
        Vector3 p2 = new Vector3(0.5f, -0.5f, -0.5f);
        Vector3 p3 = new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 p4 = new Vector3(-0.5f, 0.5f, 0.5f);
        Vector3 p5 = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 p6 = new Vector3(0.5f, 0.5f, -0.5f);
        Vector3 p7 = new Vector3(-0.5f, 0.5f, -0.5f);


        vertices = new Vector3[] { p4, p5, p1, p0 };
        normals = new Vector3[] {Vector3.forward,
                                 Vector3.forward,
                                 Vector3.forward,
                                 Vector3.forward};

        switch(side)
        {
            case Cubeside.BOTTOM:
                vertices = new Vector3[] { p0, p1, p2, p3 };
                normals = new Vector3[] { Vector3.down, Vector3.down, Vector3.down, Vector3.down };
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.TOP:
                vertices = new Vector3[] { p7, p6, p5, p4 };
                normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.LEFT:
                vertices = new Vector3[] { p7, p4, p0, p3 };
                normals = new Vector3[] { Vector3.left, Vector3.left, Vector3.left, Vector3.left };
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.RIGHT:
                vertices = new Vector3[] { p5, p6, p2, p1 };
                normals = new Vector3[] { Vector3.right, Vector3.right, Vector3.right, Vector3.right };
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.FRONT:
                vertices = new Vector3[] { p4, p5, p1, p0 };
                normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.BACK:
                vertices = new Vector3[] { p6, p7, p3, p2 };
                normals = new Vector3[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back };
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();

        GameObject quad = new GameObject("quad");
        quad.transform.parent = this.gameObject.transform;

        MeshFilter meshFilter = quad.AddComponent<MeshFilter>();
        MeshRenderer renderer = quad.AddComponent<MeshRenderer>();

        meshFilter.mesh = mesh;
        renderer.material = cubeMaterial;
    }    

    void CombineQuads()
    {
        //Combine all children meshes
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;                        
        }

        //Create a new mesh on parent object
        MeshFilter mf = this.gameObject.AddComponent<MeshFilter>();
        mf.mesh = new Mesh();

        //Add combined meshes on children as the parent' mesh
        mf.mesh.CombineMeshes(combine);

        ////Create a renderer for the parent
        MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
        renderer.material = cubeMaterial;

        //Delete all uncombined children
        foreach (Transform quad in this.transform)
        {
            Destroy(quad.gameObject);
        }
    }

    void CreateCube()
    {
        CreateQuad(Cubeside.BOTTOM);
        CreateQuad(Cubeside.TOP);
        CreateQuad(Cubeside.LEFT);
        CreateQuad(Cubeside.RIGHT);
        CreateQuad(Cubeside.FRONT);
        CreateQuad(Cubeside.BACK);

        CombineQuads();
    }
}
