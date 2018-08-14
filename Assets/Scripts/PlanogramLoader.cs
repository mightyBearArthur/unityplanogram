using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using Newtonsoft.Json;
using System.Linq;


public class PlanogramLoader : MonoBehaviour
{

    public static PlanogramLoader instance = null;

    [DllImport("__Internal")]
    public static extern string GetQueryStringParam(string name);

    public PlanogramViewModel planogram;

    public BackwallViewModel backwall;

    public GameObject backwallTemplatePopup;

    public bool isNew;

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
        string stringId = GetQueryStringParam("id");
        if (stringId != string.Empty)
        {
            isNew = false;
            Guid id = new Guid(stringId);
            StartCoroutine(GetPlanogram(id));
        }
        else
        {
            isNew = true;
            var popupInstance = Popup.instance.Open(backwallTemplatePopup);
            popupInstance.GetComponent<BackwallTemplatePopup>().OnSubmitEvent += delegate(Guid id) 
            {
                StartCoroutine(GetBackwallTemplate(id));
            };
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator GetPlanogram(Guid id)
    {
        var www = new WWW("/Planogram/GetByIdWithProducts/?id=" + id.ToString());
        yield return www;

        if (www.error != null)
        {
            Debug.Log(www.error);
        }
        else
        {
            planogram = JsonConvert.DeserializeObject<PlanogramViewModel>(www.text);
            StartCoroutine(GetBackwallTemplate(planogram.PlanogramTemplateId));
        }
    }

    public IEnumerator GetBackwallTemplate(Guid id)
    {
        var www = new WWW("/PlanogramTemplate/GetById/?id=" + id.ToString() + "&withCells=true");
        yield return www;

        if (www.error != null)
        {
            Debug.Log(www.error);
        }
        else
        {
            backwall = JsonConvert.DeserializeObject<BackwallViewModel>(www.text);
            GenerateBackwall();

            if (!isNew)
            {
                GeneratePlanogram();
            }
        }
    }

    private void GenerateBackwall()
    {
        List<BackwallCellViewModel> cells = new List<BackwallCellViewModel>();

        foreach(List<BackwallCellViewModel> row in backwall.Cells)
        {
            foreach(BackwallCellViewModel col in row)
            {
                cells.Add(col);
            }
        }

        CellsContainer.instance.SetCells(cells);
    }

    private void GeneratePlanogram()
    {
        ProductsContainer.instance.SetProducts(planogram.Cells);
    }

}
