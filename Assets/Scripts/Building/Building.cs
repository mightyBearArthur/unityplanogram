using UnityEngine;

public class Building : MonoBehaviour
{
    
    public Sprite defaultSprite;

    public Sprite validSprite;

    public Sprite invalidSprite;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetDefaultView()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = defaultSprite;
    }

    public void SetValidView(bool state)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (state)
        {
            spriteRenderer.sprite = validSprite;
        }
        else
        {
            spriteRenderer.sprite = invalidSprite;
        }
    }

}
