using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider backWheel;
    public WheelCollider frontWheel;
    public GameObject actualFrontWheels;
    public GameObject actualBackWheels;
    public bool motor = true;
    public bool steering = true;

}
public class MoveObjectScript : MonoBehaviour
{
    //public float moveSpeed = 1f;
    //public float rotationSpeed = 1f;
    //private bool isOnGround = false;
    private AxleInfo axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    private float motor;
    private float steering;
    private Vector2 is2dValue;

    private void Awake()
    {
        FindWeels();
    }

    public void FixedUpdate()
    {
        InputSystem.onActionChange +=
            (obj , change) =>
            {
                switch (change)
                {
                    case InputActionChange.ActionStarted:
                       
                        if (( ( InputAction )obj ).name == "MCart")
                        {
                            Debug.Log( $"action is performed  { ( ( InputAction )obj ).ReadValue<Vector2>()} {change}" );
                            MoveWheels( ( ( InputAction )obj ).ReadValue<Vector2>() );
                            break;
                        }
                        break;

                    case InputActionChange.ActionPerformed:         
                    case InputActionChange.ActionCanceled:
                        break;
 
                }
            };
    }

    private void ApplyLocalPositionToVisuals(AxleInfo axInfo)
    {
        float originalBWRX = axInfo.actualBackWheels.transform.localEulerAngles.x;
        float originalBWRZ = axInfo.actualBackWheels.transform.localEulerAngles.z;
        float originalFWRX = axInfo.actualFrontWheels.transform.localEulerAngles.x;
        float originalFWRZ = axInfo.actualFrontWheels.transform.localEulerAngles.z;
        //Vector3 position2;
        //Quaternion rotation2;
        transform.localEulerAngles = new Vector3( transform.localEulerAngles.x , axInfo.backWheel.steerAngle - transform.localEulerAngles.z , transform.localEulerAngles.z );
        axInfo.actualBackWheels.transform.rotation = Quaternion.Euler( originalBWRX , axInfo.backWheel.steerAngle , originalBWRZ );
        axInfo.actualFrontWheels.transform.rotation = Quaternion.Euler( originalFWRX , axInfo.frontWheel.steerAngle , originalFWRZ );
        transform.localPosition = new Vector3( axInfo.backWheel.motorTorque , transform.localPosition.y , transform.localPosition.z );
        //axInfo.frontWheel.GetWorldPose( out position2 , out rotation2 );
        //axInfo.actualFrontWheels.transform.position = position2;
        //axInfo.actualFrontWheels.transform.rotation = rotation2;
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
                
                    if (axleInfos.steering)
                    {
                    axleInfos.backWheel.steerAngle = steering;
                    axleInfos.frontWheel.steerAngle = steering;
                    }
                    ApplyLocalPositionToVisuals( axleInfos );
            }
            if (dirValueY == 1.00 || dirValueY == -1.00)
            {
                motor = maxMotorTorque * dirValueY;
                    if (axleInfos.motor)
                    {
                        axleInfos.backWheel.motorTorque = motor;
                        axleInfos.frontWheel.motorTorque = motor;
                    }
                    ApplyLocalPositionToVisuals( axleInfos );
                }
        }
    }
    private void FindWeels()
    {
        axleInfos = new AxleInfo();
        GameObject searchChild;
        GameObject originalCart = GameObject.Find( "cart4w" );

        for (int i = 0; i < originalCart.transform.childCount; i++)
        {
             searchChild = originalCart.transform.GetChild( i ).gameObject;
             if(searchChild.name == "Wheels")
            { 
                axleInfos.actualFrontWheels = searchChild.transform.Find( "frontWheels" ).gameObject;
                axleInfos.actualBackWheels = searchChild.transform.Find( "backWheels" ).gameObject;  
            }
            if (searchChild.name == "Colliders")
            {      
                axleInfos.backWheel = searchChild.transform.Find( "backCollider" ).GetComponent<WheelCollider>();
                axleInfos.frontWheel = searchChild.transform.Find( "frontCollider" ).GetComponent<WheelCollider>();
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
