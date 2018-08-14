using UnityEngine;
using System.Collections;

public class CameraMove : PlanogramEventsBase<CameraMove>
{
    
    public float MoveSpeed = 2;

    public float ScrollSpeed = 5;

    public float MaxZoom = 20f;

    public float MinZoom = 5f;

    private Vector3 MoveOrigin;

    public override void Start()
    {
        base.Start();

        GameObject eventsArea = PlanogramEventsState.instance.gameObject;
        GameObject platform = Platform.instance.gameObject;

        float platformTop = platform.transform.position.y + platform.transform.localScale.y / 2;
        float platformRigfht = platform.transform.position.x + platform.transform.localScale.x / 2;
        float platformBottom = platform.transform.position.y - platform.transform.localScale.y / 2;
        float platformLeft = platform.transform.position.x - platform.transform.localScale.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        ZoomCamera();
    }
    
    public void EnableMovement()
    {
        PlanogramEventsState.instance.Enable<CameraMove>(gameObject);
    }

    public void DisableMovement()
    {
        PlanogramEventsState.instance.Disable();
    }

    public void MoveCamera()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MoveOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0) || !Input.GetKey(KeyCode.Space)) return;

        Vector3 pos = Camera.main.ScreenToWorldPoint(MoveOrigin) - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 move = new Vector3(pos.x * MoveSpeed, pos.y * MoveSpeed, pos.z);

        Camera.main.transform.Translate(move, Space.World);
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
