using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGen
{    
    enum Cubeside { BOTTOM, TOP, LEFT, RIGHT, FRONT, BACK};
    public enum BlockType { GRASS, DIRT, AIR};

    BlockType btype;
    public bool isSolid;
    ChunkLoader owner;
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

    public BlockGen(BlockType b, Vector3 pos, GameObject p, ChunkLoader o)
    {
        btype = b;  
        owner = o;      
        parent = p;
        position = pos;        
        if (btype == BlockType.AIR)
        {
            isSolid = false;
        }else
        {
            isSolid = true;
        }        
    }

    void CreateQuad(Cubeside side)
    {
        Mesh mesh = new Mesh();
        mesh.name = ($"ScriptedMesh {side.ToString()}");

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
        quad.transform.position = position;
        quad.transform.parent = parent.transform;

        MeshFilter meshFilter = quad.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

    }     

    int ConvertBlockIndexToLocal(int i)
    {
        if(i == -1)
            i = WorldManager.chunkSize-1;
        else if (i == WorldManager.chunkSize)
            i = 0;        
        return i;
    }   

    public bool HasSolidNeighbour(int x, int y, int z)
    {
        BlockGen[,,] chunks;

		if(x < 0 || x >= WorldManager.chunkSize || 
		   y < 0 || y >= WorldManager.chunkSize ||
		   z < 0 || z >= WorldManager.chunkSize)
		{  //block in a neighbouring chunk
			
			Vector3 neighbourChunkPos = this.parent.transform.position + 
										new Vector3((x - (int)position.x)*WorldManager.chunkSize, 
											(y - (int)position.y)*WorldManager.chunkSize, 
											(z - (int)position.z)*WorldManager.chunkSize);
			string nName = WorldManager.BuildChunkName(neighbourChunkPos);

			x = ConvertBlockIndexToLocal(x);
			y = ConvertBlockIndexToLocal(y);
			z = ConvertBlockIndexToLocal(z);
			
			ChunkLoader nChunk;
			if(WorldManager.chunks.TryGetValue(nName, out nChunk))
			{
				chunks = nChunk.chunkData;
			}
			else
				return false;
		}  //block in this chunk
		else
			chunks = owner.chunkData;
		
		try
		{
            //Debug.Log($"{chunks[x,y,z].isSolid}");
			return chunks[x,y,z].isSolid;
		}
		catch(System.IndexOutOfRangeException){}

		return false;
    }

    public void Draw()
    {
        if (btype == BlockType.AIR) return;

        if (!HasSolidNeighbour((int)position.x, (int)position.y, (int)position.z + 1))
            CreateQuad(Cubeside.FRONT);
        if (!HasSolidNeighbour((int)position.x, (int)position.y, (int)position.z - 1))
            CreateQuad(Cubeside.BACK);
        if (!HasSolidNeighbour((int)position.x, (int)position.y + 1, (int)position.z))
            CreateQuad(Cubeside.TOP);
        if (!HasSolidNeighbour((int)position.x, (int)position.y - 1, (int)position.z))
            CreateQuad(Cubeside.BOTTOM);
        if (!HasSolidNeighbour((int)position.x - 1, (int)position.y, (int)position.z))
            CreateQuad(Cubeside.LEFT);
        if (!HasSolidNeighbour((int)position.x + 1, (int)position.y, (int)position.z))
            CreateQuad(Cubeside.RIGHT);
    }
}
