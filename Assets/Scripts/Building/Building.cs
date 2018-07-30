using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Building : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    private Vector3 initialPosition;

    private Vector3 shift;

    private bool isChecked = false;

    private SpriteRenderer spriteRenderer;

    public bool isMoving = false;

    public bool isValid { get; private set; }

    private int _collisionCount = 0;
    private int collisionCount
    {
        get
        {
            return _collisionCount;
        }

        set
        {
            _collisionCount = value;
            isValid = value == 0;

            Color color = spriteRenderer.color;
            color.a = isValid ? 1f : 0.3f;
            spriteRenderer.color = color;
        }
    }

    // Use this for initialization
    void Start()
    {
        isValid = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isChecked = false;
        isMoving = true;
        initialPosition = transform.position;
        shift = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = GridSystem.GetCellPosAtPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition) + shift);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isValid)
        {
            transform.position = initialPosition;
            collisionCount = 0;
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        }

        isMoving = false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!isMoving) return;
        collisionCount++;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (!isMoving) return;
        collisionCount--;
    }

}
