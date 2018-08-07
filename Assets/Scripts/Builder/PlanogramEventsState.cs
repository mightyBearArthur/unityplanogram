using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlanogramEventsState : MonoBehaviour
{

    public static PlanogramEventsState instance = null;

    private Stack<PlanogramEvents> stack;

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
        stack = new Stack<PlanogramEvents>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Enable<T>(GameObject obj) where T : PlanogramEventsBase<T>, new()
    {
        var last = stack.LastOrDefault();

        if (last != null)
        {
            last.Pause();
            last.gameObject.SetActive(false);
        }

        obj.SetActive(true);
        var events = obj.GetComponent<T>();
        stack.Push(events);
        RectTransform objTransform = obj.GetComponent<RectTransform>();
        objTransform.localPosition = new Vector3(objTransform.localPosition.x, objTransform.localPosition.y, stack.Count);
    }

    public void Disable()
    {
        if (stack.Count == 0) return;

        var events = stack.Pop();
        events.Unbind();
        events.gameObject.SetActive(false);

        var last = stack.LastOrDefault();

        if (last != null)
        {
            last.gameObject.SetActive(true);
            last.Unpause();
        }
    }

}
