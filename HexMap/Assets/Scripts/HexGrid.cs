using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class creates the HexGrid
/// number of width and height are exposed
/// the user can select in the editor how many cells to generate
/// </summary>
public class HexGrid : MonoBehaviour {

    public int width = 6;
    public int height = 6;

    public HexCell cellPrefab;

    HexCell[] _cells;

    public Text cellLabelPrefab;

    Canvas _gridCanvas;

    HexMesh _hexMesh;

    private void Awake()
    {
        _gridCanvas = GetComponentInChildren<Canvas>();
        _hexMesh = GetComponentInChildren<HexMesh>();

        _cells = new HexCell[height * width];

        for (int z = 0, _i = 0; z < height; z++) 
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, _i++);
            }
        }
    }

    private void Start()
    {
        _hexMesh.Triangulate(_cells);
    }

    /// <summary>
    /// Create cells in the grid
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <param name="i"></param>
    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z/2) * HexMetrics.innerRadius *2f;
        position.y = 0f;
        position.z = z * HexMetrics.outerRadius * 1.5f;

        HexCell cell = _cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);

        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(_gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();

    }

    private void Update()
    {
        if (Input.GetMouseButton(0) )
        {
            HandleInput();
        }
    }

    /// <summary>
    /// Check what hex cell was clicked 
    /// </summary>
    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            TouchCell(hit.point);
        }
    }

    /// <summary>
    /// Transform worldspace coordinate to HexCoordinate
    /// Determines which cell was clicked
    /// </summary>
    /// <param name="position"></param>
    void TouchCell (Vector3 position)
    {
        position = transform.InverseTransformDirection(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        Debug.Log("touched at " + coordinates.ToString());
    }

}
