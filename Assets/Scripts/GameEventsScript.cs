using UnityEngine;
using UnityEngine.Events;

public class GameEventsScript : MonoBehaviour
{
    public EventsScript thisEvent;
    public UnityEvent thisUnityEvent;
    private void OnEnable()
    {
        thisEvent.RegisterNewEvent( this );
    }
    private void OnDisable()
    {
        thisEvent.UnRegisterNewEvent( this );
    }
    public void OnRaisedEvent()
    { 
        thisUnityEvent.Invoke();
        Debug.Log( "unityEventInvoked" + thisUnityEvent );

    }

}
