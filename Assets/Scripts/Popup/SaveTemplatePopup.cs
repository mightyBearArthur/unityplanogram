using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SaveTemplatePopup : MonoBehaviour
{
    public delegate void Submit(string name);

    public event Submit OnSubmitEvent;

    public delegate void Cancel();

    public event Cancel OnCancelEvent;

    public GameObject name;

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
        if (name.GetComponent<InputField>().text == string.Empty) return;

        if (OnSubmitEvent != null)
            OnSubmitEvent(name.GetComponent<InputField>().text);

        Popup.instance.Close();
    }

    public void OnCancel()
    {
        if (OnCancelEvent != null)
            OnCancelEvent();

        Popup.instance.Close();
    }
}
