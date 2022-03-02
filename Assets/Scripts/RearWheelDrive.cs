using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class RearWheelDrive : MonoBehaviour
{
    [System.Serializable]
    public struct WheelInfo
    {
        public Transform visualwheel;
        public WheelCollider wheelcollider;
    }

    public float motor;
    public float steer;
    private float brake { get; set; }
    public WheelInfo FL;
    public WheelInfo FR;
    public WheelInfo BL;
    public WheelInfo BR;
    public float speed;
    private bool isChanged = false;
    void Awake()
    {
        GetTheWheels();
    }
    void FixedUpdate()
    {
     FindInputAction();
    }
    private void FindInputAction()
    {      
        InputSystem.onActionChange +=
        (obj , change) =>
        {
            //A/S - to to turn or W/D to move [keyboard]
            if (change == InputActionChange.ActionStarted)
            {
                if (( ( InputAction )obj ).name == "MCart" && ( ( InputAction )obj ).name != "MBrake")
                {
                    Debug.Log( $"MCart action is performed  { ( ( InputAction )obj ).ReadValue<Vector2>()} {change} brake is {brake}" );
                    if (brake != 0)
                    {
                        brake = 0;
                        TakeBrake();
                    }
                    MoveWheels( ( ( InputAction )obj ).ReadValue<Vector2>() );                 
                }
                // Space[keyboard] to stop
                else if (( ( InputAction )obj ).name == "MBrake" && ( ( InputAction )obj ).name != "MCart")
                {
                    Debug.Log( $"MBrake action is performed  { ( ( InputAction )obj ).ReadValue<float>()} {change} brake is {brake}" );
                    brake = 440f;
                    TakeBrake();           
                }
            }
        };
    }
    private void TakeBrake()
    {
        FL.wheelcollider.brakeTorque = brake;
        FR.wheelcollider.brakeTorque = brake;
        BL.wheelcollider.brakeTorque = brake;
        BR.wheelcollider.brakeTorque = brake;
    }
    private void MoveWheels(Vector2 dirValue)
    {
        float dirValueX = dirValue.x;
        float dirValueY = dirValue.y;

        if (dirValueX == 1.00 || dirValueX == -1.00 || dirValueY == 1.00 || dirValueY == -1.00 || dirValueX == 0.00 || dirValueY == 0.00)
        {
            if (this.gameObject.name == "Car")
            {

                FL.wheelcollider.steerAngle = dirValueX * steer;
                FR.wheelcollider.steerAngle = dirValueX * steer;
                BL.wheelcollider.steerAngle = dirValueX * steer;
                BR.wheelcollider.steerAngle = dirValueX * steer;
                //after each turn [A or D] for a better speed it is best to push W [forward] or S[backward]
                BL.wheelcollider.motorTorque = dirValueY * motor;
                BR.wheelcollider.motorTorque = dirValueY * motor;
                FL.wheelcollider.motorTorque = Mathf.Abs(dirValueX * motor);
                FR.wheelcollider.motorTorque = dirValueX * motor;
                UpdateVisualWheels( FL , FL.visualwheel );
                UpdateVisualWheels( BL , BL.visualwheel );
                UpdateVisualWheels( FR , FR.visualwheel );
                UpdateVisualWheels( BR , BR.visualwheel );
                // yield return new WaitUntil( () => isChanged );
                // ChangeCartPos( dirValueX , dirValueY );
            }        
        }
    }
    private void UpdateVisualWheels(WheelInfo whI,Transform tr)
    {
        Vector3 pos = tr.position;
        Quaternion rot = tr.rotation;

        whI.wheelcollider.GetWorldPose( out pos , out rot );
        whI.visualwheel.SetPositionAndRotation( pos , rot );     
    }
    private void ChangeCartPos(float dirX, float dirY) {
      Rigidbody rd = transform.GetComponent<Rigidbody>();
        //alternative move for a shopping cart
        if (rd.useGravity)
        {
            //rd.AddForce( Vector3.forward, ForceMode.Impulse);
            //move twards the camera
            //Camera.main.transform
            if (dirX == 1)
            {
                //S      
                transform.Translate( Vector3.right * Time.deltaTime * speed );
                isChanged = false;
            }
            if(dirX == -1)
            {
                //A        
                transform.Translate( Vector3.left * Time.deltaTime * speed );
                isChanged = false;
            }
            if (dirY == 1)
            {            
                //W
                transform.Translate( Vector3.forward * Time.deltaTime * speed );
                isChanged = false;

            }
            if (dirY == -1)
            {
                //D
                transform.Translate( Vector3.back * Time.deltaTime * speed );
                isChanged = false;
            }

        }
        else
        {
            rd.useGravity = true;
        }
    }
    private void GetTheWheels()
    {
        GameObject wheels = GetChildByName(this.gameObject, "Wheels");
        if (this.gameObject.name == "Car")
        {
            FL.visualwheel = GetChildByName( wheels , "FL" ).transform;
            FR.visualwheel = GetChildByName( wheels , "FR" ).transform;
            BL.visualwheel = GetChildByName( wheels , "BL" ).transform;
            BR.visualwheel = GetChildByName( wheels , "BR" ).transform;

        }
        GameObject colliders = GetChildByName(this.gameObject, "Colliders");
        if (this.gameObject.name == "Car")
        {
            FL.wheelcollider = GetChildByName( colliders , "wcFL" ).GetComponent<WheelCollider>();
            BR.wheelcollider = GetChildByName( colliders , "wcBR" ).GetComponent<WheelCollider>();
            FR.wheelcollider = GetChildByName( colliders , "wcFR" ).GetComponent<WheelCollider>();
            BL.wheelcollider = GetChildByName( colliders , "wcBL" ).GetComponent<WheelCollider>();
        }
    }
    private GameObject GetChildByName(GameObject go, string name)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            if (go.transform.GetChild(i).name == name) 
            {
                return go.transform.GetChild(i).gameObject;
            }
        }
        Debug.LogError("ERR: Could not find child gameobject " + name + ". Check spelling and case.");
        return null;
    }
}
