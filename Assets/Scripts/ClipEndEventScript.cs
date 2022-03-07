using System.Collections;
using UnityEngine;

public class ClipEndEventScript: MonoBehaviour
{
    [SerializeField] private Animator thisAnim;

    [System.ComponentModel.DefaultValue( false )]
    public bool isTargetChanged { get; set; }

    [System.ComponentModel.DefaultValue( false )]
    public bool isFighterChanged { get; set; }
    public bool animClipEnded { get; set; } = false;
    public bool targetStarted { get; set; } = false;

    public void GetClipTime(string eventFunc)
    {
        Debug.Log( "getclipTime called " + thisAnim.runtimeAnimatorController.animationClips.Length );
        for (int i = 0; i < thisAnim.runtimeAnimatorController.animationClips.Length; i++)
        {
            AnimationClip clip = thisAnim.runtimeAnimatorController.animationClips[i];
            AnimationEvent animationEndEvent = new AnimationEvent();
            animationEndEvent.time = clip.length;
            animationEndEvent.functionName = eventFunc;
            animationEndEvent.stringParameter = clip.name;
            clip.AddEvent( animationEndEvent );
            if (clip.events.Length > 0 &&  i < clip.events.Length)
            { 
                if (clip.events[i] != null)
                {
                    if (clip.events[i].functionName == "FightRoundOne" && !targetStarted)
                    {
                       targetStarted = true;
                        Debug.Log( "FightRoundOne completed" );
                    }
                    else if (clip.events[i].functionName == "FightRoundTwoEnd" && !animClipEnded)
                    {
                        animClipEnded = true;
                        Debug.Log( "FightRoundTwoEnd is completed" );
                    }
                }
            }                    
        }
    }
    bool AnimatorIsPlaying()
    {
        return thisAnim.GetCurrentAnimatorStateInfo( 0 ).length >
               thisAnim.GetCurrentAnimatorStateInfo( 0 ).normalizedTime;
    }
    public bool AnimatorIsPlaying(string stateName)
    {
        return AnimatorIsPlaying() && thisAnim.GetCurrentAnimatorStateInfo( 0 ).IsName( stateName );
    }
    public void UnLoopTarget(Animator curAnim)
    {
        var targetState = curAnim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Man_Cover_To_Stand" );
       
        if (targetState)
        {
            curAnim.SetBool( "isPaused" , true );
                //PlayInFixedTime( "Man_Cover_To_Stand" , 0 , 0.0f );
            isTargetChanged = false;
        }
    }
    public void UnLoopFighter(Animator curAnim)
    {
        var fighterState = curAnim.GetCurrentAnimatorStateInfo(0).IsName( "Back_Fight" );
        if (fighterState)
        {
            curAnim.SetBool( "isPaused" , true );
                //PlayInFixedTime( "Back_Fight" , 0 , 0.0f );
            isFighterChanged = false;
        }
        //.Rebind();
        //for (int i = 0; i < curAnim.runtimeAnimatorController.animationClips.Length; i++)
        //{
        //   // curAnim.runtimeAnimatorController.animationClips[i].wrapMode = WrapMode.Once;
        //    Debug.Log( "wrapOnce called" );

        //    AnimationClip clip = curAnim.runtimeAnimatorController.animationClips[i];
        //    //var settings = AnimationUtility.GetAnimationClipSettings( clip );
        //    //settings.loopTime = isOn;
        //    AnimationClipSettings setting = new AnimationClipSettings { loopTime = false };
        //    AnimationUtility.SetAnimationClipSettings( clip , setting );
        //}
    }
    private void CompleteTargetEvent()
    {
        StartCoroutine( TargetCo() );
    }
    private void CompleteFighterEvent()
    {
        StartCoroutine( FighterCo() );
    }
    private IEnumerator TargetCo()
    {
        bool isTarget = GetAnimParameterName( thisAnim, "isCover" );
       
        if (isTarget)
        {
            thisAnim.SetBool( "isCover" , false );
            yield return new WaitUntil( () => isTargetChanged = true );
            yield return new WaitUntil( () => thisAnim.gameObject.GetComponent<Collider>().isTrigger = false );
        }
    }
    private IEnumerator FighterCo()
    {
        bool isFighter = GetAnimParameterName(thisAnim, "isBack");
        if (isFighter)
        {
            thisAnim.SetBool( "isBack" , false );
            yield return new WaitUntil( () => isFighterChanged = true );
            yield return new WaitUntil( () => thisAnim.gameObject.GetComponent<Collider>().isTrigger = false );          
        }
    }
    private bool GetAnimParameterName(Animator anim, string thisName)
    {
        for (int i = 0; i < thisAnim.parameters.Length; i++)
        {
            if (anim.parameters[i].name == thisName)
            {
                return true;
            }
        }
        return false;
    }
}
