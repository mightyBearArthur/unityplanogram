using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProductsScrollView : MonoBehaviour
{

    public GameObject scrollViewContent;

    private List<ProductViewModel> products;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetProducts(List<ProductViewModel> items)
    {
        products = items;
        GenerateProducts();
    }

    private void GenerateProducts()
    {
        // Clear old producst.
        foreach (Transform child in scrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }


    }

}
