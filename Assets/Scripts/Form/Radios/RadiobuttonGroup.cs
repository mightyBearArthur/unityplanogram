using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class RadiobuttonGroup : MonoBehaviour
{

    [SerializeField]
    public List<GameObject> radiobuttons;

    public Radiobutton selected { get; private set; }

    // Use this for initialization
    void Start()
    {
        foreach(GameObject button in radiobuttons)
        {
            button.GetComponent<Radiobutton>().group = this;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Select(Radiobutton button)
    {
        if (selected == button) return;

        if (selected != null)
        {
            selected.OnDeselect();
        }

        selected = button;
        selected.OnSelect();
    }

}
