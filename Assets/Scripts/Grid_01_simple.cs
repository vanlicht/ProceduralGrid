using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid_01_simple : MonoBehaviour
{
    #region Public Variables
    public int xSize, ySize;
    #endregion

    #region Private Variables
    private Vector3[] vertices;
    private Mesh mesh;
    #endregion

    #region Main Methods
    private void Awake()
    {
        StartCoroutine(Generate());
    }
    #endregion

    #region Utility Methods
    private IEnumerator Generate()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "ProceduralGrid";

        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        for(int i = 0, y = 0; y < ySize + 1; y++)
        {
            for(int x = 0; x < xSize + 1; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
                yield return wait;
            }
        }

        mesh.vertices = vertices;

        ////Pattern for the first square, composed of 2 triangles
        //int[] triangles = new int[6];
        //triangles[0] = 0;
        //triangles[3] = triangles[2] = 1;
        //triangles[4] = triangles[1] = xSize + 1;
        //triangles[5] = xSize + 2;

        int[] triangles = new int[xSize * ySize * 6];
        for(int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for(int x = 0; x < xSize; x++, ti +=6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
                yield return wait;
                mesh.triangles = triangles;
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
