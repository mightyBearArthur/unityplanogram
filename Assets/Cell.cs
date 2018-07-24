using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {


    private Vector3 _prevePosition;

    private Vector3 _shift;

    private bool _dragging = false;

    public Vector2 GridSize;

    public int GridScale;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (_dragging && Input.GetMouseButtonUp(0))
        {
            OnMouseDragEnd();
            _dragging = false;
        } 
	}

    void OnMouseDrag()
    {
        if (!_dragging)
        {
            _dragging = true;
            OnMouseDragStart();
        }

        transform.position = GetCellAtPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition) + _shift);
    }

    void OnMouseDragStart()
    {
        _prevePosition = transform.position;
        _shift = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
    }

    void OnMouseDragEnd()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    private Vector2 GetStepSize()
    {
        return GridSize / GridScale;
    }

    private Vector3 GetCellAtPoint(Vector3 point)
    {
        var stepSize = GetStepSize();

        return new Vector3(
            Mathf.Sign(point.x) * (((int)(Mathf.Abs(point.x) / stepSize.x) + 0.5f) * stepSize.x),
            Mathf.Sign(point.y) * (((int)(Mathf.Abs(point.y) / stepSize.y) + 0.5f) * stepSize.y),
            -1f
        );

    }

}
