using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class BackwallTemplatePopup : MonoBehaviour
{

    public delegate void Submit(Guid id);

    public event Submit OnSubmitEvent;

    public delegate void Cancel();

    public event Cancel OnCancelEvent;

    public GameObject templateId;

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
        int value = templateId.GetComponent<Dropdown>().value;
        Guid id = templateId.GetComponent<BackwallTemplatesDropdown>().templates[value].Id;

        if (OnSubmitEvent != null)
            OnSubmitEvent(id);

        Popup.instance.Close();
    }

    public void OnCancel()
    {
        if (OnCancelEvent != null)
            OnCancelEvent();

        Popup.instance.Close();
    }

}
