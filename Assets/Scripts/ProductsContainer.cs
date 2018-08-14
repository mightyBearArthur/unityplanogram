using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProductsContainer : MonoBehaviour
{

    public GameObject productPrefab;

    public static ProductsContainer instance = null;

    public List<GameObject> products { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        products = new List<GameObject>();
    }
    
    public void SetProducts(List<PlanogramCellViewModel> cells)
    {
        foreach(PlanogramCellViewModel cell in cells)
        {
            // Get related backwall cell for product.
            GameObject backwallCell = CellsContainer.instance.cells.Find(x => 
            {
                BackwallCell backwallCellComponent = x.GetComponent<BackwallCell>();

                return backwallCellComponent.cellModel.Row == cell.Row &&
                    backwallCellComponent.cellModel.Column == cell.Column;
            });

            GameObject cellInstance = Instantiate(
                productPrefab,
                transform
            );

            cellInstance.transform.localPosition = new Vector3(
                backwallCell.transform.position.x,
                backwallCell.transform.position.y,
                -1f
            );

            cellInstance.GetComponent<Product>().SetProductModel(cell);

            products.Add(cellInstance);
        }
    }

}
