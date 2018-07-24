using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class PlaceNewBox : MonoBehaviour
{

    public GameObject BoxesContainer;

    public GameObject Box;

    private GameObject CurrentBox;

    private Vector3 initialPoint;

    private Vector2 initialMatrix = new Vector2(1, 1);

    private bool followMouse = true;

    private List<MatrixCell> attemptedToPlace = new List<MatrixCell>();

    // Use this for initialization
    void Start()
    {
        CurrentBox = Instantiate(Box, BoxesContainer.transform);
        attemptedToPlace.Add(new MatrixCell()
        {
            x = 0,
            y = 0,
            gameObject = CurrentBox
        });
        //CurrentBox.gameObject.SetActive(false);

        var trigger = GetComponent<EventTrigger>();

        //var onPointerEnter = new EventTrigger.Entry();
        //onPointerEnter.eventID = EventTriggerType.PointerEnter;
        //onPointerEnter.callback.AddListener((data) => { OnPointerEnter((PointerEventData)data); });
        //trigger.triggers.Add(onPointerEnter);

        //var onPointerExit = new EventTrigger.Entry();
        //onPointerExit.eventID = EventTriggerType.PointerExit;
        //onPointerExit.callback.AddListener((data) => { OnPointerExit((PointerEventData)data); });
        //trigger.triggers.Add(onPointerExit);

        //var onPointerUp = new EventTrigger.Entry();
        //onPointerUp.eventID = EventTriggerType.PointerUp;
        //onPointerUp.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
        //trigger.triggers.Add(onPointerUp);

        //var onPointerDown = new EventTrigger.Entry();
        //onPointerDown.eventID = EventTriggerType.PointerDown;
        //onPointerDown.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
        //trigger.triggers.Add(onPointerDown);
    }

    // Update is called once per frame
    void Update()
    {
        MoveBox();
    }
    
    public void OnPointerEnter(PointerEventData data)
    {
        if (CurrentBox == null) return;
        CurrentBox.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (CurrentBox == null) return;
        CurrentBox.gameObject.SetActive(false);
    }
    
    public void OnPointerUp(PointerEventData data)
    {
        if (Input.GetMouseButtonUp(0))
        {
            Instantiate(CurrentBox, BoxesContainer.transform);
        }

        if (Input.GetMouseButtonUp(1))
        {
            Destroy(CurrentBox);
        }
    }
        
    public void OnPointerDown(PointerEventData data)
    {
        PlaceGrid();
    }

    public void MoveBox()
    {
        Debug.Log(followMouse);
        if (!followMouse || CurrentBox == null || !CurrentBox.gameObject.activeSelf) return;

        var mouseCell = GameManager.instance.GetCellPosUnderMouse();
        CurrentBox.transform.position = new Vector3(mouseCell.x, mouseCell.y, -1f);
    }

    public void OnBeginDrag()
    {
        followMouse = false;
        initialPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void OnDrag()
    {
        PlaceGrid();
    }

    public void OnDragEnd()
    {
        CurrentBox = Instantiate(Box, BoxesContainer.transform);
        followMouse = true;
        attemptedToPlace.Clear();
        attemptedToPlace.Add(new MatrixCell()
        {
            x = 1,
            y = 1,
            gameObject = CurrentBox
        });
    }

    private bool MatrixContains(float x, float y, Vector2 matrix)
    {
        return (x < (int)matrix.x &&
                y < (int)matrix.y);
    }

    private Vector3 GetBoxPosition(int x, int y)
    {
        Vector2 boxSize = Box.GetComponent<BoxCollider2D>().size;
        return new Vector3(
            CurrentBox.transform.position.x + x * boxSize.x,
            CurrentBox.transform.position.y - y * boxSize.y
        );
    }

    private void PlaceGrid()
    {
        Vector3 currentPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distX = Vector3.Distance(initialPoint, new Vector3(currentPoint.x, initialPoint.y, currentPoint.z));
        float distY = Vector3.Distance(initialPoint, new Vector3(initialPoint.x, currentPoint.y, currentPoint.z));

        Vector2 boxSize = Box.GetComponent<BoxCollider2D>().size;
        var currentMatrix = new Vector2((int)(distX / boxSize.x) + 1, (int)(distY / boxSize.y) + 1);

        if (currentMatrix == initialMatrix) return;
        
        var maximumMatrix = new Vector2(
            Mathf.Max(initialMatrix.x, currentMatrix.x), 
            Mathf.Max(initialMatrix.y, currentMatrix.y)
        );

        for (int r = 0; r < maximumMatrix.y; r++)
        {
            for (int c = 0; c < maximumMatrix.x; c++)
            {
                if (MatrixContains(c, r, initialMatrix))
                {
                    if (!MatrixContains(c, r, currentMatrix))
                    {
                        MatrixCell existingBox = attemptedToPlace.Find((item) => item.x == c && item.y == r);
                        Destroy(existingBox.gameObject);
                        attemptedToPlace.Remove(existingBox);
                    }
                }
                else
                {
                    if (MatrixContains(c, r, currentMatrix))
                    {
                        attemptedToPlace.Add(new MatrixCell()
                        {
                            x = c,
                            y = r,
                            gameObject = Instantiate(
                                Box,
                                GetBoxPosition(c, r),
                                Quaternion.identity,
                                BoxesContainer.transform
                            )
                        });
                    }
                }
            }
        }

        initialMatrix = currentMatrix;
    }

}
