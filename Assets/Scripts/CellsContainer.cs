using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using Newtonsoft.Json;

public class CellsContainer : MonoBehaviour
{

    [DllImport("__Internal")]
    public static extern string GetBackwallTemplateId();

    public GameObject cellPrefab;

    public static CellsContainer instance = null;

    public List<GameObject> cells;

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

    void Start()
    {
        cells = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCells(List<BackwallCellViewModel> items)
    {
        foreach (var cell in items)
        {
            GameObject cellInstance = Instantiate(
                cellPrefab,
                Vector3.zero,
                Quaternion.identity,
                transform
            );

            cellInstance.transform.localPosition = new Vector3((float)cell.CanvasX, (float)cell.CanvasY, -1);

            BackwallCell backwallCellComponent = cellInstance.GetComponent<BackwallCell>();
            backwallCellComponent.SetCellModel(cell);

            Building buildingComponent = cellInstance.GetComponent<Building>();
            buildingComponent.SetDefaultView();

            cellInstance.GetComponent<BoxCollider2D>().enabled = true;

            cells.Add(cellInstance);
        }
    }

}
