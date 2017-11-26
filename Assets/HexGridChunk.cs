using UnityEngine;
using UnityEngine.UI;

public class HexGridChunk : MonoBehaviour {

    HexCell[] cells;
    public HexMesh map;
    Canvas gridCanvas;
    HexMetrics _hexMetrics;
    public Text cellLabelPrefab;

    void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        cells = new HexCell[HexGrid.chunkSizeX * HexGrid.chunkSizeY];
    }

    public void SetHexMetrics(HexMetrics hexMetrics)
    {
        _hexMetrics = hexMetrics;
    }

    public void AddCell(int index, HexCell cell)
    {
        cells[index] = cell;
        cell.chunk = this;
        cell.transform.SetParent(transform, false);

        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.anchoredPosition = cell.transform.position;
        cell.uiRect = label.rectTransform;
        cell.uiRect.SetParent(gridCanvas.transform, false);
    }

    public void Refresh()
    {
        //hexMesh.Triangulate(cells);
        enabled = true;
    }

    void LateUpdate()
    {
        TriangulateAll();
        enabled = false;
    }

    /// <summary>
    /// Create all Hexes from individual triangles and add it to the mesh
    /// </summary>
    public void TriangulateAll()
    {
        map.Clear();

        for (int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i]);
        }
        map.Apply();
    }

    /// <summary>
    /// Create an individual triangle 
    /// </summary>
    void Triangulate(HexCell cell)
    {
        for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
        {
            Triangulate(d, cell);
        }
    }

    void Triangulate(HexDirection direction, HexCell cell)
    {
        Vector3 center = cell.transform.localPosition;
        map.AddTriangle(
            center,
            center + _hexMetrics.GetFirstCorner(direction),
            center + _hexMetrics.GetSecondCorner(direction)
            );
        map.AddTriangleColor(cell.Color);
    }


}
