using UnityEditor.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BaseCannonController : MonoBehaviour
{
    [SerializeField] private Rigidbody Cannon;
    protected Vector3 CannonForard;
    protected Vector3 AimDirection;
    public float turnSpeed = 1.0f;
    // Start is called before the first frame update
    protected virtual void Start()
    {        
        Cannon.angularDrag = 5.0f;
        Cannon.useGravity = false;
        CannonForard = -Cannon.transform.up;
        AimDirection = CannonForard;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CannonForard = -Cannon.transform.up;
    }

    protected  virtual void TurnCannon()
    {
        Vector3 torque = Vector3.Cross(CannonForard.normalized, AimDirection.normalized) * turnSpeed;
        Cannon.AddTorque(torque);
    }
}
