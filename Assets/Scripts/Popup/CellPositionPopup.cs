using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CellPositionPopup : MonoBehaviour
{

    public delegate void Submit(Building.CellPosition pos);

    public event Submit OnSubmitEvent;

    public delegate void Cancel();

    public event Cancel OnCancelEvent;

    public GameObject row;

    public GameObject col;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnSubmit()
    {
        if (row.GetComponent<InputField>().text == string.Empty ||
            col.GetComponent<InputField>().text == string.Empty) return;

        if (OnSubmitEvent != null)
            OnSubmitEvent(new Building.CellPosition() {
                row = int.Parse(row.GetComponent<InputField>().text),
                col = int.Parse(col.GetComponent<InputField>().text)
            });

        Popup.instance.Close();
    }

    public void OnCancel()
    {
        if (OnCancelEvent != null)
            OnCancelEvent();

        Popup.instance.Close();
    }

}
