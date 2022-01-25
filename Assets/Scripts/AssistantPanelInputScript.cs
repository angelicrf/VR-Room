using UnityEngine;

public class AssistantPanelInputScript : MonoBehaviour
{
    [SerializeField] private XRDeviceSimulatorControls xrDeviceSimulatorControls;
    [SerializeField] private XRIDefaultInputActions xrDefaultInputAction;
    [SerializeField] private GameObject xrDeviceSimulator;
    [SerializeField] private GameObject target;
    private float upBtnValue;
    private float leftBtnValue;
    private float downBtnValue;
    private float rightBtnValue;
    private float speed = 2;
    void Awake()
    {
        if (xrDeviceSimulator != null && xrDeviceSimulator.activeSelf)
        {
            xrDeviceSimulatorControls = new XRDeviceSimulatorControls();
        }
        else
        {
            xrDefaultInputAction = new XRIDefaultInputActions();
        }
    }
    private void OnDestroy()
    {
        if (xrDeviceSimulator != null && xrDeviceSimulator.activeSelf)
        {
            xrDeviceSimulatorControls.Assistant.Disable();
        }
        else
        {
            xrDefaultInputAction.Assistant.Disable();
        }
    }
    private void OnEnable()
    {
        if (xrDeviceSimulator != null && xrDeviceSimulator.activeSelf)
        {
            xrDeviceSimulatorControls.Assistant.Enable();
        }
        else{
            xrDefaultInputAction.Assistant.Enable();
        }
    }

    public void GetLeftBtnValue()
    {
        Debug.Log( "leftBtnValue" + upBtnValue );
        leftBtnValue = xrDeviceSimulatorControls.Assistant.LeftMove.ReadValue<float>();
        if (leftBtnValue == 0)
        {
            Debug.Log( "leftispressed" );
            target.transform.Translate( Vector3.left * speed * Time.deltaTime );

        }
    }
    public void GetRightBtnValue()
    {
        Debug.Log( "rightBtnValue" + upBtnValue );
        rightBtnValue = xrDeviceSimulatorControls.Assistant.RightMove.ReadValue<float>();
        if (rightBtnValue == 0)
        {
            Debug.Log( "rightispressed" );
            target.transform.Translate( Vector3.right * speed * Time.deltaTime );
        }
    }
    public void GetDownBtnValue()
    {
        Debug.Log( "downBtnValue" + upBtnValue );
        downBtnValue = xrDeviceSimulatorControls.Assistant.DownMove.ReadValue<float>();
        if (downBtnValue == 0)
        {
            Debug.Log( "downtispressed" );
            target.transform.Translate( Vector3.down * speed * Time.deltaTime );
        }
    }
    public void GetUpBtnValue()
    {
       
        upBtnValue = xrDeviceSimulatorControls.Assistant.UpMove.ReadValue<float>();
        if (upBtnValue == 0)
        {         
           Vector2 tempValue = new Vector2(
             Mathf.Round(target.transform.position.x * 10.0f) * 0.1f ,Mathf.Round(target.transform.position.y * 10.0f ) * 0.1f + speed * Time.deltaTime );
            target.transform.position = tempValue;
        }
    }
}
