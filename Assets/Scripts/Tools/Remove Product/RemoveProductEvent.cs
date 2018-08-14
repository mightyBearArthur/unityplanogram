using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class RemoveProductEvent : PlanogramEventsBase<RemoveProductEvent>, IPointerClickHandler
{

    private bool eventEnabled = false;

    private bool eventPaused = false;

    // Update is called once per frame
    void Update()
    {

    }

    public void Bind()
    {
        if (eventEnabled) return;

        eventEnabled = true;
        PlanogramEventsState.instance.Disable();
        PlanogramEventsState.instance.Enable<RemoveProductEvent>(gameObject);
    }

    public override void Unbind()
    {
        eventEnabled = false;
    }

    public override void Pause()
    {
        eventPaused = true;
    }

    public override void Unpause()
    {
        eventPaused = false;
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (eventPaused) return;

        RaycastHit2D hit = Physics2D.Raycast(
            Camera.main.ScreenToWorldPoint(Input.mousePosition),
            Vector2.zero,
            Mathf.Infinity,
            LayerMask.GetMask("Products")
        );

        if (hit.collider == null) return;

        ProductsContainer.instance.products.Remove(hit.collider.gameObject);
        Destroy(hit.collider.gameObject);
    }

}
