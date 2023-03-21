using UnityEngine;

/// <summary>
/// Disk is a model containing information that may vary from each disk instance.
/// </summary>
public class Disk : MonoBehaviour
{
    /// <summary>
    /// _value is a virtual value to help checking certain game rules; you cannot put a disk over another disk with a lower value.
    /// </summary>
    public int Value;

    private Transform Transform;
    private void Start()
    {
        Transform = this.GetComponent<Transform>();
    }

}
