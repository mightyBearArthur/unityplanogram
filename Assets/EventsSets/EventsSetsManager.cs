using UnityEngine;
using System.Collections;

public class EventsSetsManager : MonoBehaviour
{

    public static EventsSetsManager instance = null;

    // Use this for initialization
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

        DontDestroyOnLoad(instance);
    }

    

}
