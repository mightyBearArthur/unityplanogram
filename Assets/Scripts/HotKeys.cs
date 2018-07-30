using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class HotKeys : MonoBehaviour
{

    private bool cameraMoveEnabled = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        OnSpaceKeyDown();
        OnSpaceKeyUp();

        OnCtrlKeyDown();
        OnCtrlKeyUp();
    }

    void OnSpaceKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !cameraMoveEnabled)
        {
            CameraMove.instance.EnableMovement();
            cameraMoveEnabled = true;
        }
    }

    void OnSpaceKeyUp()
    {
        if (Input.GetKeyUp(KeyCode.Space) && cameraMoveEnabled)
        {
            CameraMove.instance.DisableMovement();
            cameraMoveEnabled = false;
        }
    }

    void OnCtrlKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !cameraMoveEnabled)
        {
            CameraMove.instance.EnableMovement();
            cameraMoveEnabled = true;
        }
    }

    void OnCtrlKeyUp()
    {
        if (Input.GetKeyUp(KeyCode.LeftControl) && cameraMoveEnabled)
        {
            CameraMove.instance.DisableMovement();
            cameraMoveEnabled = false;
        }
    }

}
