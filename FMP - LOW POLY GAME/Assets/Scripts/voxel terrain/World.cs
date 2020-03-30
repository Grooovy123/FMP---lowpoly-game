using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class World : MonoBehaviour {

	public Material textureAtlas;
	public static int columnHeight = 2;
	public static int chunkSize = 8;
	public static int worldSize = 12;
	public static Dictionary<string, Chunk> chunks;

	public static string BuildChunkName(Vector3 v)
	{
		return (int)v.x + "_" + 
			         (int)v.y + "_" + 
			         (int)v.z;
	}

	IEnumerator BuildChunkColumn()
	{
		for(int i = 0; i < columnHeight; i++)
		{
			Vector3 chunkPosition = new Vector3(this.transform.position.x, 
												i*chunkSize, 
												this.transform.position.z);
			Chunk c = new Chunk(chunkPosition, textureAtlas);
			c.chunk.transform.parent = this.transform;
			chunks.Add(c.chunk.name, c);
		}

		foreach(KeyValuePair<string, Chunk> c in chunks)
		{
			c.Value.DrawChunk();
			yield return null;
		}
		
	}

	//IEnumerator 
	void BuildWorld()
	{
		for(int z = 0; z < worldSize; z++)
			for(int x = 0; x < worldSize; x++)
				for(int y = 0; y < columnHeight; y++)
				{
					Vector3 chunkPosition = new Vector3(x*chunkSize, y*chunkSize, z*chunkSize);
					Chunk c = new Chunk(chunkPosition, textureAtlas);
					c.chunk.transform.parent = this.transform;
					chunks.Add(c.chunk.name, c);
					
				}

		foreach(KeyValuePair<string, Chunk> c in chunks)
		{
			c.Value.DrawChunk();			
		}
		
	}

	// Use this for initialization
	void Start () {
		float startTime = Time.realtimeSinceStartup;

		chunks = new Dictionary<string, Chunk>();
		this.transform.position = Vector3.zero;
		this.transform.rotation = Quaternion.identity;						
		BuildWorld();

		Debug.Log($"Time taken to generate world (chunkSize {chunkSize} - worldSize {worldSize} - columnHeight {columnHeight}): TIME {(Time.realtimeSinceStartup - startTime).ToString()} seconds");
	}	
}
