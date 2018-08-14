using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using System.Linq;

public class HttpService : MonoBehaviour
{

    public GameObject buildingsContainer;

    public GameObject savePopup;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void OpenSavePopup()
    {
        PlanogramEventsState.instance.Disable();
        GameObject savePopupInstance = Popup.instance.Open(savePopup);
        savePopupInstance.GetComponent<SaveTemplatePopup>().OnSubmitEvent += SaveNewBackwallTemplate;
    }

    public void SaveNewBackwallTemplate(string name)
    {
        BackwallTemplateParamModel model = new BackwallTemplateParamModel()
        {
            name = name,
            cells = new List<List<BackwallCellParam>>()
            {
                new List<BackwallCellParam>()
            }
        };


        foreach (Transform child in buildingsContainer.transform)
        {
            GameObject obj = child.gameObject;
            Building buildingHelper = obj.GetComponent<Building>();

            model.cells[0].Add(new BackwallCellParam()
            {
                row = buildingHelper.position.row,
                column = buildingHelper.position.col,
                canvasHeight = obj.transform.localScale.y,
                canvasWidth = obj.transform.localScale.x,
                canvasX = obj.transform.position.x,
                canvasY = obj.transform.position.y
            });
        }

        StartCoroutine(SaveNewBackwallTemplateReq(model));
    }

    private IEnumerator SaveNewBackwallTemplateReq(BackwallTemplateParamModel model)
    {
        string json = JsonConvert.SerializeObject(model);
        var headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json; charset=UTF-8");
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(json);
        var www = new WWW("/PlanogramTemplate/Create", postData, headers);

        yield return www;

        if (www.error != null)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Ok");
        }
    }
    
}
