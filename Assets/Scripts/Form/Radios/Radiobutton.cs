using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Radiobutton : EventTrigger
{

    public delegate void Select();

    public event Select OnSelectEvent;

    public delegate void Deselect();

    public event Deselect OnDeselectEvent;

    public RadiobuttonGroup group = null;
    
    public override void OnPointerClick(PointerEventData data)
    {
        if (data.button != PointerEventData.InputButton.Left ||
            group.selected == this) return;

        group.Select(this);
        base.OnPointerClick(data);
    }

    public void OnSelect()
    {
        if (OnSelectEvent != null) OnSelectEvent();
    }

    public void OnDeselect()
    {
        if (OnDeselectEvent != null) OnDeselectEvent();
    }

}
