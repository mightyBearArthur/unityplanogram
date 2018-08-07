using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MoveTool : PlanogramEventsBase<MoveTool>, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private Vector3 initialPosition;

    private Vector3 shift;

    private GameObject building = null;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBeginDrag(PointerEventData data)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            Camera.main.ScreenToWorldPoint(Input.mousePosition),
            Vector2.zero,
            Mathf.Infinity,
            LayerMask.GetMask("Buildings")
        );

        if (hit.collider == null) return;

        building = hit.collider.gameObject;

        Building buildingHelper = building.GetComponent<Building>();
        BuilderCollider builderCollider = building.GetComponent<BuilderCollider>();
        builderCollider.OnValidStateEvent += buildingHelper.SetValidView;
        builderCollider.ResetState();

        SpriteRenderer spriteRenderer = building.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = buildingHelper.validSprite;

        initialPosition = building.transform.position;
        shift = building.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        building.transform.position = new Vector3(building.transform.position.x, building.transform.position.y, -1f);
    }

    public void OnDrag(PointerEventData data)
    {
        building.transform.position = GridSystem.GetCellPosAtPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition) + shift);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        BuilderCollider builderCollider = building.GetComponent<BuilderCollider>();

        if (!builderCollider.isValid)
        {
            building.transform.position = initialPosition;
        }
        else
        {
            building.transform.position = new Vector3(building.transform.position.x, building.transform.position.y, 0f);
        }

        Building buildingHelper = building.GetComponent<Building>();
        buildingHelper.SetDefaultView();

        builderCollider.OnValidStateEvent -= buildingHelper.SetValidView;
        builderCollider.ResetState();

        building = null;
    }

    public void Bind()
    {
        PlanogramEventsState.instance.Disable();
        PlanogramEventsState.instance.Enable<MoveTool>(gameObject);
    }

    public override void Unbind()
    {
        building = null;
    }

}
