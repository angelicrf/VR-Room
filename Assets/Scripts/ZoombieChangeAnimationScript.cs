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
    private GameObject zombieFight;
    private bool animIsFight = false;
    private bool animIsRunDie = false;
    //public AnimationEvent OnAnimationComplete;
    private XRInteractorLineVisual m_ValidColorGradient;
    private void Awake()
    {
        xrdeviceSimulatorControl = new XRDeviceSimulatorControls();
        m_LineRenderer = thisObj.GetComponent<LineRenderer>();
        m_ValidColorGradient = thisObj.GetComponent<XRInteractorLineVisual>();
        blockBox = GameObject.Find( "/AllBuildingBlocks/BlockBox" );
        zombieFight = GameObject.Find( "/AllZombies/ZombieFit" );
    }
    void FixedUpdate()
    {
        
        m_ValidColorGradient.validColorGradient.SetKeys( new[] { new GradientColorKey( Color.magenta , 0f ) , new GradientColorKey( Color.magenta , 1f ) } , new[] { new GradientAlphaKey( 1f , 0f ) , new GradientAlphaKey( 1f , 1f ) } );
        m_LineRenderer.colorGradient = m_ValidColorGradient.validColorGradient;
        if (animIsFight)
        {
            CallAnimFight();
            animIsFight = false;
        }
        if (animIsRunDie)
        {
            CallAnimBack();
            animIsRunDie = false;
        }
    }
    private void DrawLineThreeD(LineRenderer m_LineRenderer)
    {
        var rf = m_LineRenderer.colorGradient;
        Debug.Log( "rf" + rf );
    }
    private void OnDrawGizmosSelected()
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
    private void CreateNewRay()
    {
        Vector3 direction = new Vector3(4,0,5);
        Debug.DrawRay( zoombieMan.transform.position , direction * 1f , Color.yellow );
        Ray newRay = new Ray( zoombieMan.transform.position , direction );
        bool result = Physics.Raycast( newRay , out RaycastHit rayhit , 1f );
        Debug.Log( "result " + result );

        if (result)
        {
            Debug.Log( "Fired and hit a wall" );
        }
    }
    private void ChangeAnime()
    {
        Debug.Log( "changeAnimCalled" );

        float valueController = xrdeviceSimulatorControl.InputControls.Grip.ReadValue<float>();

        if (zoombieMan.gameObject.CompareTag( "ZoobmieMan" ))
            if (valueController != 0)
            {
                StartCoroutine( SetAnimCo() );
            }
            else
            {
                StartCoroutine( SetAnimFightCo() );
            }
    }
    private bool ChangeFight()
    {
        return animIsFight = true;
    }
    private bool ChangeRunDie()
    {
        return animIsRunDie = true;
    }
    public void CallTrigerFight()
    {
        ChangeFight();
    }
    public void CallTrigerRunDie()
    {
        ChangeRunDie();
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
        Debug.Log( "name " + zoombieAnim.runtimeAnimatorController.animationClips[1].name );

        AnimationClip clip = zoombieAnim.runtimeAnimatorController.animationClips[1];
        AnimationEvent animationEndEvent = new AnimationEvent();
        animationEndEvent.time = clip.length;
        animationEndEvent.functionName = "AnimationCompleteHandler";
        animationEndEvent.stringParameter = clip.name;

        clip.AddEvent( animationEndEvent );
        yield return new WaitForSeconds(2.00f);
        //bool getAnimTime = zoombieAnim.GetCurrentAnimatorStateInfo( 0 ).normalizedTime < 1;
        //Debug.Log( "callBackAnim" + zoombieAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name );
        //Debug.Log( "callBackAnimTime " + zoombieAnim.GetCurrentAnimatorStateInfo(0).normalizedTime );
        //yield return new WaitUntil( () => getAnimTime );
        //zoombieAnim.SetBool( "isFall" , false );
    }
    public void AnimationCompleteHandler(string name)
    {
        Debug.Log( $"{name} animation complete." );
        zoombieAnim.SetBool( "isFall" , false );
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
        yield return new WaitForSeconds( 3f );
        zoombieAnim.SetBool( "isFight" , true );
        if (!zombieFight)
        {
            yield return new WaitForSeconds( 0.6f );
            zoombieAnim.SetBool( "isFight" , false );
            yield return new WaitForSeconds( 0.6f );
            zoombieAnim.SetBool( "isFallBack" , true );
        }

        yield return new WaitForSeconds( 0.6f );
        zoombieAnim.SetBool( "isFallAfterFight" , true );
        bool getAnimTime = zoombieAnim.GetCurrentAnimatorStateInfo( 0 ).normalizedTime < 1;
        yield return new WaitUntil( () => getAnimTime );
        zoombieAnim.SetBool( "isFallAfterFight" , false );
        zoombieAnim.SetBool( "isFight" , false );
    }
    private void CheckPosByDistance()
    {
        Vector2 newCamPos = camPos.transform.position;
        Vector2 blockBoxPos = blockBox.transform.position;
        Vector2 zombieFitPos = zombieFight.transform.position;
        Vector2 zoombiePos = zoombieMan.transform.position;
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
}
