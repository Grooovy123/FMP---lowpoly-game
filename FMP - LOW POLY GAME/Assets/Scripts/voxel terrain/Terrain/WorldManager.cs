using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public Material cubeMaterial;

    [SerializeField]
    private int chunkSize;
    [SerializeField]
    private int height;

    public BlockGen[,,] chunkData;

    void Start()
    {
        StartCoroutine(GenerateChunk(chunkSize, height));
    }

    IEnumerator GenerateChunk(int chunkSize, int height)
    {
        chunkData = new BlockGen[chunkSize, height, chunkSize];

        //Create blocks
        for (int z = 0; z < chunkSize; z++){
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < chunkSize; x++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    chunkData[x, y, z] = new BlockGen(BlockGen.BlockType.DIRT, pos,
                                         this.gameObject, cubeMaterial);
                }
            }
        }

        //Draw blocks
        for (int z = 0; z < chunkSize; z++){
            for (int y = 0; y < height; y++){
                for (int x = 0; x < chunkSize; x++)
                {
                    chunkData[x, y, z].Draw();
                    yield return null;
                }                
            }
        }
        CombineQuads();
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
}
