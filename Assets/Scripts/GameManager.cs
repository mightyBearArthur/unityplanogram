using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public Vector2 GridSize { get; private set; }

    public int GridScale;

    public bool isBuild = true;

    public static GameManager instance = null;

    public GameObject platform;

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

    public void Start()
    {
        Vector2 materialScale = platform.GetComponent<MeshRenderer>().material.mainTextureScale;
        Vector2 platformSize = platform.transform.localScale;
        GridSize = new Vector2(1, 1) / (materialScale / platformSize) / 2;
    }

}
