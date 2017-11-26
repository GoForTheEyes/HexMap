using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// This is the hero hex object
/// this 
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class HeroHex : MonoBehaviour {

    public HexCoordinates coordinates;
    public Sprite heroSprite;

    HexMetrics hexMetrics;
    MeshRenderer _meshRenderer;

    Material _material;
    Mesh _heroMesh;

    [NonSerialized] List<Vector3> _vertices;
    [NonSerialized] List<int> _triangles;
    [NonSerialized] List<Vector2> _uvs;

    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = _heroMesh = new Mesh();
        _heroMesh.name = "Hero Mesh";
        _meshRenderer = GetComponent<MeshRenderer>();
        _material = _meshRenderer.material;
        hexMetrics = GameObject.FindObjectOfType<HexGrid>().GetHexMetrics();
        Clear();

        Rect spriteRect = heroSprite.textureRect;
        spriteRect.x /= heroSprite.texture.width;
        spriteRect.width /= heroSprite.texture.width;
        spriteRect.y /= heroSprite.texture.height;
        spriteRect.height /= heroSprite.texture.height;
        _material.mainTexture = heroSprite.texture;

        Triangulate();
        Apply();
    }


    Vector3 GetFirstUVCorner(HexDirection direction)
    {
        return CornersUV()[(int)direction];
    }

    Vector3 GetSecondUVCorner(HexDirection direction)
    {
        return CornersUV()[(int)direction + 1];
    }

    Vector2[] CornersUV()
    {
        return new Vector2[7] {
            new Vector2(0.5f, 1f),
            new Vector2(1f, 0.75f),
            new Vector2(1f, 0.25f),
            new Vector2(0.5f, 0f),
            new Vector2(0f, 0.25f),
            new Vector2(0f, 0.75f),
            new Vector2(0.5f, 1f)
        };
        //return new Vector2[7] {
        //    new Vector2(0.5f, 1f),
        //    new Vector2(_innerRadius2D + 0.5f, 0.75f),
        //    new Vector2(_innerRadius2D + 0.5f, 0.25f),
        //    new Vector2(0.5f, 0f),
        //    new Vector2(-_innerRadius2D + 0.5f, 0.25f),
        //    new Vector2(-_innerRadius2D + 0.5f, 0.75f),
        //    new Vector2(0.5f, 1f)
        //};
    }


    void Triangulate()
    {
        for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
        {
            Triangulate(d);
        }
    }

    void Triangulate(HexDirection direction)
    {
        Vector3 center = transform.localPosition;
        AddTriangle(
            center,
            center + hexMetrics.GetFirstInnerCorner(direction),
            center + hexMetrics.GetSecondInnerCorner(direction)
            );
        var centerUV = new Vector3(0.5f, 0.5f);
        AddTriangleUV(
            centerUV, GetFirstUVCorner(direction), GetSecondUVCorner(direction)
            );

    }

    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = _vertices.Count;
        _vertices.Add(v1);
        _vertices.Add(v2);
        _vertices.Add(v3);
        _triangles.Add(vertexIndex);
        _triangles.Add(vertexIndex + 1);
        _triangles.Add(vertexIndex + 2);
    }

    public void AddTriangleUV(Vector2 uv1, Vector2 uv2, Vector3 uv3)
    {
        _uvs.Add(uv1);
        _uvs.Add(uv2);
        _uvs.Add(uv3);
    }

    void Apply()
    {
        _heroMesh.SetVertices(_vertices);
        ListPool<Vector3>.Add(_vertices);
        _heroMesh.SetTriangles(_triangles, 0);
        ListPool<int>.Add(_triangles);
        _heroMesh.SetUVs(0, _uvs);
        ListPool<Vector2>.Add(_uvs);
        _heroMesh.RecalculateNormals();
    }

    public void Clear()
    {
        _heroMesh.Clear();
        _vertices = ListPool<Vector3>.Get();
        _triangles = ListPool<int>.Get();
        _uvs = ListPool<Vector2>.Get();
    }
}
