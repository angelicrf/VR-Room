using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    //public float moveSpeed = 1f;
    //public float rotationSpeed = 1f;
    //private bool isOnGround = false;
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    private float motor;
    private float steering;
    private Vector2 is2dValue;

    public void FixedUpdate()
    {
        // MoveWheels();
        InputSystem.onActionChange +=
            (obj , change) =>
            {
                switch (change)
                {
                    case InputActionChange.ActionStarted:
                        if (( ( InputAction )obj ).name == "MCart")
                        {
                            Debug.Log( $"action is started  { ( ( InputAction )obj ).ReadValue<Vector2>()} {change}" );
                        }
                        break;

                    case InputActionChange.ActionPerformed:
                        if (( ( InputAction )obj ).name == "MCart")
                        {
                            Debug.Log( $"action is performed  { ( ( InputAction )obj ).ReadValue<Vector2>()} {change}" );
                            MoveWheels( ( ( InputAction )obj ).ReadValue<Vector2>() );
                        }
                        break;
                    case InputActionChange.ActionCanceled:
                        if (( ( InputAction )obj ).name == "MCart")
                        {
                            Debug.Log( $"{( ( InputAction )obj ).ReadValue<Vector2>()} {change}" );
                        }
                        break;
                }
            };

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
        Debug.Log( "positionWheel " + position );

        visualWheel.transform.position = position;
        //collider.transform.parent.position;
        //TransformPoint( position );
        Debug.Log( "positionRotation " + rotation );

        visualWheel.transform.rotation = rotation;
        //collider.transform.parent.rotation * rotation;
    }
    private void FindInputValueOculusDevice()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices( inputDevices );
        foreach (var device in inputDevices)
        {
            Debug.Log( "devices '{0}'" );

            if (device.TryGetFeatureValue( UnityEngine.XR.CommonUsages.primary2DAxis , out is2dValue ))
            {
                Debug.Log( "2dValue  is pressed" + is2dValue );
            }
        }
    }
    private void MoveWheels(Vector2 dirValue)
    {
        // position += dirValue * moveSpeed * Time.deltaTime;
        float dirValueX = dirValue.x;
        float dirValueY = dirValue.y; 

        if (dirValueX == 1.00 || dirValueX == -1.00 || dirValueY == 1.00 || dirValueY == -1.00)
        {
            if (dirValueX == 1.00 || dirValueX == -1.00)
            {
                steering = maxSteeringAngle * dirValueX;
                Debug.Log( "insideHorizantal" + steering );
            }
            if (dirValueY == 1.00 || dirValueY == -1.00)
            {
                motor = maxMotorTorque * dirValueY;
                Debug.Log( "insideVertical"  + motor);
            }
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
