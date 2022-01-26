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
        //else
        //{
        //    xrDefaultInputAction = new XRIDefaultInputActions();
        //}
    }
    private void OnDestroy()
    {
        if (xrDeviceSimulator != null && xrDeviceSimulator.activeSelf)
        {
            xrDeviceSimulatorControls.Assistant.Disable();
        }
        //else if(xrDefaultInputAction != null)
        //{
          
        //    xrDefaultInputAction.Assistant.Disable();
        //}
    }
    private void OnEnable()
    {
        if (xrDeviceSimulator != null && xrDeviceSimulator.activeSelf)
        {
            xrDeviceSimulatorControls.Assistant.Enable();
        }
        //else if (xrDefaultInputAction != null)
        //{
        //    xrDefaultInputAction.Assistant.Enable();
        //}
    }

    public void GetLeftBtnValue()
    {
        leftBtnValue = xrDeviceSimulatorControls.Assistant.LeftMove.ReadValue<float>();
        if (leftBtnValue == 0)
        {
            target.transform.Translate( Vector3.back * speed * Time.deltaTime );
        }
    }
    public void GetRightBtnValue()
    {
        rightBtnValue = xrDeviceSimulatorControls.Assistant.RightMove.ReadValue<float>();
        if (rightBtnValue == 0)
        {
            target.transform.Translate( Vector3.forward * speed * Time.deltaTime );
        }
    }
    public void GetDownBtnValue()
    {
        downBtnValue = xrDeviceSimulatorControls.Assistant.DownMove.ReadValue<float>();
        if (downBtnValue == 0)
        {
            target.transform.Translate( Vector3.left * speed * Time.deltaTime );
        }
    }
    public void GetUpBtnValue()
    {
       
        upBtnValue = xrDeviceSimulatorControls.Assistant.UpMove.ReadValue<float>();
        if (upBtnValue == 0)
        {
            target.transform.Translate( Vector3.right * speed * Time.deltaTime );
        }
    }
}
