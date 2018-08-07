using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class RemoveTool : PlanogramEventsBase<RemoveTool>, IPointerClickHandler
{

    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData data)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            Camera.main.ScreenToWorldPoint(Input.mousePosition), 
            Vector2.zero,
            Mathf.Infinity,
            LayerMask.GetMask("Buildings")
        );

        if (hit.collider == null) return;

        Destroy(hit.collider.gameObject);
    }

    public void Bind()
    {
        PlanogramEventsState.instance.Disable();
        PlanogramEventsState.instance.Enable<RemoveTool>(gameObject);
    }
    
    public override void Unbind()
    {

    }

}
