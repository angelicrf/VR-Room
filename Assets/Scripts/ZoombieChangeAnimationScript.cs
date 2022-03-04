using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ZoombieChangeAnimationScript : MonoBehaviour
{

    [SerializeField] private GameObject zombieMan;
    [SerializeField] private GameObject camPos;
    [SerializeField] private Animator zombieAnim;
    [SerializeField] private GameObject thisObj;

    private XRDeviceSimulatorControls xrdeviceSimulatorControl;
    private LineRenderer m_LineRenderer;
    private ClipEndEventScript clipEndEventScript;
    private GameObject blockBox;
    private GameObject zombieFight;
    private GameObject zombieTarget;

    private XRInteractorLineVisual m_ValidColorGradient;
    private void Awake()
    {
        xrdeviceSimulatorControl = new XRDeviceSimulatorControls();
        m_LineRenderer = thisObj.GetComponent<LineRenderer>();
        clipEndEventScript = GetComponent<ClipEndEventScript>();
        m_ValidColorGradient = thisObj.GetComponent<XRInteractorLineVisual>();
        blockBox = GameObject.Find( "/AllBuildingBlocks/BlockBox" );
        zombieFight = GameObject.Find( "/AllZombies/ZombieFit" );
        zombieTarget = GameObject.Find( "/AllZombies/ZombieTarget" );
    }
    void FixedUpdate()
    {       
        m_ValidColorGradient.validColorGradient.SetKeys( new[] { new GradientColorKey( Color.magenta , 0f ) , new GradientColorKey( Color.magenta , 1f ) } , new[] { new GradientAlphaKey( 1f , 0f ) , new GradientAlphaKey( 1f , 1f ) } );
        m_LineRenderer.colorGradient = m_ValidColorGradient.validColorGradient;
    }
    private void DrawLineThreeD(LineRenderer m_LineRenderer)
    {
        var rf = m_LineRenderer.colorGradient;
    }
    private void OnDrawGizmosSelected()
    {
        // Draws a 5 unit long yellow line in front of the object
        Gizmos.color = Color.yellow;
        Vector3 direction = zombieMan.transform.TransformDirection( Vector3.forward ) * 5;
        Gizmos.DrawRay( zombieMan.transform.position , direction );
    }
    private void OnEnable()
    {
        xrdeviceSimulatorControl.Enable();
    }
    private void OnDisable()
    {
        xrdeviceSimulatorControl.Disable();
    }
    private void CreateNewRay()
    {
        Vector3 direction = new Vector3(4,0,5);
        Debug.DrawRay( zombieMan.transform.position , direction * 1f , Color.yellow );
        Ray newRay = new Ray( zombieMan.transform.position , direction );
        bool result = Physics.Raycast( newRay , out RaycastHit rayhit , 1f );
        if (result)
        {
            Debug.Log( "Fired and hit a wall" );
        }
    }
    private void ChangeAnime()
    {
        float valueController = xrdeviceSimulatorControl.InputControls.Grip.ReadValue<float>();

        if (zombieMan.gameObject.CompareTag( "ZombieMan" ))
            if (valueController != 0)
            {
                StartCoroutine( SetAnimCo() );
            }
            else
            {
                StartCoroutine( SetAnimFightCo() );
            }
    }
    IEnumerator SetAnimCo()
    {
        yield return new WaitForSeconds( 0.2f );
        zombieAnim.SetBool( "isFall" , true );
    }
    IEnumerator SetAnimBackCo()
    {
        zombieAnim.SetBool( "isFall" , true );
        yield return new WaitForSeconds( 2.0f );
        clipEndEventScript.GetClipTime("RunDieCompleteHandler");
    }
    IEnumerator GetCurrentAnimTime()
    {
        bool getAnimTime = zombieAnim.GetCurrentAnimatorStateInfo( 0 ).normalizedTime < 1;
        yield return new WaitUntil( () => getAnimTime );
        zombieAnim.SetBool( "isFall" , false );
    }

    private void RunDieCompleteHandler()
    {
        zombieAnim.SetBool( "isFall" , false );
    }
    private void FightRoundOne()
    {
        zombieAnim.SetBool( "isFight" , false );
        StartCoroutine( FightRoundTwoCo() );
    }
    private void FightRoundTwoEnd()
    {
        zombieAnim.SetBool( "isFallAfterFight" , false );
        zombieFight.GetComponent<Animator>().enabled = false;
        //create a back idle animation and transit into it
    }
    IEnumerator FightRoundTwoCo()
    {
        zombieAnim.SetBool( "isFallAfterFight" , true );
        yield return new WaitForSeconds( 1f );
    }
    private void CallAnimBack()
    {
        StartCoroutine( SetAnimBackCo() );
    }
    private void CallAnimFight()
    {
        StartCoroutine( SetAnimFightCo() );
    }
    IEnumerator SetAnimFightCo()
    {
        zombieAnim.SetBool( "isFight" , true );
        clipEndEventScript.GetClipTime( "FightRoundOne" );

        yield return new WaitForSeconds( 2f );
     
        //if (!targetDefended && fightStarted)
        //{
        //    StartCoroutine( SetAnimTargetCo() );
        //    targetDefended = true;
        //}
        //yield return new WaitUntil( () => targetDefended );
        //if (!fightOver)
        //{
        //    zombieAnim.SetBool( "isFallAfterFight" , true );
        //    fightOver = true;
        //}
        //yield return new WaitUntil( () => fightOver );
        //clipEndEventScript.GetClipTime("FightCompleteHandler");
    }
    private IEnumerator SetAnimTargetCo()
    {
        Debug.Log( "zombieTarget called" );

        yield return new WaitForSeconds( 3.0f );
    }
    private void CheckPosByDistance()
    {
        Vector2 newCamPos = camPos.transform.position;
        Vector2 blockBoxPos = blockBox.transform.position;
        Vector2 zombieFitPos = zombieFight.transform.position;
        Vector2 zoombiePos = zombieMan.transform.position;
        Vector2 diffZBPos = blockBoxPos - zoombiePos;
        Vector2 diffZFPos = zombieFitPos - zoombiePos;

        if (Mathf.Abs( diffZBPos.y) < 2f  || Mathf.Abs(diffZBPos.x) < 2f )
        {
            StartCoroutine( SetAnimBackCo() );
        }
        if (Mathf.Abs( diffZFPos.x ) < 3.00f)
        {
            StartCoroutine( SetAnimFightCo() );
        }
    }
    public void TestChangeAnim()
    {
        Debug.Log( "zoombie selected" );
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.name)
        {
            case "ZombieTarget":
               StartCoroutine( SetAnimFightCo() );
                //start target fight first step
                break;
            case "BlockBox":
                StartCoroutine( SetAnimBackCo() );
                break;
            default:
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        switch (other.name)
        {
            case "ZombieTarget":
                Debug.Log( "exited the trriger" );
                clipEndEventScript.GetClipTime( "FightRoundTwoEnd" ); 
                //target back to idle
                break;
            default:
                break;
        }
    }
}
