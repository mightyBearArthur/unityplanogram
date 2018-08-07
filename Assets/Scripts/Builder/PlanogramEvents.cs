using UnityEngine;
using System.Collections;

public abstract class PlanogramEvents : MonoBehaviour
{
    public abstract void Pause();
    public abstract void Unpause();
    public abstract void Unbind();
}
