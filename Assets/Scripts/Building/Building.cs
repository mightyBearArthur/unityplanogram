using UnityEngine;

public class Building : MonoBehaviour
{
    
    public class CellPosition
    {

        public int row;

        public int col;

    }

    public GameObject rowText;

    public GameObject colText;

    public Sprite defaultSprite;

    public Sprite validSprite;

    public Sprite invalidSprite;

    public CellPosition position { get; private set; }

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

    public void SetPosition(CellPosition pos)
    {
        position = pos;

        rowText.GetComponent<TextMesh>().text = pos.row.ToString();
        colText.GetComponent<TextMesh>().text = pos.col.ToString();
    }

}
