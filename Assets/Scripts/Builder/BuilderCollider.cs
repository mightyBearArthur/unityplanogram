using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class BuilderCollider : MonoBehaviour
{

    public delegate void ValidState(bool state);

    public event ValidState OnValidStateEvent;

    private int _collisionCount = 0;
    private int collisionCount {
        get
        {
            return _collisionCount;
        }

        set
        {
            bool prev = _collisionCount == 0;
            _collisionCount = value;

            if (prev != isValid && OnValidStateEvent != null)
                OnValidStateEvent(isValid);
        }
    }

    public bool isValid { get { return collisionCount == 0; } } 

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        collisionCount++;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        collisionCount--;
    }

    public void ResetState()
    {
        collisionCount = 0;
    }

}
