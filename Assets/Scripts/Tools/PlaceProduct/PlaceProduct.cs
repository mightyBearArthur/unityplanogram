using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class PlaceProduct : PlanogramEventsBase<PlaceProduct>, IPointerClickHandler
{

    private ProductViewModel currentModel;

    private GameObject currentCell;

    private Texture2D currentCellTexture;

    private Vector3 mousePrevPos;

    private GameObject productInstance;

    public GameObject productPrefab;

    public GameObject productsContainer;

    public GameObject hiddenProduct;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        OnMouseMove();
    }

    public void Place(ProductViewModel model)
    {
        if (currentModel == model) return;


        if (productInstance == null)
        {
            PlanogramEventsState.instance.Disable();
            PlanogramEventsState.instance.Enable<PlaceProduct>(gameObject);
            productInstance = Instantiate(productPrefab, productsContainer.transform);
            productInstance.transform.localPosition = new Vector3(0f, 0f, -1.0001f);
            productInstance.SetActive(false);
        }

        Product productComponent = productInstance.GetComponent<Product>();
        currentModel = model;
        productComponent.SetProductModel(new PlanogramCellViewModel()
        {
            ProductId = currentModel.Id,
            ProductDescription = currentModel.Description,
            ProductImage = currentModel.Image,
            ProductName = currentModel.Name
        });
    }

    public override void Unbind()
    {
        Destroy(productInstance);
        currentModel = null;
    }

    private void OnMouseMove()
    {
        if (mousePrevPos == Input.mousePosition)
            return;

        mousePrevPos = Input.mousePosition;

        RaycastHit2D hit = Physics2D.Raycast(
            Camera.main.ScreenToWorldPoint(Input.mousePosition),
            Vector2.zero,
            Mathf.Infinity,
            LayerMask.GetMask("Buildings")
        );

        if (currentCell != null)
        {
            // New Cell
            if (hit.collider != null && currentCell != hit.collider.gameObject)
            {
                currentCell = hit.collider.gameObject;
                ShowProduct();
                HideProduct();
                MoveProduct();
                productInstance.SetActive(true);
            }
            // Empty
            else if (hit.collider == null)
            {
                currentCell = null;
                ShowProduct();
                productInstance.SetActive(false);
            }

        }
        else
        {
            if (hit.collider != null)
            {
                currentCell = hit.collider.gameObject;
                ShowProduct();
                HideProduct();
                MoveProduct();
                productInstance.SetActive(true);
            }
        }
    }

    private void MoveProduct()
    {
        productInstance.transform.localPosition = new Vector3(
            currentCell.transform.position.x,
            currentCell.transform.position.y,
            -1f
        );

        BackwallCell backwallCellComponent = currentCell.GetComponent<BackwallCell>();
        productInstance.GetComponent<Product>().SetBackwallPosition(backwallCellComponent.cellModel.Row, backwallCellComponent.cellModel.Column);
    }


    private void HideProduct()
    {
        BackwallCell currentCellData = currentCell.GetComponent<BackwallCell>();
        GameObject existing = ProductsContainer.instance.products.Find(obj =>
        {
            Product productComponent = obj.GetComponent<Product>();
            return productComponent.model.Row == currentCellData.cellModel.Row &&
                productComponent.model.Column == currentCellData.cellModel.Column;
        });

        if (existing == null) return;

        hiddenProduct = existing;
        hiddenProduct.SetActive(false);
    }

    private void ShowProduct()
    {
        if (hiddenProduct != null)
        {
            hiddenProduct.SetActive(true);
            hiddenProduct = null;
        }
    }


    public void OnPointerClick(PointerEventData data)
    {
        if (currentCell == null) return;

        ShowProduct();

        BackwallCell currentCellData = currentCell.GetComponent<BackwallCell>();
        GameObject existing = ProductsContainer.instance.products.Find(obj => 
        {
            Product productComponent = obj.GetComponent<Product>();
            return productComponent.model.Row == currentCellData.cellModel.Row &&
                productComponent.model.Column == currentCellData.cellModel.Column;
        });

        var newModel = new PlanogramCellViewModel()
        {
            ProductId = currentModel.Id,
            ProductDescription = currentModel.Description,
            ProductImage = currentModel.Image,
            ProductName = currentModel.Name,
            Row = currentCellData.cellModel.Row,
            Column = currentCellData.cellModel.Column
        };

        if (existing != null)
        {
            existing.GetComponent<Product>().SetProductModel(newModel);
        }
        else
        {
            GameObject cellInstance = Instantiate(productPrefab, productsContainer.transform);
            cellInstance.transform.localPosition = new Vector3(
                currentCell.transform.position.x,
                currentCell.transform.position.y,
                -1f
            );
            cellInstance.GetComponent<Product>().SetProductModel(newModel);

            ProductsContainer.instance.products.Add(cellInstance);
        }
    }

}
