using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
public class Platform : MonoBehaviour
{

    public static Platform instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        //boxSize = GetComponent<MeshRenderer>().material.mainTextureScale / 
    }

    // Update is called once per frame
    void Update()
    {

    }
    
}
