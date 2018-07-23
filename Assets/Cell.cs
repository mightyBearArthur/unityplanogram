using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {


    private Vector3 _prevePosition;

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

        transform.position = GetCellPosUnderMouse();
    }

    void OnMouseDragStart()
    {
        _prevePosition = transform.position;
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

    private Vector3 GetCellPosUnderMouse()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var stepSize = GetStepSize();

        return new Vector3(
            Mathf.Sign(mousePosition.x) * (((int)(Mathf.Abs(mousePosition.x) / stepSize.x) + 0.5f) * stepSize.x),
            Mathf.Sign(mousePosition.y) * (((int)(Mathf.Abs(mousePosition.y) / stepSize.y) + 0.5f) * stepSize.y),
            -1f
        );

    }

}
