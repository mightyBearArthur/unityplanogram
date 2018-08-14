using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Radiobutton), typeof(Image))]
public class ToolButton : MonoBehaviour
{

    public Sprite defaultSprite;

    public Sprite selectedSprite;

    // Use this for initialization
    void Start()
    {
        Radiobutton radioButton = GetComponent<Radiobutton>();
        radioButton.OnSelectEvent += Select;
        radioButton.OnDeselectEvent += Deselect;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Select()
    {
        Image image = GetComponent<Image>();
        image.sprite = selectedSprite;
    }

    public void Deselect()
    {
        Image image = GetComponent<Image>();
        image.sprite = defaultSprite;
    }

}
