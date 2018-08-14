using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Product : MonoBehaviour
{

    public PlanogramCellViewModel model { get; private set; }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetProductModel(PlanogramCellViewModel product)
    {
        model = product;

        Texture2D texture = new Texture2D(133, 167, TextureFormat.ARGB32, false);
        texture.LoadImage(model.ProductImage);
        texture.Apply();

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        spriteRenderer.sprite = sprite;
        spriteRenderer.size = new Vector2(1.333333f, 1.666667f);
    }

    public void SetSpriteAlpha(float a)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.material.color;
        color.a = a;
        spriteRenderer.material.color = color;
    }

    public void SetBackwallPosition(int row, int col)
    {
        model.Row = row;
        model.Column = col;
    }
        
}
