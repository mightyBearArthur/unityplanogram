using UnityEngine;
using System.Collections;

public class GridSystem : MonoBehaviour
{
    
    /// <summary>
    /// Get cell position at mouse position.
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetCellPosAtPoint(Vector3 point)
    {
        var stepSize = GameManager.instance.GridSize;

        // @todo fix hardcode..
        return new Vector3(
            Mathf.Sign(point.x) * (((int)(Mathf.Abs(point.x) / stepSize.x)) * stepSize.x + (stepSize.x / 2)),
            Mathf.Sign(point.y) * (((int)(Mathf.Abs(point.y) / stepSize.y)) * stepSize.y),
            -1f
        );

    }

    public static Vector3 GetCellPosAtPoint()
    {
        return GridSystem.GetCellPosAtPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    /// <summary>
    /// Get step size.
    /// </summary>
    /// <returns></returns>
    public static Vector2 GetStepSize()
    {
        return GameManager.instance.GridSize;
    }

}
