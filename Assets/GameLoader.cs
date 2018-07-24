using UnityEngine;
using System.Collections;

public class GameLoader : MonoBehaviour
{

    public GameObject managers[];

    // Use this for initialization
    void Awake()
    {
        if (GameManager.instance == null)
        {
            Instantiate(gameManager);
        }

        foreach (GameObject)
    }



}
