using UnityEngine;
using System.Collections;

public class PlaceNewBox : MonoBehaviour
{

    public GridSystem GridSystem;

    public GameObject BoxesContainer;

    public GameObject Box;

    private GameObject CurrentBox;

    // Use this for initialization
    void Start()
    {
        CurrentBox = Instantiate(Box, BoxesContainer.transform);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseEnter()
    {
        CurrentBox.gameObject.SetActive(true);
    }

    void OnMouseExit()
    {
        Debug.Log("er1");
        CurrentBox.gameObject.SetActive(false);
    }

    void OnMouseOver()
    {
        Debug.Log("er");
        var mouseCell = GridSystem.GetCellPosUnderMouse();
        CurrentBox.transform.position = new Vector3(mouseCell.x, mouseCell.y, -1f);
    }


    void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            Instantiate(CurrentBox, BoxesContainer.transform);
        }

        if (Input.GetMouseButton(1))
        {
            Destroy(CurrentBox);
        }
    }

    public void OnMove(AxisEventData eventData)
    {
        Debug.Log("move");
    }
}
