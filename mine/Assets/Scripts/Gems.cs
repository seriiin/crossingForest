using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gems : MonoBehaviour
{
    public int value;
    void Update()
    {
        transform.Rotate(Vector3.up * 30 * Time.deltaTime, Space.World);
    }
}
