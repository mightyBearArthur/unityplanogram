using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour
{

    public float MoveSpeed = 2;

    public float ScrollSpeed = 5;

    public float MaxZoom = 20f;

    public float MinZoom = 5f;

    private Vector3 MoveOrigin;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        ZoomCamera();
    }
    
    public void MoveCamera()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MoveOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0) || !Input.GetKey("space")) return;

        Vector3 pos = Camera.main.ScreenToWorldPoint(MoveOrigin) - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 move = new Vector3(pos.x * MoveSpeed, pos.y * MoveSpeed, pos.z);

        transform.Translate(move, Space.World);
        MoveOrigin = Input.mousePosition;
    }

    public void ZoomCamera()
    {
        var scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll == 0)
        {
            return;
        }

        var orthSize = Camera.main.orthographicSize - scroll * ScrollSpeed;
        Camera.main.orthographicSize = Mathf.Min(MaxZoom, Mathf.Max(MinZoom, orthSize));
    }

}
