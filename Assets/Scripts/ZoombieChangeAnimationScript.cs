using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
    private ClipEndEventScript clipEndEventScript = new ClipEndEventScript();
    private GameObject blockBox;
    private GameObject zombieFight;
    private GameObject zombieTarget;
    private bool thisTargetStarted;
    private bool thisAnimClipEnded;

    private XRInteractorLineVisual m_ValidColorGradient;

    private void Awake()
    {
        if (gameObject.GetComponent<Animator>() && clipEndEventScript != null)
        {
            clipEndEventScript.thisAnim = gameObject.GetComponent<Animator>();
            thisTargetStarted = false;
            thisAnimClipEnded = false;
            xrdeviceSimulatorControl = new XRDeviceSimulatorControls();
            m_LineRenderer = thisObj.GetComponent<LineRenderer>();
            m_ValidColorGradient = thisObj.GetComponent<XRInteractorLineVisual>();
            blockBox = GameObject.Find( "/AllBuildingBlocks/BlockBox" );
            zombieFight = GameObject.Find( "/AllZombies/ZombieFit" );
            zombieTarget = GameObject.Find( "/AllZombies/ZombieTarget" );
        }

    }
    void FixedUpdate()
    {
        if (clipEndEventScript != null)
        {
            thisTargetStarted = clipEndEventScript.targetStarted;
            thisAnimClipEnded = clipEndEventScript.animClipEnded;
            m_ValidColorGradient.validColorGradient.SetKeys( new[] { new GradientColorKey( Color.magenta , 0f ) , new GradientColorKey( Color.magenta , 1f ) } , new[] { new GradientAlphaKey( 1f , 0f ) , new GradientAlphaKey( 1f , 1f ) } );
            m_LineRenderer.colorGradient = m_ValidColorGradient.validColorGradient;
        }
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
        if (zombieFight.GetComponent<Animator>().GetParameter( 0 ).name == "isFall")
        {
            yield return new WaitForSeconds( 0.2f );
            zombieFight.GetComponent<Animator>().SetBool( "isFall" , true );
        }
    }
    IEnumerator SetAnimBackCo()
    {
        if (zombieFight.GetComponent<Animator>().GetParameter( 0 ).name == "isFall")
        {
            zombieFight.GetComponent<Animator>().SetBool( "isFall" , true );
            yield return new WaitForSeconds( 2.0f );
            clipEndEventScript.GetClipTime( "RunDieCompleteHandler" );
        }
    }
    IEnumerator GetCurrentAnimTime()
    {
        bool getAnimTime = zombieAnim.GetCurrentAnimatorStateInfo( 0 ).normalizedTime < 1;
        yield return new WaitUntil( () => getAnimTime );
        zombieAnim.SetBool( "isFall" , false );
    }

    private void RunDieCompleteHandler()
    {
        if (zombieFight.GetComponent<Animator>().GetParameter( 0 ).name == "isFall")
        {
            zombieFight.GetComponent<Animator>().SetBool( "isFall" , false );
        }
    }
    private void FightRoundOne()
    {
        if (zombieFight.GetComponent<Animator>().GetParameter( 0 ).name == "isFight")
        {
            zombieFight.GetComponent<Animator>().SetBool( "isFight" , false );
            StartCoroutine( FightRoundTwoCo() );
        }
    }
    private void FightRoundTwoEnd()
    {
        if (zombieFight.GetComponent<Animator>().GetParameter( 2 ).name == "isFallAfterFight") {
            zombieFight.GetComponent<Animator>().SetBool( "isFallAfterFight" , false );
            zombieFight.GetComponent<Animator>().enabled = false;
           // clipEndEventScript.animClipEnded = true;
        }
        //create a back idle animation and transit into it
    }
    IEnumerator FightRoundTwoCo()
    {
        if (zombieFight.GetComponent<Animator>().GetParameter( 2 ).name == "isFallAfterFight")
        {
            zombieFight.GetComponent<Animator>().SetBool( "isFallAfterFight" , true );
            yield return new WaitForSeconds( 1f );
        }
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
        if(zombieFight.GetComponent<Animator>().GetParameter( 0 ).name == "isFight"){
            zombieFight.GetComponent<Animator>().SetBool( "isFight" , true );
            TargetStartAnim();
            yield return new WaitForSeconds( 2f );
            clipEndEventScript.GetClipTime( "FightRoundOne" );
            yield return new WaitForSeconds(1.2f);
            clipEndEventScript.GetClipTime( "FightRoundTwoEnd" );
        }
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
    private void SetTargetEnd()
    {
        if (zombieAnim)
        {
            if (zombieTarget.GetComponent<Animator>().GetParameter( 0 ).name == "isTargetSt")
            {
                zombieTarget.GetComponent<Animator>().SetBool( "isTargetSt" , false );            
            }
        }
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
    public void TargetStartAnim()
    {
        if (zombieAnim)
        {
            if (zombieTarget.GetComponent<Animator>().GetParameter(0).name == "isTargetSt")
            {
                zombieTarget.GetComponent<Animator>().SetBool( "isTargetSt" , true );                   
            }
        }
    }
    IEnumerator TargetTriggerExit()
    {
        yield return new WaitUntil( () => thisAnimClipEnded );
        SetTargetEnd();
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.name)
        {
            case "ZombieTarget":
                StartCoroutine( SetAnimFightCo() );
                //TaskToRunAnim();
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
                StartCoroutine( TargetTriggerExit() );
                break;
            default:
                break;
        }
    }
}
