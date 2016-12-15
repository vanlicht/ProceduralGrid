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
    private Vector2[] uv;
    Vector4[] tangents;
    Vector4 tangent;
    #endregion

    #region Main Methods
    private void Awake()
    {
        Generate();
    }
    #endregion

    #region Utility Methods
    void Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "ProceduralGrid";

        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        
        //for uv
        uv = new Vector2[vertices.Length];
        
        //for normal
        tangents = new Vector4[vertices.Length];

        /*The surface normal represents upward in this space, but which way is right? That's defined by the tangent. 
        Ideally, the angle between these two vectors is 90°. The cross product of them yields the third direction needed to define 3D space. 
        In reality the angle is often not 90° but the results are still good enough.
        So a tangent is a 3D vector, but Unity actually uses a 4D vector. Its fourth component is always either −1 or 1, 
        which is used to control the direction of the third tangent space dimension – either forward or backward. 
        This facilitates mirroring of normal maps, which is often used in 3D models of things with bilateral symmetry, like people. 
        The way Unity's shaders perform this calculation requires us to use −1.
        */
        tangent = new Vector4(1, 0, 0, -1);
        

        for (int i = 0, y = 0; y < ySize + 1; y++)
        {
            for(int x = 0; x < xSize + 1; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
                uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
                tangents[i] = tangent;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.tangents = tangents;

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
                
            }
        }
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

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
