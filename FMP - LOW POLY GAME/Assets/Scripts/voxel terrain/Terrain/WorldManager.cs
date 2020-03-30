using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public Material TextureTile;
    public static int columnHeight = 4;
    public static int chunkSize = 16;
    public static Dictionary<string, ChunkLoader> chunks;

    public void Start(){        
        chunks = new Dictionary<string, ChunkLoader>();
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;        
        StartCoroutine(BuildChunk());
    }

    public static string BuildChunkName(Vector3 v){
        return ($"{(int)v.x}_{(int)v.y}_{(int)v.z}");
    }

    IEnumerator BuildChunk(){
        for (int i = 0; i < columnHeight; i++)
        {
            Vector3 chunkPositon = new Vector3(this.transform.position.x,
                                 i*columnHeight, this.transform.position.z);

            ChunkLoader c = new ChunkLoader(chunkPositon, TextureTile);
            c.chunk.transform.parent = this.transform;
            chunks.Add(c.chunk.name, c);
        }

        foreach(KeyValuePair<string, ChunkLoader> c in chunks)
        {
           c.Value.DrawChunk();           
           yield return null; 
        }
    }
}
