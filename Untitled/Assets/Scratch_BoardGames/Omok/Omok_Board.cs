using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Omok_Board : MonoBehaviour
{
    public int tilesRow;
    public int tilesColumn;
    public Texture2D tileTexture;

    [HideInInspector]
    public Omok_BoardTile[,] boardData;

    private int tileResolution;

    // Use this for initialization
    void Start() {
        SetBoard();
    }

    // Update is called once per frame
    void Update() {

    }

    public void SetBoard()
    {
        int verticesRow = tilesRow + 1;
        int verticesColumn = tilesColumn + 1;
        int verticesNumber = verticesRow * verticesColumn;

        Vector3[] vertices = new Vector3[verticesNumber];
        Vector3[] normals = new Vector3[verticesNumber];
        Vector2[] uv = new Vector2[verticesNumber];

        int index;
        for (int z = 0; z < verticesColumn; z++)
        {
            for (int x = 0; x < verticesRow; x++)
            {
                index = z * verticesRow + x;

                vertices[index] = new Vector3((float)x, 0.0f, (float)z);
                normals[index] = Vector3.up;
                uv[index] = new Vector2((float)x / (float)tilesRow, (float)z / (float)tilesColumn);
            }
        }

        int[] triangles = new int[tilesRow * tilesColumn * 2 * 3];

        int startingIndex = 0;
        index = 0;
        for (int z = 0; z < tilesColumn; z++)
        {
            for (int x = 0; x < tilesRow; x++)
            {
                triangles[index] = startingIndex;
                index++;
                triangles[index] = startingIndex + verticesRow;
                index++;
                triangles[index] = startingIndex + verticesRow + 1;
                index++;
                triangles[index] = startingIndex;
                index++;
                triangles[index] = startingIndex + verticesRow + 1;
                index++;
                triangles[index] = startingIndex + 1;
                index++;

                startingIndex++;
            }

            startingIndex++;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;
        mesh.triangles = triangles;

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;

        SetBoardData();

        SetTexture();
    }

    private void SetTexture()
    {
        tileResolution = tileTexture.height;
        int textureSize = tileResolution * tilesColumn;

        Texture2D texture = new Texture2D(textureSize, textureSize);

        for (int z = 0; z < tilesColumn; z++)
        {
            for (int x = 0; x < tilesRow; x++)
            {
                texture.SetPixels(x * tileResolution, z * tileResolution, tileResolution, tileResolution, tileTexture.GetPixels());
            }
        }

        texture.filterMode = FilterMode.Bilinear;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial.mainTexture = texture;
    }

    private void SetBoardData()
    {
        boardData = new Omok_BoardTile[tilesRow, tilesColumn];

        for (int z = 0; z < tilesColumn; z++)
        {
            for (int x = 0; x < tilesRow; x++)
            {
                boardData[x, z] = new Omok_BoardTile();
            }
        }
    }
}
