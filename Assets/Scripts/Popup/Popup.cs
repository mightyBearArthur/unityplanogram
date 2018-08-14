using UnityEngine;
using System.Collections;

public class Popup : MonoBehaviour
{

    public static Popup instance = null;

    private GameObject popup = null;

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

    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject Open(GameObject prefab)
    {
        if (popup != null) return null;
        popup = Instantiate(prefab, transform);
        return popup;
    }

    public void Close()
    {
        if (popup == null) return;
        Destroy(popup);
    }

}
