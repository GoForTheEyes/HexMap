using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HexGrid : MonoBehaviour
{
    public const float HexOuterRadiusInMeters = 10f;
    public const float HexInnerRadiusInMeters = HexOuterRadiusInMeters * 0.866025404f;
    public const float innerHex = 0.75f;
    public const float outerHex = 1 - innerHex;
    public const int chunkSizeX = 14;
    public const int chunkSizeY = 13;
    public const int chunkCountX = 1, chunkCountY = 1;

    public Color defaultColor = Color.white;
    public HexGridChunk chunkPrefab;
    public HexCell cellPrefab;


    HexGridChunk[] chunks;
    HexMetrics hexMetrics;
    HexCell[] cells;


    int cellCountX, cellCountY;

    //Initialize first
    void Awake()
    {
        hexMetrics = new HexMetrics(HexGrid.HexOuterRadiusInMeters, HexGrid.HexInnerRadiusInMeters);


        CreateChunks();

        cellCountX = chunkCountX * chunkSizeX;
        cellCountY = chunkCountY * chunkSizeY;

        InitializeHexCellsInGrid();
    }


    public HexMetrics GetHexMetrics()
    {
        return hexMetrics;
    }

    public HexCell GetHexCell(int index)
    {
        return cells[index];
    }

    void CreateChunks()
    {
        chunks = new HexGridChunk[chunkCountX * chunkCountY];

        for (int y = 0, i = 0; y < chunkCountY; y++)
        {
            for (int x = 0; x < chunkCountX; x++)
            {
                HexGridChunk chunk = chunks[i++] = Instantiate(chunkPrefab);
                chunk.transform.SetParent(transform);
                chunk.SetHexMetrics(hexMetrics);
            }
        }
    }

    void InitializeHexCellsInGrid()
    {
        cells = new HexCell[cellCountY * cellCountX];

        for (int y = 0, i = 0; y < cellCountY; y++)
        {
            for (int x = 0; x < cellCountX; x++)
            {
                CreateCell(x, y, i++);
            }
        }

    }

    void CreateCell(int x, int y, int i)
    {
        Vector3 position;
        //Offsetting X so it lays down Hex on a rectangle instead of a rhombus
        position.x = (x + y *0.5f - y/2) * (HexInnerRadiusInMeters * 2f); 
        position.y = y * (HexOuterRadiusInMeters * 1.5f);
        position.z = 0f;

        HexCell cell = cells[i] = Instantiate(cellPrefab);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, y);
        cell.Color = defaultColor;

        SetNeighbors(cell, x, y, i);

        AddCellToChunk(x, y, cell);



    }

    void AddCellToChunk(int x, int y, HexCell cell)
    {
        int chunkX = x / chunkSizeX;
        int chunkY = y / chunkSizeY;
        HexGridChunk chunk = chunks[chunkX + chunkY * chunkCountX];
        int localX = x - chunkX * chunkSizeX;
        int localY = y - chunkY * chunkSizeY;
        chunk.AddCell(localX + localY * chunkSizeX, cell);
    }

    void SetNeighbors(HexCell cell, int x, int y, int i)
    {
        if (x > 0)
        {
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        }
        if (y > 0)
        {
            if ((y % 2) == 0)
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - chunkSizeX]);
                if (x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - chunkSizeX - 1]);
                }
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - chunkSizeX]);
                if (x < chunkSizeX - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - chunkSizeX + 1]);
                }
            }
        }
    }


    public void FindPath(HexCell fromCell, HexCell toCell)
    {
        StopAllCoroutines();
        StartCoroutine(Search(fromCell, toCell));
    }


    IEnumerator Search(HexCell fromCell, HexCell toCell)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Distance = int.MaxValue;
            cells[i].DisableHighlight();
        }
        fromCell.EnableHighlight(Color.blue);
        toCell.EnableHighlight(Color.red);

        WaitForSeconds delay = new WaitForSeconds(1 / 60f);
        List<HexCell> frontier = new List<HexCell>();
        fromCell.Distance = 0;
        frontier.Add(fromCell);
        while (frontier.Count > 0)
        {
            yield return delay;
            HexCell current = frontier[0];
            frontier.RemoveAt(0);

            if (current == toCell) 
            {
                current = current.PathFrom;
                while (current != fromCell)
                {
                    current.EnableHighlight(Color.white);
                    current = current.PathFrom;
                }
                break;
            }

            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                HexCell neighbor = current.GetNeighbor(d);
                int distance = current.Distance;
                //Here we should also have to search for obstacles in the if statement
                //example: neighbor.tag != "obstacle"
                if (neighbor == null)
                {
                    continue;
                }
                if (neighbor.Distance == int.MaxValue)
                {
                    neighbor.Distance = distance;
                    neighbor.PathFrom = current;
                    frontier.Add(neighbor);
                }
                else if (distance < neighbor.Distance)
                {
                    neighbor.Distance = distance;
                    neighbor.PathFrom = current;
                }
                frontier.Sort((x, y) => x.Distance.CompareTo(y.Distance));
            }

        }
    }


    public HexCell ChangeHex(Vector3 position)
    {
        HexCoordinates coordinates = 
            HexCoordinates.FromPosition(position, HexOuterRadiusInMeters, HexInnerRadiusInMeters);
        int index = coordinates.X + coordinates.Y * chunkSizeX + coordinates.Y / 2;
        return cells[index];
    }


}
