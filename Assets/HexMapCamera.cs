using UnityEngine;

public class HexMapCamera : MonoBehaviour
{
    Transform swivel;

    //public BoundsHeight;

    public float cameraMinZoom, cameraMaxZoom;
    public float cameraZoomSpeed;
    public float cameraMoveSpeedMinZoom, cameraMoveSpeedMaxZoom;
    public HexGrid grid;



    void Awake()
    {
        Camera.main.orthographicSize = cameraMinZoom;
        swivel = transform.GetChild(0);
    }

    void Update()
    {
        float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
        if (zoomDelta != 0f)
        {
            AdjustZoom(zoomDelta);
        }

        float xDelta = Input.GetAxis("Horizontal");
        float yDelta = Input.GetAxis("Vertical");
        if (xDelta!= 0f || yDelta != 0f)
        {
            AdjustPosition(xDelta, yDelta);
        }

    }

    void AdjustZoom(float delta)
    {
        float zoom = Camera.main.orthographicSize + delta * cameraZoomSpeed;
        float clampedZoom = Mathf.Clamp(zoom, cameraMinZoom, cameraMaxZoom);

        Camera.main.orthographicSize = clampedZoom;
    }

    /// <summary>
    /// Adjust x,y position of camera
    /// </summary>
    /// <param name="xDelta"></param>
    /// <param name="yDelta"></param>
    void AdjustPosition(float xDelta, float yDelta)
    {
        Vector3 direction = new Vector3(xDelta, yDelta, 0f).normalized;
        float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(yDelta));
        float distance =
            Mathf.Lerp(cameraMoveSpeedMinZoom, cameraMoveSpeedMaxZoom, Camera.main.orthographicSize/cameraMaxZoom) *
            damping * Time.deltaTime;

        Vector3 position = transform.localPosition;
        position += direction * distance;
        transform.localPosition = ClampPosition(position);

    }

    Vector3 ClampPosition(Vector3 position)
    {
        var cameraHeight = Camera.main.orthographicSize;
        var cameraWidth = cameraHeight * Screen.width / Screen.height;



        float xMax = (HexGrid.chunkCountX * HexGrid.chunkSizeX -0.5f) * (HexGrid.HexInnerRadiusInMeters * 2f);
        position.x = Mathf.Clamp(position.x, 0f + cameraWidth/2f, xMax-cameraWidth/2f);

        float yMax = (HexGrid.chunkCountY * HexGrid.chunkSizeY - 1f)* (HexGrid.HexOuterRadiusInMeters * 1.5f);
        position.y = Mathf.Clamp(position.y, 0f + cameraHeight/2f, yMax-cameraHeight/2f);

        Debug.Log("Position {" + xMax + "}, {" + yMax + "}");

        return position;
    }






}