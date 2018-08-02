using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public Vector2 GridSize;

    public int GridScale;

    public bool isBuild = true;

    public static GameManager instance = null;

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

        DontDestroyOnLoad(gameObject);
    }



}
