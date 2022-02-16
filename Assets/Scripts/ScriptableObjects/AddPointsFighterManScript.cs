using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class AddPointsFighterManScript : ScriptableObject
{ 
     public List<int> fighterPoints = new List<int>();
     public int countPoints;
     private int minPoint = 10;

    public void AddPoints()
    {
        countPoints += 10;
        Debug.Log( "points added " + fighterPoints[0]);
        fighterPoints[0] = countPoints;
    }
    public void LoosePoint()
    {
        countPoints -= 10;
        Debug.Log( "points reduced " + fighterPoints[0] );
        fighterPoints[0] = countPoints;
    }
    private void OnEnable()
    {
        if (fighterPoints.Count == 0)
        {
            fighterPoints.Add( minPoint );
            Debug.Log( "first Value " + fighterPoints[0] );
        }  
    }
    private void OnDisable()
    {
        Debug.Log( "OnDisabled AddPoints" );
    }
}
