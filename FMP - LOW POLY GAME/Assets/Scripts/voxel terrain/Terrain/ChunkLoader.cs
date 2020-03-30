using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLoader
{
    public Material cubeMaterial;    
    public BlockGen[,,] chunkData;    
    public GameObject chunk;

    public ChunkLoader (Vector3 position, Material c){
        chunk = new GameObject (WorldManager.BuildChunkName(position));
        chunk.transform.position = position;
        cubeMaterial = c;        
        GenerateChunk();
    }

    void GenerateChunk()
    {
        chunkData = new BlockGen[WorldManager.chunkSize, WorldManager.columnHeight, WorldManager.chunkSize];

        //Create blocks
        for (int z = 0; z < WorldManager.chunkSize; z++){
            for (int y = 0; y < WorldManager.columnHeight; y++) {
                for (int x = 0; x < WorldManager.chunkSize; x++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    // Create biome function here
                    if (Random.Range(0,100) < 50){
                        chunkData[x, y, z] = new BlockGen(BlockGen.BlockType.GRASS, pos,
                                         chunk.gameObject, this);
                    }
                    else{
                        chunkData[x, y, z] = new BlockGen(BlockGen.BlockType.AIR, pos,
                                         chunk.gameObject, this);
                    }                    
                }
            }
        }       
    }
    
    //Draw blocks
    public void DrawChunk()
    {
        for (int z = 0; z < WorldManager.chunkSize; z++){
            for (int y = 0; y < WorldManager.columnHeight; y++){
                for (int x = 0; x < WorldManager.chunkSize; x++)
                {
                    chunkData[x, y, z].Draw();                    
                }                
            }              
        }
        CombineQuads();        
    }    
    
    void CombineQuads()
    {
        //Combine all children meshes
        MeshFilter[] meshFilters = chunk.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        //Create a new mesh on parent object
        MeshFilter mf = chunk.gameObject.AddComponent<MeshFilter>();
        mf.mesh = new Mesh();

        //Add combined meshes on children as the parent' mesh
        mf.mesh.CombineMeshes(combine);

        ////Create a renderer for the parent
        MeshRenderer renderer = chunk.gameObject.AddComponent<MeshRenderer>();
        renderer.material = cubeMaterial;

        //Delete all uncombined children
        foreach (Transform quad in chunk.transform)
        {
            GameObject.Destroy(quad.gameObject);
        }
    }
}