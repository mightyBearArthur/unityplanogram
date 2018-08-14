using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BackwallCell : MonoBehaviour
{

    public GameObject rowText;

    public GameObject colText;

    public BackwallCellViewModel cellModel { get; private set; }

    public void SetCellModel(BackwallCellViewModel cell)
    {
        cellModel = cell;

        rowText.GetComponent<TextMesh>().text = cellModel.Row.ToString();
        colText.GetComponent<TextMesh>().text = cellModel.Column.ToString();
    }

}
