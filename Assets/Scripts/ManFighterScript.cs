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
    private void Start()
    {
       fighter = new FighterScript();
       fighterOneName = transform.name;
        allFighters = new List<FighterScript>
       {
           new FighterScript(100,"ManFighter2",0,"fr", FighterScript.FighterType.deffender),
           new FighterScript(101,"ManCovered",0,"fr",FighterScript.FighterType.target),
           new FighterScript(102,"New",0,"null",FighterScript.FighterType.deffender)
       };
       //allMenNames = new List<string>( new [] { "ManFighter2" , "ManCovered" } );
    }
    private FighterScript FindManName() {

        foreach (FighterScript fi in allFighters)
        {
            if(fi.fighterType == FighterScript.FighterType.deffender)
            {
                return fi;
            }
            else if(fi.fighterType == FighterScript.FighterType.target)
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
            //defender audio
            if (other.name != getFighterName.FighterName)
            {
                addPointsFighter.AddPoints();
                    if (addPointsFighter.countPoints >= 20)
                    {
                        Debug.Log( "the pint is 20" );
                        //defender display msg audio
                        //change anim defender or display canvas with a msg
                        //update fighter score
                        getFighterName.FighterScore = addPointsFighter.countPoints;
                        StartCoroutine( ChangeFightAnim() );
                    }
            }
            else
            {
                //target audio
                Debug.Log( "targetCalled" );
                //change Anim target
                addPointsFighter.LoosePoint();
                getFighterName.FighterScore = addPointsFighter.countPoints;
            }

        }

    }
    private IEnumerator ChangeFightAnim()
    {
        if (manAnim)
        {
            yield return new WaitForSeconds( 2.0f );
            //change anim state
            manAnim.SetBool( "isBack" , true );
        }
      yield return null;
        
    }
    private void OnTriggerExit(Collider other)
    {
        FighterScript getFighterName = FindManName();
        if (allMenNames.Contains( other.name ))
        {
            if (other.name != getFighterName.FighterName)
            {
                Debug.Log( "exited" );
                if (signal)
                {
                    signal.hasSignal = false;
                }
                //run new anim state to trigger
            }
        }

    }
}
