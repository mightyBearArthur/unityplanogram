using UnityEngine;
using System.Collections;


public class GUI : MonoBehaviour
{


    public GameObject buildMenu = null;

    public GameObject fillMenu = null;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SwitchSate()
    {
        GameManager.instance.isBuild = !GameManager.instance.isBuild;

        if (GameManager.instance.isBuild)
        {
            buildMenu.SetActive(true);
            fillMenu.SetActive(false);
        }
        else
        {

            buildMenu.SetActive(false);
            fillMenu.SetActive(true);
        }
    }

}
