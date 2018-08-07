using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class PlanogramEventsBase<T> : PlanogramEvents
    where T: PlanogramEventsBase<T>, new()
{

    public static T instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = (T)this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public virtual void Start()
    {
        gameObject.SetActive(false);
    }

    public override void Pause()
    {
        
    }

    public override void Unpause()
    {

    }

    //public abstract void Bind();

    public override void Unbind()
    {

    }

}
