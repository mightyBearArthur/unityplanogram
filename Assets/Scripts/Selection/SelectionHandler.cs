using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionHandler : MonoBehaviour
{

    public static SelectionHandler instance = null;

    public HashSet<GameObject> selected;

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
        selected = new HashSet<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        OnDelete();
    }

    private void OnDelete()
    {
        if (!Input.GetKeyDown(KeyCode.Delete) || selected.Count == 0) return;

        foreach (GameObject obj in selected)
        {
            Destroy(obj);
        }
    }

}
