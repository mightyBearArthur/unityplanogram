using UnityEngine;
using System.Collections;

public abstract class ManagerBase : MonoBehaviour {

    public static ManagerBase instance = null;

    public void Awake()
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
