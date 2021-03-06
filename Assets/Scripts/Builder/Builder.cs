﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Builder : PlanogramEventsBase<Builder>, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{

    /// <summary>
    /// Object that contains all buildings.
    /// </summary>
    public GameObject buildingsContainer = null;

    /// <summary>
    /// Current building prefab.
    /// </summary>
    private GameObject buildingPrefab = null;

    /// <summary>
    /// Working instance of current building prefab.
    /// </summary>
    private GameObject building = null;

    /// <summary>
    /// Prefab for instantiating building collider.
    /// </summary>
    public GameObject buildingColliderPrefab = null;

    /// <summary>
    /// Object that contais collider for validation.
    /// </summary>
    private GameObject buildingCollider = null;

    private bool buildStarted = false;

    private bool draggin = false;

    public GameObject popup = null;

    /// <summary>
    /// Enable building actions and events overlay.
    /// </summary>
    public void Build(GameObject prefab)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (buildStarted)
        {
            if (prefab == buildingPrefab)
                return;

            Destroy(building);
        }
        else
        {
            // Prepare building collider.
            buildingCollider = Instantiate(
                buildingColliderPrefab,
                pos,
                Quaternion.identity,
                buildingsContainer.transform
            );

            // Add event on validation state.
            buildingCollider.GetComponent<BuilderCollider>()
                .OnValidStateEvent += SetValidView;

            // Enable overlay.
            PlanogramEventsState.instance.Disable();
            PlanogramEventsState.instance.Enable<Builder>(gameObject);

            buildStarted = true;
        }

        buildingPrefab = prefab;

        building = Instantiate(
            prefab,
            pos,
            Quaternion.identity,
            buildingsContainer.transform
        );

        // Set position at building.
        buildingCollider.transform.localScale = building.transform.localScale;
        buildingCollider.transform.position = building.transform.position;

        preparedForBuild.Add(building);
    }


    public override void Unbind()
    {
        StopBuild();
    }

    /// <summary>
    /// Disable building actions and events overlay.
    /// </summary>
    public void StopBuild()
    {
        buildStarted = false;
        PlanogramEventsState.instance.Disable();
        // Add event on validation state.
        buildingCollider.GetComponent<BuilderCollider>()
            .OnValidStateEvent -= SetValidView;

        Destroy(building);
        Destroy(buildingCollider);
        buildingsMatrix.Clear();
        preparedForBuild.Clear();
    }

    private List<BuilderMatrixCell> buildingsMatrix;

    private bool followPointer = true;

    private Vector3 mouseInitialPoint;

    private Vector3 dragInitialPoint;

    private Vector3 vectorDirection;

    private Vector2 initialMatrix;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        buildingsMatrix = new List<BuilderMatrixCell>();
        preparedForBuild = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (followPointer)
        {
            MoveBuilding();
        }
    }

    private List<GameObject> preparedForBuild;

    private void SetValidView(bool state)
    {
        foreach (GameObject obj in preparedForBuild)
        {
            obj.GetComponent<Building>()
                .SetValidView(state);
        }
    }

    public void OnBeginDrag(PointerEventData data)
    {
        if (data.button != PointerEventData.InputButton.Left) return;

        followPointer = false;
        mouseInitialPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        buildingCollider.transform.localScale = Vector3.zero;
        initialMatrix = new Vector2(0, 0);
        building.SetActive(false);
        draggin = true;
        preparedForBuild.Clear();
    }

    public void OnDrag(PointerEventData data)
    {
        if (data.button != PointerEventData.InputButton.Left) return;

        Vector3 currentPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 currentVectorDirection = currentPoint - mouseInitialPoint;

        if (vectorDirection == null || vectorDirection != currentVectorDirection)
        {
            vectorDirection = new Vector3(
                Mathf.Sign(currentVectorDirection.x),
                Mathf.Sign(currentVectorDirection.y),
                Mathf.Sign(currentVectorDirection.z)
            );

            Vector3 position = building.transform.localScale;
            position.Scale(vectorDirection);

            dragInitialPoint = building.transform.position - (position / 2);
        }

        float distX = Vector3.Distance(dragInitialPoint, new Vector3(currentPoint.x, dragInitialPoint.y, dragInitialPoint.z));
        float distY = Vector3.Distance(dragInitialPoint, new Vector3(dragInitialPoint.x, currentPoint.y, dragInitialPoint.z));

        Vector2 boxSize = building.transform.localScale;
        var currentMatrix = new Vector2(Mathf.Ceil(distX / boxSize.x), Mathf.Ceil(distY / boxSize.y));
        if (currentMatrix == initialMatrix) return;

        Vector2 deltaSize = currentMatrix - initialMatrix;
        deltaSize.Scale(boxSize);
        buildingCollider.transform.localScale += (Vector3)deltaSize;

        Vector2 step = new Vector2(currentMatrix.x * boxSize.x / 2, currentMatrix.y * boxSize.y / 2);
        step.Scale(vectorDirection);
        buildingCollider.transform.position = (Vector2)dragInitialPoint + step;

        var items = new List<BuilderMatrixCell>();

        bool status = buildingCollider.GetComponent<BuilderCollider>().isValid;

        for (int r = 0; r < currentMatrix.y; r++)
        {
            for (int c = 0; c < currentMatrix.x; c++)
            {
                int matrixX = c * (int)vectorDirection.x;
                int matrixY = r * (int)vectorDirection.y;

                BuilderMatrixCell existingBox = buildingsMatrix.Find((item) => item.x == matrixX && item.y == matrixY);

                if (existingBox == null)
                {
                    GameObject newObj = Instantiate(
                        buildingPrefab,
                        GetBoxPosition(matrixX, matrixY),
                        Quaternion.identity,
                        buildingsContainer.transform
                    );

                    newObj.GetComponent<Building>().SetValidView(status);

                    items.Add(new BuilderMatrixCell()
                    {
                        x = matrixX,
                        y = matrixY,
                        gameObject = newObj
                    });
                }
                else
                {
                    items.Add(existingBox);
                }
            }
        }

        foreach (BuilderMatrixCell obj in buildingsMatrix.Except(items))
        {
            Destroy(obj.gameObject);
        }

        buildingsMatrix = items;

        preparedForBuild = items.Select(x => x.gameObject).ToList();

        initialMatrix = currentMatrix;
    }

    /// <summary>
    /// Made some after built setups.
    /// </summary>
    /// <param name="obj"></param>
    private void FinalizeBuilding(GameObject obj, Building.CellPosition pos)
    {
        Building buildingHelper = obj.GetComponent<Building>();
        obj.GetComponent<BoxCollider2D>().enabled = true;
        buildingHelper.SetDefaultView();
        buildingHelper.SetPosition(pos);
        obj.transform.position = new Vector3(
            obj.transform.position.x,
            obj.transform.position.y,
            -0.9999f
        );
    }

    /// <summary>
    /// Place a single building instatiating from existing.
    /// </summary>
    private void PlaceBuilding(Building.CellPosition pos)
    {
        if (!buildingCollider.GetComponent<BuilderCollider>().isValid) return;

        FinalizeBuilding(Instantiate(building, buildingsContainer.transform), pos);
        CleanActionState();
        followPointer = true;
    }

    private void CancelBuilding()
    {
        CleanActionState();
        followPointer = true;
    }

    /// <summary>
    /// Place building from matrix.
    /// </summary>
    private void PlaceMatrixBuildings(Building.CellPosition pos)
    {
        // If building is not valid destroy all prepared instances.
        foreach (GameObject obj in preparedForBuild)
        {
            if (!buildingCollider.GetComponent<BuilderCollider>().isValid)
            {
                Destroy(obj);
            }
            else
            {
                BuilderMatrixCell matrixPos = buildingsMatrix.Find(x => x.gameObject == obj);
                Building.CellPosition cellPos = new Building.CellPosition()
                {
                    row = Mathf.Abs(matrixPos.y) + pos.row,
                    col = Mathf.Abs(matrixPos.x) + pos.col
                };

                FinalizeBuilding(obj, cellPos);
            }
        }

        CleanActionState();
    }

    private void CancelMatrixBuildings()
    {
        foreach (GameObject obj in preparedForBuild)
        {
            Destroy(obj);
        }

        CleanActionState();
    }

    private void CleanActionState()
    {
        buildingsMatrix.Clear();
        preparedForBuild.Clear();
        preparedForBuild.Add(building);
        followPointer = true;
        building.SetActive(true);
    }

    public void OnEndDrag(PointerEventData data)
    {
        if (data.button != PointerEventData.InputButton.Left) return;

        buildingCollider.transform.localScale = building.transform.localScale;
        buildingCollider.transform.position = building.transform.position;
        draggin = false;

        GameObject popupInstance = Popup.instance.Open(popup);
        popupInstance.GetComponent<CellPositionPopup>().OnSubmitEvent += PlaceMatrixBuildings;
        popupInstance.GetComponent<CellPositionPopup>().OnCancelEvent += CancelMatrixBuildings;
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (draggin) return;

        //if (data.button == PointerEventData.InputButton.Right)
        //{
        //    StopBuild();
        //}

        if (data.button == PointerEventData.InputButton.Left)
        {
            followPointer = false;
            GameObject popupInstance = Popup.instance.Open(popup);
            popupInstance.GetComponent<CellPositionPopup>().OnSubmitEvent += PlaceBuilding;
            popupInstance.GetComponent<CellPositionPopup>().OnCancelEvent += CancelBuilding;
        }
    }

    /// <summary>
    /// Make building follow pointer.
    /// </summary>
    private void MoveBuilding()
    {
        Vector3 cell = GridSystem.GetCellPosAtPoint();
        building.transform.position = new Vector3(cell.x, cell.y, -1f);
        buildingCollider.transform.position = new Vector3(cell.x, cell.y, -1f);
    }

    private bool MatrixContains(float x, float y, Vector2 matrix)
    {
        return AxisPointInRange(x, matrix.x) && AxisPointInRange(y, matrix.y);
    }

    private bool AxisPointInRange(float point, float range)
    {
        if (range == 0) return false;

        float min = Mathf.Min(0, range);
        float max = Mathf.Max(0, range);

        return (point <= max && point >= min);
    }

    private Vector3 GetBoxPosition(int x, int y)
    {
        Vector2 boxSize = Builder.instance.building.transform.localScale;
        return new Vector3(
            building.transform.position.x + x * boxSize.x,
            building.transform.position.y + y * boxSize.y,
            -1f
        );
    }

    public override void Pause()
    {
        building.SetActive(false);
    }

    public override void Unpause()
    {
        building.SetActive(true);
    }

}
