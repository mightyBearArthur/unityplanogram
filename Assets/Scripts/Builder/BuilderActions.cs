using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BuilderActions : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{

    public static BuilderActions instance = null;

    private List<BuilderMatrixCell> buildingsMatrix;

    private bool followPointer = true;

    private Vector3 dragInitialPoint;

    private Vector2 initialMatrix;

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
            Builder.instance.StopBuild();
        }
    }

    public void OnBeginDrag(PointerEventData data)
    {
        followPointer = false;
        Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        dragInitialPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition) /*- (Builder.instance.building.transform.localScale / 2)*/;
        initialMatrix = new Vector2(1, 1);
    }

    public void OnDrag(PointerEventData data)
    {
        Vector3 currentPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distX = Vector3.Distance(dragInitialPoint, new Vector3(currentPoint.x, dragInitialPoint.y, currentPoint.z));
        float distY = Vector3.Distance(dragInitialPoint, new Vector3(dragInitialPoint.x, currentPoint.y, currentPoint.z));

        Vector2 boxSize = Builder.instance.building.transform.localScale;
        var currentMatrix = new Vector2(Mathf.Ceil(distX / boxSize.x), Mathf.Ceil(distY / boxSize.y));

        if (currentMatrix == initialMatrix) return;
        
        Vector3 vectorDirection = currentPoint - dragInitialPoint;

        Vector2 deltaSize = currentMatrix - initialMatrix;
        deltaSize.Scale(boxSize);
        Builder.instance.buildingCollider.transform.localScale += (Vector3)deltaSize;

        var items = new List<BuilderMatrixCell>();

        for (int r = 0; r < currentMatrix.y; r++)
        {
            for (int c = 0; c < currentMatrix.x; c++)
            {
                var matrixX = c * (int)Mathf.Sign(vectorDirection.x);
                var matrixY = r * (int)Mathf.Sign(vectorDirection.y);

                BuilderMatrixCell existingBox = buildingsMatrix.Find((item) => item.x == matrixX && item.y == matrixY);

                if (existingBox == null)
                {
                    items.Add(new BuilderMatrixCell()
                    {
                        x = matrixX,
                        y = matrixY,
                        gameObject = Instantiate(
                            Builder.instance.building,
                            GetBoxPosition(matrixX, matrixY),
                            Quaternion.identity,
                            Builder.instance.buildingsContainer.transform
                        )
                    });
                }
                else
                {
                    items.Add(existingBox);
                }
            }
        }

        foreach(BuilderMatrixCell obj in buildingsMatrix.Except(items))
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

}
