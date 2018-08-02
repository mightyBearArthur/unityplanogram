using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Building : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{

    private Vector3 initialPosition;

    private Vector3 shift;

    private bool isChecked = false;
    
    public bool isMoving = false;

    private bool selected = false;

    private BuilderCollider builderCollider;

    // Use this for initialization
    void Start()
    {
        builderCollider = GetComponent<BuilderCollider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        builderCollider.OnValidStateEvent += SetValidView;

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
        if (!builderCollider.isValid)
        {
            transform.position = initialPosition;
            builderCollider.ResetState();
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        }

        builderCollider.OnValidStateEvent -= SetValidView;
        isMoving = false;
    }
        
    public void OnPointerClick(PointerEventData data)
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            SelectionHandler.instance.selected.Clear();
        }

        SelectionHandler.instance.selected.Add(gameObject);
    }

    public void SetValidView(bool state)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        color.a = state ? 1f : 0.3f;
        spriteRenderer.color = color;
    }

}
