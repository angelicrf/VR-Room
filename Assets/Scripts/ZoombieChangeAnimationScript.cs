using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ZoombieChangeAnimationScript : MonoBehaviour
{

    [SerializeField] private GameObject zoombieMan;
    [SerializeField] private GameObject camPos;
    [SerializeField] private Animator zoombieAnim;
    private XRDeviceSimulatorControls xrdeviceSimulatorControl;
    private LineRenderer m_LineRenderer;
    [SerializeField] private GameObject thisObj;
    private GameObject blockBox;
    
    private XRInteractorLineVisual m_ValidColorGradient;
    private void Awake()
    {
        xrdeviceSimulatorControl = new XRDeviceSimulatorControls();
        m_LineRenderer = thisObj.GetComponent<LineRenderer>();
        m_ValidColorGradient = thisObj.GetComponent<XRInteractorLineVisual>();
        blockBox = GameObject.Find( "/AllBuildingBlocks/BlockBox" );
    }
    void FixedUpdate()
    {
        
        m_ValidColorGradient.validColorGradient.SetKeys( new[] { new GradientColorKey( Color.magenta , 0f ) , new GradientColorKey( Color.magenta , 1f ) } , new[] { new GradientAlphaKey( 1f , 0f ) , new GradientAlphaKey( 1f , 1f ) } );
        m_LineRenderer.colorGradient = m_ValidColorGradient.validColorGradient;
        ChangeAnime();
    }
    void DrawLineThreeD(LineRenderer m_LineRenderer)
    {
        var rf = m_LineRenderer.colorGradient;
        Debug.Log( "rf" + rf );
    }
    void OnDrawGizmosSelected()
    {
        // Draws a 5 unit long yellow line in front of the object
        Gizmos.color = Color.yellow;
        Vector3 direction = zoombieMan.transform.TransformDirection( Vector3.forward ) * 5;
        Gizmos.DrawRay( zoombieMan.transform.position , direction );
    }
    private void OnEnable()
    {
        xrdeviceSimulatorControl.Enable();
    }
    private void OnDisable()
    {
        xrdeviceSimulatorControl.Disable();
    }
    public void CreateNewRay()
    {
        Vector3 direction = new Vector3(4,0,5);
        Debug.DrawRay( zoombieMan.transform.position , direction * 1f , Color.yellow );
        Ray newRay = new Ray( zoombieMan.transform.position , direction );
        //Debug.Log( "newRay " + newRay);
        bool result = Physics.Raycast( newRay , out RaycastHit rayhit , 1f );
        Debug.Log( "result " + result );

        if (result)
        {
            Debug.Log( "Fired and hit a wall" );
        }
    }
    public void ChangeAnime()
    {
        Debug.Log( "changeAnimCalled" );

        float valueController = xrdeviceSimulatorControl.InputControls.Grip.ReadValue<float>();
        //Debug.Log( "valueController " + valueController );
        if (zoombieMan.gameObject.CompareTag( "ZoobmieMan" ))
            if (valueController != 0)
            {
                StartCoroutine( SetAnimCo() );
            }
            else
            {
               Vector2 newCamPos = camPos.transform.position;
               Vector2 blockBoxPos = blockBox.transform.position;
               Vector2 zoombiePos = zoombieMan.transform.position;
               Vector2 diffPos = blockBoxPos - zoombiePos;
                    //newCamPos - zoombiePos;
               if (diffPos.y < 2f || diffPos.x < 2f)
                {
                    StartCoroutine( SetAnimBackCo() );
                }
            }
    }
    IEnumerator SetAnimCo()
    {
        Debug.Log( "rayCast zoombie" );
        yield return new WaitForSeconds( 0.2f );
        zoombieAnim.SetBool( "isFall" , true );
    }
    IEnumerator SetAnimBackCo()
    {
        zoombieAnim.SetBool( "isFall" , true );
        yield return new WaitForSeconds( 0.5f );
    }
    public void TestChangeAnim()
    {
        Debug.Log( "zoombie selected" );

        // zoombieAnim.SetBool( "isFall" , true );
    }
}
