using UnityEditor.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BaseCannonController : MonoBehaviour
{
    protected Vector3 CannonForard;
    protected Vector3 AimDirection;
    public float turnSpeed = 1.0f;
    // Start is called before the first frame update
    protected virtual void Start()
    {        
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.angularDrag = 5.0f;
        rb.useGravity = false;
        CannonForard = -gameObject.GetComponent<Rigidbody>().transform.up;
        AimDirection = CannonForard;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CannonForard = -gameObject.GetComponent<Rigidbody>().transform.up;
    }

    protected  virtual void TurnCannon()
    {
        Vector3 torque = Vector3.Cross(CannonForard.normalized, AimDirection.normalized) * turnSpeed;
        gameObject.GetComponent<Rigidbody>().AddTorque(torque);
    }
}
