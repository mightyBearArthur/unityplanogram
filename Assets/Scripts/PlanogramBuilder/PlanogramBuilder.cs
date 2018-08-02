using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PlanogramBuilder : PlanogramEventsBase<PlanogramBuilder>, IPointerClickHandler
{

    public GameObject productsContainer = null;

    private bool buildStarted = false;

    private GameObject productPrefab = null;

    private GameObject currentProduct = null;
    
    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        MoveProduct();
    }

    public void SetProduct(GameObject prefab)
    {
        if (buildStarted)
        {
            if (productPrefab == prefab)
                return;
            else
                Destroy(currentProduct);
        }

        productPrefab = prefab;

        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        currentProduct = Instantiate(
            productPrefab,
            position,
            Quaternion.identity,
            productsContainer.transform
        );

        PlanogramEventsState.instance.Enable<PlanogramBuilder>(gameObject);

        buildStarted = true;
    }


    public void Stop()
    {
        Destroy(currentProduct);
        PlanogramEventsState.instance.Disable();
    }

    private void MoveProduct()
    {
        currentProduct.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void OnPointerClick(PointerEventData data)
    {

    }

}
