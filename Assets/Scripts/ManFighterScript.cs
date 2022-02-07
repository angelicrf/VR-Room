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
    [SerializeField] private FighterState currentFighterState;
    [SerializeField] private TargetState currentTargetState;

    public enum FighterState
    {
        empty,
        stageOne,
        stageTwo,
        final
    }
    public enum TargetState
    {
        empty,
        stageOne,
        stageTwo,
        final
    }

    private void Start()
    {
        fighter = new FighterScript();
        fighterOneName = transform.name;
        clipEndEventScript = GetComponent<ClipEndEventScript>();
        allFighters = new List<FighterScript>
       {
           new FighterScript(100,"ManFighter2",0,"fr", FighterScript.FighterType.deffender),
           new FighterScript(101,"ManCovered",0,"fr",FighterScript.FighterType.target),
           new FighterScript(102,"New",0,"null",FighterScript.FighterType.deffender)
       };
        SetFighterState( FighterState.empty );
        SetTargetState( TargetState.empty );
        clipEndEventScript.UnLooping( manAnim, true );
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
                SetFighterState( FighterState.stageOne );           
            }
            else
            {
                SetTargetState( TargetState.stageOne );
               // addPointsFighter.LoosePoint();
               // getFighterName.FighterScore = addPointsFighter.countPoints;                
            }
        }
    }
    private void ShowActualFighterState(FighterState currentFiSt)
    {
        FighterScript getFighterName = FindManName();
        switch (currentFiSt)
        {
            case FighterState.stageOne:
                addPointsFighter.AddPoints();
                if (addPointsFighter.countPoints >= 20)
                {
                    //defender display msg audio
                    //change anim defender or display canvas with a msg
                    //update fighter score
                    getFighterName.FighterScore = addPointsFighter.countPoints;
                    SetFighterState( FighterState.stageTwo );
                }
                break;
            case FighterState.stageTwo:
                StartCoroutine( ChangeFightAnim() );
                break;
            case FighterState.final:
                signal.hasSignal = false;
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
        FighterScript getFighterName = FindManName();
        switch (currentTiSt)
        {
            case TargetState.stageOne:
                //StartCoroutine( ChangeTargetAnim() );
                int thisCurP = addPointsFighter.countPoints;
                addPointsFighter.LoosePoint();
                getFighterName.FighterScore = addPointsFighter.countPoints;
                int getDiff = DifferenceTargetPoints( thisCurP , getFighterName.FighterScore );
                if (getDiff == 10)
                {
                    SetTargetState( TargetState.stageTwo );
                }
                break;
            case TargetState.stageTwo:
                TestFunc();
                break;
            case TargetState.final:
                signal.hasSignal = false;
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
    private IEnumerator ChangeTargetAnim()
    {
        yield return new WaitForSeconds( 2.0f );
    }
    private IEnumerator ChangeFightAnim()
    {
        if (manAnim)
        {
            yield return new WaitForSeconds( 2.0f );
            //change anim state
            manAnim.SetBool( "isBack" , true );
            yield return new WaitForSeconds( 2.0f );
            clipEndEventScript.GetClipTime( "CompleteDefenderEvent" );         
        }
      yield return null;
        
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
