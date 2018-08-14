using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json;

public class ProductsForm : MonoBehaviour
{

    private List<ProductGroup> productGroups;

    public GameObject productToolPrefab;

    public GameObject dropdownGameObject;

    public GameObject dropdownLabelGameObject;

    public GameObject scrollViewContent;

    public GameObject createPlanogramPopup;

    public List<ProductViewModel> products { get; private set; }

    private int coroutinesCount = 0;

    // Use this for initialization
    void Start()
    {
        GetBrdands();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBrandSelected(int index)
    {
        int value = dropdownGameObject.GetComponent<Dropdown>().value;
        ProductGroup selectedGroup = productGroups[value];

        if (selectedGroup == null) return;
        GetProducts(selectedGroup);
    }

    public void GetBrdands()
    {
        StartCoroutine(GetBrandsReq());
    }

    public IEnumerator GetBrandsReq()
    {
        var www = new WWW("/Planogram/GetProductGroups");
        yield return www;

        if (www.error != null)
        {
            Debug.Log(www.error);
        }
        else
        {
            productGroups = JsonConvert.DeserializeObject<List<ProductGroup>>(www.text);

            Dropdown dropdown = dropdownGameObject.GetComponent<Dropdown>();
            dropdown.options = new List<Dropdown.OptionData>();

            foreach (var group in productGroups)
            {
                dropdown.options.Add(new Dropdown.OptionData(group.Name));
            }

            dropdownLabelGameObject.GetComponent<Text>().text = "select brand...";
        }
    }

    public void GetProducts(ProductGroup group)
    {
        StartCoroutine(GetProdutsReq(group.Id));
    }

    private IEnumerator GetProdutsReq(Guid id)
    {
        WWWForm form = new WWWForm();
        form.AddField("groupId", id.ToString());

        var www = new WWW("/Planogram/GetProductsByGroupId", form);

        yield return www;

        if (www.error != null)
        {
            Debug.Log(www.error);
        }
        else
        {
            products = JsonConvert.DeserializeObject<List<ProductViewModel>>(www.text);

            coroutinesCount = products.Count;

            foreach(var product in products)
            {
                StartCoroutine(GetProductImage(product.Id));
            }
        }
    }

    private IEnumerator GetProductImage(Guid id)
    {
        var wwwForm = new WWWForm();
        wwwForm.AddField("id", id.ToString());

        var www = new WWW("/Product/GetImage", wwwForm);
        yield return www;

        if (www.error != null)
        {
            Debug.Log(www.error);
            coroutinesCount--;
        }
        else
        {
            var product = products.Find(x => x.Id == id);
            product.Image = www.bytes;
            coroutinesCount--;

            if (coroutinesCount == 0)
            {
                // Clear old.
                foreach (Transform child in scrollViewContent.transform)
                {
                    Destroy(child.gameObject);
                }

                RectTransform scrollViewContentRectTransform = scrollViewContent.GetComponent<RectTransform>();
                RectTransform productToolPrefabRectTransform = productToolPrefab.GetComponent<RectTransform>();

                // Set scrollView container height.
                scrollViewContentRectTransform.sizeDelta = new Vector2(
                    scrollViewContentRectTransform.sizeDelta.x,
                    productToolPrefabRectTransform.sizeDelta.y * products.Count
                );

                if (products.Count != 0)
                {

                    // Set first tool rect position.
                    Vector3 productToolPosition = new Vector3(
                        0f,
                        (scrollViewContentRectTransform.sizeDelta.y / 2) - (productToolPrefabRectTransform.sizeDelta.y / 2),
                        0f
                    );

                    // Generate producst tool buttons.
                    foreach (ProductViewModel model in products)
                    {
                        // Create product tool button.
                        var productToolInstance = Instantiate(productToolPrefab, scrollViewContent.transform);
                        productToolInstance.GetComponent<RectTransform>().anchoredPosition = productToolPosition;
                        productToolInstance.GetComponent<ProductToolButton>().SetProductModel(model);

                        // Calculate next item position.
                        productToolPosition -= new Vector3(0f, productToolPrefabRectTransform.sizeDelta.y, 0f);
                    }
                }
            }
        }
    }

    public void OpenCreatePopup()
    {
        if (PlanogramLoader.instance.isNew)
        {
            GameObject popupInstance = Popup.instance.Open(createPlanogramPopup);
            popupInstance.GetComponent<CreatePlanogramPopup>().OnSubmitEvent += CreatePlanogram;
        }
        else
        {
            UpdatePlanogram();
        }
    }

    public void CreatePlanogram(string name)
    {
        StartCoroutine(CreatePlanogramReq(name));
    }

    private IEnumerator CreatePlanogramReq(string name)
    {
        var planogram = new PlanogramParamModel()
        {
            PlanogramTemplateId = PlanogramLoader.instance.backwall.Id,
            Name = name,
            Cells = new List<List<PlanogramCellParamModel>>()
            {
                ProductsContainer.instance.products.Select(x =>
                {
                    var cellModel = x.GetComponent<Product>().model;

                    return new PlanogramCellParamModel()
                    {
                        ProductId = cellModel.ProductId,
                        Row = cellModel.Row,
                        Column = cellModel.Column
                    };
                }).ToList()
            }
        };

        planogram.RowsCount = planogram.Cells.Count;

        var json = JsonConvert.SerializeObject(planogram);
        var headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(json);
        var www = new WWW("/Planogram/Create", postData, headers);

        yield return www;

        if (www.error != null)
        {
            Debug.Log(www.error);
        }
        else
        {

        }
    }

    public void UpdatePlanogram()
    {
        StartCoroutine(UpdatePlanogramReq());
    }

    private IEnumerator UpdatePlanogramReq()
    {
        var planogram = new PlanogramParamModel()
        {
            Id = PlanogramLoader.instance.planogram.Id,
            PlanogramTemplateId = PlanogramLoader.instance.backwall.Id,
            Name = name,
            Cells = new List<List<PlanogramCellParamModel>>()
            {
                ProductsContainer.instance.products.Select(x =>
                {
                    var cellModel = x.GetComponent<Product>().model;

                    return new PlanogramCellParamModel()
                    {
                        Id = cellModel.Id,
                        PlanogramId = PlanogramLoader.instance.planogram.Id,
                        ProductId = cellModel.ProductId,
                        Row = cellModel.Row,
                        Column = cellModel.Column
                    };
                }).ToList()
            }
        };

        planogram.RowsCount = planogram.Cells.Count;

        var json = JsonConvert.SerializeObject(planogram);
        var headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(json);
        var www = new WWW("/Planogram/Edit", postData, headers);

        yield return www;

        if (www.error != null)
        {
            Debug.Log(www.error);
        }
        else
        {

        }
    }

}
