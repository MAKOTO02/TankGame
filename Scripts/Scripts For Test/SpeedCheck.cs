using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpeedCheck : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Debug.Log("���x: " + GetComponent<Rigidbody>().velocity.magnitude.ToString() + "�@����: " + Time.time.ToString());
    }
}
