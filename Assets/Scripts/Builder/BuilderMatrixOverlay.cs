using UnityEngine;
using System.Collections;

public class BuilderMatrixOverlay : MonoBehaviour
{

    public bool isValid { get; private set; }

    // Use this for initialization
    void Start()
    {
        isValid = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isValid = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isValid = true;
    }

}
