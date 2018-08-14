using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ProductToolButton : MonoBehaviour, IPointerClickHandler
{

    public GameObject imageGameObject;

    public GameObject textGameObject;

    public ProductViewModel productModel { get; private set; }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetProductModel(ProductViewModel model)
    {
        productModel = model;

        Texture2D texture = new Texture2D(50, 80, TextureFormat.ARGB32, false);
        texture.LoadImage(model.Image);
        texture.Apply();

        imageGameObject.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        // Set text.
        textGameObject.GetComponent<Text>().text = model.Name;
    }

    public void OnPointerClick(PointerEventData data)
    {
        PlaceProduct.instance.Place(productModel);
    }

}
