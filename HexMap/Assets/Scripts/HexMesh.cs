using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the main 
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour {

    Mesh hexMesh;
    List<Vector3> vertices;
    List<int> triangles;

    MeshCollider meshCollider;

    private void Awake()
    {
        hexMesh = new Mesh()
        {
            name = "Hex Mesh"

        };
        GetComponent<MeshFilter>().mesh = hexMesh;
        meshCollider = gameObject.AddComponent<MeshCollider>();
        vertices = new List<Vector3>();
        triangles = new List<int>();
    }

    /// <summary>
    /// Creates triangles for all hexagons
    /// </summary>
    /// <param name="cells"> array of total hexagons </param>
    public void Triangulate (HexCell[] cells)
    {
        hexMesh.Clear();
        vertices.Clear();
        triangles.Clear();
        for (int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i]);
        }

        hexMesh.vertices = vertices.ToArray();
        hexMesh.triangles = triangles.ToArray();
        hexMesh.RecalculateNormals();
        meshCollider.sharedMesh = hexMesh;

    }

    /// <summary>
    /// Creates 6 inner triangles of hexagon
    /// </summary>
    /// <param name="cell"></param>
    void Triangulate (HexCell cell)
    {
        Vector3 center = cell.transform.localPosition;
        for (int i = 0; i < 6; i++)
        {
            AddTriangle(
            center,
            center + HexMetrics.corners[i],
            center + HexMetrics.corners[i+1]
            );
        }
        
    }

   /// <summary>
   /// Creates triangle using 3 vertices and adds it to the triangle list
   /// </summary>
    void AddTriangle (Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex+1);
        triangles.Add(vertexIndex+2);
    }


}
