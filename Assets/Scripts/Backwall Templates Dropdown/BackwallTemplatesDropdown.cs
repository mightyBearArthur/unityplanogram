using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

[RequireComponent(typeof(Dropdown))]
public class BackwallTemplatesDropdown : MonoBehaviour
{

    public List<BackwallViewModel> templates { get; private set; }

    public GameObject label;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(GetBackwallTemplates());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator GetBackwallTemplates()
    {
        var www = new WWW("/PlanogramTemplate/GetAll");
        yield return www;

        if (www.error != null)
        {
            Debug.Log(www.error);
        }
        else
        {
            templates = JsonConvert.DeserializeObject<List<BackwallViewModel>>(www.text);
            Dropdown dropdownComponent = GetComponent<Dropdown>();
            dropdownComponent.options = new List<Dropdown.OptionData>();
            
            foreach(BackwallViewModel template in templates)
            {
                dropdownComponent.options.Add(new Dropdown.OptionData(template.Name));
            }

            label.GetComponent<Text>().text = "select template";
        }
    }

}
