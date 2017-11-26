using UnityEngine;

/// <summary>
/// Translates cartesian coordinates to cube coordinates
/// </summary>
[System.Serializable]
public struct HexCoordinates
{

    [SerializeField] private int _x, _y;


    public int X { get { return _x; } }

    public int Y { get { return _y; } }

    public int Z { get {return -X - Y; } }


    public HexCoordinates(int x, int y)
    {
        _x = x;
        _y = y;
    }

    public static HexCoordinates FromOffsetCoordinates(int x, int y)
    {
        return new HexCoordinates(x - y / 2, y);
    }

    public override string ToString()
    {
        return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
    }

    public static HexCoordinates FromPosition(Vector3 position, float outerRadius, float innerRadius)
    {
        float x = position.x / (innerRadius * 2f);
        float z = -x;

        float offset = position.y / (outerRadius * 3f);
        x -= offset;
        z -= offset;

        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(-x -z);
        int iZ = Mathf.RoundToInt(z);

        //Heuristic to compensate for rounding error
        if (iX + iY + iZ != 0)
        {
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(-x - z - iY);
            float dZ = Mathf.Abs(z - iZ);

            if (dX > dY && dX > dZ)
            {
                iX = -iY - iZ;
            }
            else if (dY > dZ)
            {
                iY = -iX - iZ;
            }
        }

        return new HexCoordinates(iX, iY);
    }

    /// <summary>
    /// Return number of hexes of distance
    /// </summary>
    public int DistanceTo (HexCoordinates other)
    {
        return (Mathf.Abs(X - other.X) + Mathf.Abs(Y - other.Y) + Mathf.Abs(Z - other.Z)) / 2;
    }




}