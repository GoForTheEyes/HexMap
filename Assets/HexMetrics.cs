using UnityEngine;

public class HexMetrics
{
    float _outerRadius;
    float _innerRadius;

    public float OuterRadius { get { return _outerRadius; } private set { _outerRadius = value; } }
    public float InnerRadius { get { return _innerRadius; } private set { _innerRadius = value; } }

    public HexMetrics(float outerRadius, float innerRadius)
    {
        OuterRadius = outerRadius;
        InnerRadius = innerRadius;
    }


    public Vector3[] Corners()
    {
        return new Vector3[7] {
            new Vector3(0f, OuterRadius, 0f),
            new Vector3(InnerRadius, 0.5f * OuterRadius, 0f),
            new Vector3(InnerRadius, -0.5f * OuterRadius, 0f),
            new Vector3(0f, -OuterRadius, 0f),
            new Vector3(-InnerRadius, -0.5f * OuterRadius, 0f),
            new Vector3(-InnerRadius, 0.5f * OuterRadius, 0f),
            new Vector3(0f, OuterRadius, 0f)
        }; 
    }

    public Vector3[] InnerCorners()
    {
        return new Vector3[7] {
            new Vector3(0f, OuterRadius, 0f),
            new Vector3(InnerRadius, 0.5f * OuterRadius, 0f),
            new Vector3(InnerRadius, -0.5f * OuterRadius, 0f),
            new Vector3(0f, -OuterRadius, 0f),
            new Vector3(-InnerRadius, -0.5f * OuterRadius, 0f),
            new Vector3(-InnerRadius, 0.5f * OuterRadius, 0f),
            new Vector3(0f, OuterRadius, 0f)
        };
    }

    public Vector3 GetFirstCorner(HexDirection direction)
    {
        return Corners()[(int)direction]; 
    }

    public Vector3 GetSecondCorner(HexDirection direction)
    {
        return Corners()[(int)direction+1]; 
    }

    // We use Inner corner for smaller hexes that we want to draw
    // This can potentially be used to create a smaller hex with the Hero image
    // to represent Hero position

    public Vector3 GetFirstInnerCorner(HexDirection direction)
    {
        return InnerCorners()[(int)direction] * HexGrid.innerHex;
    }

    public Vector3 GetSecondInnerCorner(HexDirection direction)
    {
        return InnerCorners()[(int)direction + 1] * HexGrid.innerHex;
    }


}
