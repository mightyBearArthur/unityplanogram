using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.UI;

public class BrandsDropdown : MonoBehaviour
{

    private List<ProductGroup> productGroups;

    // Use this for initialization
    void Start()
    {
        GetBrdands();
    }

    // Update is called once per frame
    void Update()
    {

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
            Dropdown dropdown = GetComponent<Dropdown>();
            dropdown.options = new List<Dropdown.OptionData>();

            foreach (var group in productGroups)
            {
                dropdown.options.Add(new Dropdown.OptionData(group.Name));
            }
        }
    }

}
