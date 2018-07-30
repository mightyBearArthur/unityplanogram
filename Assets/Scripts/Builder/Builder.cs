using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Builder : PlanogramEventsBase<Builder>, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    
    public GameObject buildingsContainer = null;

    public GameObject buildingColliderPrefab = null;

    public GameObject buildingCollider { get; private set; }

    public GameObject building { get; private set; }

    public Vector3 movementShift { get; private set; }

    private bool buildStarted = false;
    
    /// <summary>
    /// Validate buildings.
    /// </summary>
    /// <param name="buildings"></param>
    /// <returns></returns>
    private bool Validate(List<GameObject> buildings)
    {
        foreach (GameObject obj in buildings)
        {
            if (!obj.GetComponent<Building>().isValid) return false;
        }

        return true;
    }

    /// <summary>
    /// Enable building actions and events overlay.
    /// </summary>
    public void Build(GameObject prefab)
    {
        if (building != null)
        {
            if (building == prefab) return;
            Destroy(building);
        }
        else
        {
            PlanogramEventsState.instance.Enable<Builder>(gameObject);
        }

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        building = Instantiate(
            prefab,
            pos, 
            Quaternion.identity, 
            buildingsContainer.transform
        );

        if (!buildStarted)
        {
            buildingCollider = Instantiate(
                buildingColliderPrefab,
                pos,
                Quaternion.identity,
                buildingsContainer.transform
            );
            buildingCollider.transform.localScale = building.transform.localScale;
            buildStarted = true;
        }
    }

    public void PlaceBuilding()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        building = Instantiate(
            building,
            pos,
            Quaternion.identity,
            buildingsContainer.transform
        );
    }

    /// <summary>
    /// Disable building actions and events overlay.
    /// </summary>
    public void StopBuild()
    {
        buildStarted = false;
        PlanogramEventsState.instance.Disable();
        Destroy(building);
        Destroy(buildingCollider);
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
    }

    // Update is called once per frame
    void Update()
    {
        if (followPointer)
        {
            MoveBuilding();
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            StopBuild();
        }
    }

    public void OnBeginDrag(PointerEventData data)
    {
        followPointer = false;
        mouseInitialPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        initialMatrix = new Vector2(1, 1);
    }

    public void OnDrag(PointerEventData data)
    {
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

        var items = new List<BuilderMatrixCell>();

        for (int r = 0; r < currentMatrix.y; r++)
        {
            for (int c = 0; c < currentMatrix.x; c++)
            {
                int matrixX = c * (int)vectorDirection.x;
                int matrixY = r * (int)vectorDirection.y;

                BuilderMatrixCell existingBox = buildingsMatrix.Find((item) => item.x == matrixX && item.y == matrixY);

                if (existingBox == null)
                {
                    items.Add(new BuilderMatrixCell()
                    {
                        x = matrixX,
                        y = matrixY,
                        gameObject = Instantiate(
                            building,
                            GetBoxPosition(matrixX, matrixY),
                            Quaternion.identity,
                            buildingsContainer.transform
                        )
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

        initialMatrix = currentMatrix;
    }

    public void OnEndDrag(PointerEventData data)
    {
        Builder.instance.buildingCollider.transform.localScale = Builder.instance.building.transform.localScale;
        buildingsMatrix.Clear();
        followPointer = true;
        buildingsMatrix.Add(new BuilderMatrixCell()
        {
            x = 1,
            y = 1,
            gameObject = Instantiate(Builder.instance.building, Builder.instance.buildingsContainer.transform)
        });
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Right)
        {
            Builder.instance.StopBuild();
        }

        if (data.button == PointerEventData.InputButton.Left)
        {
            Builder.instance.PlaceBuilding();
        }
    }

    /// <summary>
    /// Make building follow pointer.
    /// </summary>
    private void MoveBuilding()
    {
        Vector3 cell = GridSystem.GetCellPosAtPoint();
        Builder.instance.building.transform.position = new Vector3(cell.x, cell.y, -1f);
        Builder.instance.buildingCollider.transform.position = new Vector3(cell.x, cell.y, -1f);
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
            Builder.instance.building.transform.position.x + x * boxSize.x,
            Builder.instance.building.transform.position.y + y * boxSize.y
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
