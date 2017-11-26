using UnityEngine;
using UnityEngine.UI;

public class HexCell : MonoBehaviour
{

    Color color;
    int distance;

    public int Distance
    {
        get { return distance; }
        set { distance = value; }
    }
    
    public HexCoordinates coordinates;

    public Color Color
    {
        get  { return color; }
        set { 
            if (color == value) return;
            color = value;
            Refresh();
        }
    }

    public HexGridChunk chunk;

    public RectTransform uiRect;

    public HexCell PathFrom { get; set; }

    [SerializeField] HexCell[] neighbors = new HexCell[6];

    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }

    /// <summary>
    /// Set neighbor in one direction for a particular hex
    /// Set the opposite hex in that direction
    /// (ie) Neighborhood is symmetrical  E <-> W
    /// </summary>
    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    void Refresh()
    {
        if (chunk)
        {
            chunk.Refresh();
            for (int i = 0; i < neighbors.Length; i++)
            {
                HexCell neighbor = neighbors[i];
                if (neighbor != null && neighbor.chunk != chunk)
                {
                    neighbor.chunk.Refresh();
                }
            }
        }
    }

    public void DisableHighlight()
    {
        Image highlight = uiRect.GetChild(0).GetComponent<Image>();
        highlight.enabled = false;
    }

    public void EnableHighlight(Color color)
    {
        Image highlight = uiRect.GetChild(0).GetComponent<Image>();
        highlight.color = color;
        highlight.enabled = true;
    }

}
