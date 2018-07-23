using UnityEngine;
using System.Collections;

public class GridSystem : MonoBehaviour
{

    public Vector2 GridSize;

    public int GridScale;

    public Vector3 GetCellPosUnderMouse()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var stepSize = GetStepSize();

        return new Vector3(
            Mathf.Sign(mousePosition.x) * (((int)(Mathf.Abs(mousePosition.x) / stepSize.x) + 0.5f) * stepSize.x),
            Mathf.Sign(mousePosition.y) * (((int)(Mathf.Abs(mousePosition.y) / stepSize.y) + 0.5f) * stepSize.y),
            -1f
        );

    }

    public Vector2 GetStepSize()
    {
        return GridSize / GridScale;
    }

}
