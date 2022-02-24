using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}
public class MoveObjectScript : MonoBehaviour
{
    [SerializeField] private XRDeviceSimulatorControls xrDeviceSimulatorControls;
    [SerializeField] private GameObject xrDeviceSimulator;
    //public float moveSpeed = 1f;
    //public float rotationSpeed = 1f;
    //private bool isOnGround = false;
    //
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    private float motor;
    private float steering;

    void Awake()
    {
        if (xrDeviceSimulator != null && xrDeviceSimulator.activeSelf)
        {
            xrDeviceSimulatorControls = new XRDeviceSimulatorControls();
        }
    }
        private void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }
 
        Transform visualWheel = collider.transform.GetChild( 0 );
        Vector3 position;
        Quaternion rotation;

        collider.GetWorldPose( out position , out rotation );

        visualWheel.transform.position = position;
        //collider.transform.parent.position;
        //TransformPoint( position );
        visualWheel.transform.rotation = rotation;
        //collider.transform.parent.rotation * rotation;
    }
    private void OnDestroy()
    {
        if (xrDeviceSimulator != null && xrDeviceSimulator.activeSelf)
        {
            xrDeviceSimulatorControls.Assistant.Disable();
        }
    }
    private void OnEnable()
    {
        if (xrDeviceSimulator != null && xrDeviceSimulator.activeSelf)
        {
            xrDeviceSimulatorControls.Assistant.Enable();
        }
    }

    public void FixedUpdate()
    {
        MoveWheels();
    }
    private void MoveWheels()
    {
        float verticalValue = xrDeviceSimulatorControls.Cart.MCart.ReadValue<float>();
        float horizantalValue = xrDeviceSimulatorControls.Assistant.LeftMove.ReadValue<float>();
        if (verticalValue == 1)
        {
            motor = maxMotorTorque * verticalValue;
        }
        if (horizantalValue == 1)
        {
            steering = maxSteeringAngle * horizantalValue;
        }

        if (horizantalValue != 0 || verticalValue != 0)
        {
            foreach (AxleInfo axleInfo in axleInfos)
            {
                if (axleInfo.steering)
                {
                    axleInfo.leftWheel.steerAngle = steering;
                    axleInfo.rightWheel.steerAngle = steering;
                }
                if (axleInfo.motor)
                {
                    axleInfo.leftWheel.motorTorque = motor;
                    axleInfo.rightWheel.motorTorque = motor;
                }
                ApplyLocalPositionToVisuals( axleInfo.leftWheel );
                ApplyLocalPositionToVisuals( axleInfo.rightWheel );
            }
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    isOnGround = true;
    //}
    //private void Start()
    //{
    //    MoveObjectDir( Vector3.right );
    //}
    //public void MoveForwardTarget()
    //{
    //    //transform.rotation = Quaternion.Lerp( transform.rotation , Quaternion.identity , rotationSpeed * Time.time );
    //    if (isOnGround)
    //    {
    //        transform.position = Vector3.MoveTowards( transform.position , Vector3.up , moveSpeed * Time.deltaTime );
    //    }
    //}
    //public void MoveBackwardTarget()
    //{
    //    if (isOnGround)
    //    {
    //        transform.position = Vector3.MoveTowards( transform.position , Vector3.down , moveSpeed * Time.deltaTime );
    //    }
    //}
    //public void MoveLeftTarget()
    //{
    //    if (isOnGround)
    //    {
    //        transform.position = Vector3.MoveTowards( transform.position , Vector3.left , moveSpeed * Time.deltaTime );
    //    }
    //}
    //public void MoveRightTarget()
    //{
    //    if (isOnGround)
    //    {
    //        transform.position = Vector3.MoveTowards( transform.position , Vector3.right , moveSpeed * Time.deltaTime );
    //    }
    //}
    //public void MoveObjectDir(Vector3 thisDir)
    //{
    //    if (isOnGround)
    //    {
    //        transform.position = Vector3.MoveTowards( transform.position , thisDir , moveSpeed * Time.deltaTime );
    //    }
    //}

}
