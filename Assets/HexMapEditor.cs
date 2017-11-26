using UnityEngine;

public class HexMapEditor : MonoBehaviour {

    public Color[] colors;

    public Color ActiveColor { get; private set; }

    public Material mapMaterial;


    private void Awake()
    {
        SelectColor(0);
        ShowGrid(true);
    }


    public void SelectColor(int index)
    {
        ActiveColor = colors[index];
    }

    public void ShowGrid(bool visible)
    {
        if (visible)
        {
            mapMaterial.EnableKeyword("GRID_ON");
        }
        else
        {
            mapMaterial.DisableKeyword("GRID_ON");
        }
    }


}
