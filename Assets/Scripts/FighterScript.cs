using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterScript
{
    public enum FighterType
    {
        target,
        deffender
    }
    public string FighterName { get; set; }
    public int FighterScore { get; set; }
    public string FighterDesc { get; set; }
    public FighterType fighterType; 
    public int FighterId;
    public FighterScript(int fiId,string nm, int score, string desc, FighterType fiType)
    {
        FighterId = fiId;
        FighterName = nm;
        FighterScore = score;
        FighterDesc = desc;
        fighterType = fiType;
    }
    public FighterScript() { }
    public void PrintFighterInfo()
    {
        Debug.Log( "FiName " + FighterName + "FScore " + FighterScore + "FDesc " + FighterDesc );
    }

}
