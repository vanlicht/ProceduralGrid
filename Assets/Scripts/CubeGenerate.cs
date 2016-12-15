using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CubeGenerate : MonoBehaviour
{
    #region Public Variables
    public int xSize, ySize, zSize;
    #endregion

    #region Private Variables
    private Vector3[] vertices;
    private Mesh mesh;
    private int cornerVertices;
    private int edgeVertices;
    private int faceVertices;
    #endregion

    #region Main Methods
    private void Awake()
    {
        StartCoroutine(Generate());
    }
    #endregion

    #region
    IEnumerator Generate()
    {
        this.GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "ProceduralCube";
        WaitForSeconds wait = new WaitForSeconds(0.05f);

        cornerVertices = 8;
        edgeVertices = 4 * (xSize + ySize + zSize - 3);
        faceVertices = 2 * ((xSize - 1) * (ySize - 1) + (ySize - 1) * (zSize - 1) + (xSize - 1) * (zSize - 1));
        vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];

        int v = 0;
        for(int y = 0; y <=ySize; y++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                vertices[v++] = new Vector3(x, y, 0);
                Debug.Log(v);
                yield return wait;
            }

            for (int z = 1; z <= zSize; z++)
            {
                vertices[v++] = new Vector3(xSize, y, z);
                Debug.Log(v);
                yield return wait;
            }

            for (int x = xSize - 1; x >= 0; x--)
            {
                vertices[v++] = new Vector3(x, y, zSize);
                Debug.Log(v);
                yield return wait;
            }

            for (int z = zSize - 1; z > 0; z--)
            {
                vertices[v++] = new Vector3(0, y, z);
                Debug.Log(v);
                yield return wait;
            }
        }

        for(int z = 1; z < zSize; z++)
        {
            for(int x = 1; x < xSize; x++)
            {
                vertices[v++] = new Vector3(x, ySize, z);
                Debug.Log(v);
                yield return wait;
            }
        }

        for (int z = 1; z < zSize; z++)
        {
            for (int x = 1; x < xSize; x++)
            {
                vertices[v++] = new Vector3(x, 0, z);
                Debug.Log(v);
                yield return wait;
            }
        }

    }

    private void OnDrawGizmos()
    {
        if(vertices == null)
        {
            return;
        }
        Gizmos.color = Color.black;
        for(int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.05f);
        }
    }
    #endregion
}
