using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

[Serializable]
public class BackwallTemplateParamModel
{

    public string name;

    [SerializeField]
    public List<List<BackwallCellParam>> cells;

}
