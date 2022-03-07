using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManFighterScript : MonoBehaviour
{
    private string fighterOneName;
    private string oponentName;
    public Animator manAnim;
    private List<string> allMenNames;
    public AddPointsFighterManScript addPointsFighter;
    public EventsScript signal;
    private FighterScript fighter;
    private List<FighterScript> allFighters;
    private ClipEndEventScript clipEndEventScript;
    private bool isReduced = false;
    private bool isIncreaded = false;

    [SerializeField] private FighterState currentFighterState;
    [SerializeField] private TargetState currentTargetState;
    [SerializeField] private SoundEffectScript soundEffectScript;

    public enum FighterState
    {
        empty,
        stageOne,
        stageTwo,
        final,
        last
    }
    public enum TargetState
    {
        empty,
        stageOne,
        stageTwo,
        final,
        last
    }

    private void Start()
    {
        clipEndEventScript.thisAnim = gameObject.GetComponent<Animator>();
        fighter = new FighterScript();
        fighterOneName = transform.name;
        clipEndEventScript = new ClipEndEventScript();
        soundEffectScript = GetComponent<SoundEffectScript>();

        if (soundEffectScript)
        {
            if (soundEffectScript.thisAudio)
            {
                soundEffectScript.thisAudio = GetComponent<AudioSource>();
            }
        }

        allFighters = new List<FighterScript>
       {
           new FighterScript(100,"ManFighter",0,"fr", FighterScript.FighterType.deffender),
           new FighterScript(101,"ManTarget",0,"fr",FighterScript.FighterType.target),
           new FighterScript(102,"New",0,"null",FighterScript.FighterType.deffender)
       };
        SetFighterState( FighterState.empty );
        SetTargetState( TargetState.empty );
        //clipEndEventScript.UnLooping( manAnim, true );
    }
    private void FixedUpdate()
    {
        if (currentFighterState != FighterState.empty)
        {
            ShowActualFighterState( currentFighterState );
        }
        if (currentTargetState != TargetState.empty)
        {
            ShowActualTargetState( currentTargetState );
        }
    }
    private FighterScript FindManName() {

        foreach (FighterScript fi in allFighters)
        {
            if (fi.fighterType == FighterScript.FighterType.deffender)
            {
                return fi;
            }
            else if (fi.fighterType == FighterScript.FighterType.target)
            {
                return fi;
            }
        }
        return null;

    }
    private bool FindOtherName(string otherName)
    {
        foreach (FighterScript fi in allFighters)
        {
            if (fi.FighterName == otherName)
            {
                return true;
            }
        }
        return false;
    }
    private void OnTriggerEnter(Collider other)
    {
        FighterScript getFighterName = FindManName();
        bool getOtherName = FindOtherName( other.name );

        if (getOtherName && signal && !signal.hasSignal && addPointsFighter)
        {
            signal.RaiseEvent();
            //run audio G
            if (other.name != getFighterName.FighterName)
            {
                if (currentFighterState != FighterState.stageTwo && currentFighterState != FighterState.stageOne && currentFighterState != FighterState.final)
                {

                    SetFighterState( FighterState.stageOne );
                }
            }
            else
            {
                SetTargetState( TargetState.stageOne );              
            }
        }
    }
    private void ShowActualFighterState(FighterState currentFiSt)
    {
        switch (currentFiSt)
        {
            case FighterState.stageOne:
                StartCoroutine( FighterStageOne() );
                break;
            case FighterState.stageTwo:
                StartCoroutine( ChangeFighterAnim() );
                break;
            case FighterState.final:
                StartCoroutine( FighterLastStage() );
                break;
            case FighterState.last:
                TestFunc();
                break;
            default:
                break;
        }
    }
    private int DifferenceTargetPoints(int curP, int changedP)
    {
        int currentPoint = curP;
        int resultP = curP - changedP;
        return resultP;
    }
    private void ShowActualTargetState(TargetState currentTiSt)
    {
        
        switch (currentTiSt)
        {
            case TargetState.stageOne:
                StartCoroutine( TargetStageOne() );
                break;
            case TargetState.stageTwo:
                StartCoroutine( ChangeTargetAnim() );
                break;
            case TargetState.final:
                StartCoroutine( TargetLastStage() );
                break;
            case TargetState.last:
                TestFunc();
                break;
            default:
                break;
        }
    }
    private FighterState SetFighterState(FighterState newFiState){
        currentFighterState = newFiState;
        return currentFighterState;
        }
    private TargetState SetTargetState(TargetState newTiState)
    {
        currentTargetState = newTiState;
        return currentTargetState;
    }
    private IEnumerator FighterStageOne()
    {
        FighterScript getFighterName = FindManName();
        if (soundEffectScript)
        {
            if (soundEffectScript.thisAudio)
            {
                yield return new WaitForSeconds( 2.0f );
                if (!isIncreaded)
                {
                    isIncreaded = true;
                    soundEffectScript.isOn = true;
                    yield return new WaitForSeconds( 2.0f );

                    soundEffectScript.HitSoundEffect();
                    addPointsFighter.AddPoints();
                    if (addPointsFighter.countPoints >= 20)
                    {
                        //defender display msg audio
                        //change anim defender or display canvas with a msg
                        //update fighter score
                        getFighterName.FighterScore = addPointsFighter.countPoints;
                        SetFighterState( FighterState.stageTwo );
                      
                    }               
                }
            }
        }
    }
    private IEnumerator TargetStageOne()
    {
        FighterScript getTargetName = FindManName();
        if (soundEffectScript)
        {
            if (soundEffectScript.thisAudio)
            {
                int thisCurP = addPointsFighter.countPoints;
                yield return new WaitForSeconds( 2.0f );
                if (!isReduced)
                {
                    isReduced = true;
                    soundEffectScript.isOn = true;
                    yield return new WaitForSeconds( 2.0f );
                    soundEffectScript.HitSoundEffect();
                    addPointsFighter.LoosePoint();
                  
                    getTargetName.FighterScore = addPointsFighter.fighterPoints[0];
                    int getDiff = DifferenceTargetPoints( thisCurP , getTargetName.FighterScore );
                    if (addPointsFighter.fighterPoints[0] >= 10 && getDiff == 10)
                    {
                        Debug.Log( "targetsatageOne is done" );

                        SetTargetState( TargetState.stageTwo );
                        StopCoroutine( TargetStageOne() );
                      
                    }
                    
                }
            }
        }
       
    }
    private IEnumerator ChangeTargetAnim()
    {
        if (manAnim)
        {
            if (soundEffectScript)
            {
                if (soundEffectScript.thisAudio)
                {
                    //soundEffectScript.isOn = true;
                    //yield return new WaitForSeconds( 2.0f );
                    //soundEffectScript.HitSoundEffect();

                    manAnim.SetBool( "isCover" , true );
                    yield return new WaitForSeconds( 2.0f );
                    clipEndEventScript.GetClipTime( "CompleteTargetEvent" );
                }
            }
        }
      yield return null;
        
    }
    private IEnumerator ChangeFighterAnim()
    {
        if (manAnim)
        {
            if (soundEffectScript)
            {
                if (soundEffectScript.thisAudio)
                {
                    //soundEffectScript.isOn = true;
                    //yield return new WaitForSeconds( 2.0f );
                    //soundEffectScript.HitSoundEffect();

                    manAnim.SetBool( "isBack" , true );
                    yield return new WaitForSeconds( 2.0f );
                    clipEndEventScript.GetClipTime( "CompleteFighterEvent" );
                }
            } 
        }
        yield return null;

    }
    private IEnumerator FighterLastStage()
    {
        if (soundEffectScript)
        {
            if (soundEffectScript.thisAudio)
            {
                signal.hasSignal = false;
                yield return new WaitForSeconds( 1.0f );

                if (!signal.hasSignal && clipEndEventScript.isFighterChanged)
                {
                    Debug.Log( "calledisFighterChanged" );

                    clipEndEventScript.UnLoopFighter( manAnim );
                    yield return new WaitForSeconds( 3.0f );
                    soundEffectScript.FinishAudio();
                    SetFighterState( FighterState.empty );                 
                }
            }
        }
        
    }
    private IEnumerator TargetLastStage()
    {
        if (soundEffectScript)
        {
            if (soundEffectScript.thisAudio)
            {
                signal.hasSignal = false;
                yield return new WaitForSeconds( 1.0f );

                if (!signal.hasSignal && clipEndEventScript.isTargetChanged)
                {
                    Debug.Log( "calledisTargetChanged" );
                    clipEndEventScript.UnLoopTarget( manAnim );
                    yield return new WaitForSeconds( 3.0f );
                    soundEffectScript.FinishAudio();
                    SetTargetState( TargetState.empty );              
                }
            }
        }
    }
    private void TestFunc() { }
    private void OnTriggerExit(Collider other)
    {
        FighterScript getFighterName = FindManName();
            Debug.Log( "exited" );
        // check
               SetFighterState( FighterState.final );
               SetTargetState( TargetState.final );
            //run new anim state to trigger
    }
}
