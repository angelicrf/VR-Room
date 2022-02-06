using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EventsScript : ScriptableObject
{
    public List<GameEventsScript> allListeners = new List<GameEventsScript>();
    public bool hasSignal = false;
    public void RegisterNewEvent(GameEventsScript newEvent)
    {
        if (!allListeners.Contains( newEvent)){
            allListeners.Add( newEvent );
        }
    }
    public void RaiseEvent()
    {
        Debug.Log( "raised event" );

        for (int i = allListeners.Count -1; i >= 0; i--)
        {
            allListeners[i].OnRaisedEvent();
        }
        hasSignal = true;
    }
    public void UnRegisterNewEvent(GameEventsScript newEvent)
    {
        if (allListeners.Contains( newEvent ))
        {
            allListeners.Remove( newEvent );
        }
    }

}
