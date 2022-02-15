using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Windows.Speech;
public class ProjectAlexaScript : MonoBehaviour
{
    //Prohibited to be Copied or used as a simmilar version
    [SerializeField] private XRDeviceSimulatorControls xrDeviceSimulatorControls;
    public Light thisLight;
    public Animator thisDoor;
    public GameObject thisRoom;
    private string[] allKeywords = new string[] { "yoyo" , "lamp" , "door", "turn","on","off", "light", "cut", "move", "go", "back","come","enter", "exit","close","open" };
    private KeywordRecognizer keywordRecognizer;
    private bool isListening = false;
    private Coroutine receiveMsg;
    private List<string> duplicates = new List<string>();
    private bool isLight = false;
    private bool isNotLight = false;
    private bool isDoor = false;
    private bool isNotDoor = false;
    private bool isRoom = false;
    private bool isNotRoom = false;
    void Awake()
    {
        xrDeviceSimulatorControls = new XRDeviceSimulatorControls();
    
        if (AudioListener.volume == 0.0f)
        {
            AudioListener.volume = 1.0f;         
        }
        keywordRecognizer = new KeywordRecognizer( allKeywords );
        keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        keywordRecognizer.Start();
    }
    private void OnEnable()
    {
        xrDeviceSimulatorControls.Assistant.Enable();
    }
    private void OnDisable()
    {
        xrDeviceSimulatorControls.Assistant.Disable();
    }
    void Update()
    {
        //float leftBtnValue = xrDeviceSimulatorControls.Assistant.YoYoUp.ReadValue<float>();
        //if (leftBtnValue == 1)
        //{
        //    Debug.Log( "P is pressed" );


        //}
        float rightBtnValue = xrDeviceSimulatorControls.Assistant.YoYoDown.ReadValue<float>();
        if (rightBtnValue == 1)
        {
            Debug.Log( "V is pressed" );
            keywordRecognizer.Stop();
        }
    }
    private List<string> FindWord(string thisWord)
    {
            List<string> foundWrd = thisWord.Split(' ').Intersect( allKeywords ).ToList();
        //.Where(x => x!= "yoyo")
        if (!thisWord.Contains( "yoyo" ))
        {
            Debug.Log( "foundWord " + foundWrd[0] );
            if (foundWrd != null)
            {
                duplicates.Add( foundWrd[0] );
                Debug.Log( "allDuplicates " + duplicates.Count );
                return duplicates;
            }
        }
        
        return null;
    }
    private bool WordsPriority()
    {
        if (duplicates.Contains( "lamp" ) || duplicates.Contains( "light" ))
        {
            isLight = duplicates.Contains( "turn" ) || duplicates.Contains( "on" ) ? isLight = true :
                      duplicates.Contains( "cut" ) || duplicates.Contains( "off" ) ? isLight = false :
                      isLight = false;
            return isLight;
        }
        if(duplicates.Contains( "door" ))
        {
            isDoor = duplicates.Contains( "open" ) ? isDoor = true :
                     duplicates.Contains( "close" ) ? isDoor = false :
                     isDoor = false;
            return isDoor;
        }

        if (duplicates.Contains( "room" ))
        {
            isRoom = duplicates.Contains( "go" ) ||
               duplicates.Contains( "enter" )  ||
               duplicates.Contains( "move" ) ||
               duplicates.Contains( "teleport" )
               ? isRoom = true :
               duplicates.Contains( "exit" ) ||
               duplicates.Contains( "go" ) && duplicates.Contains( "back" ) ||
               duplicates.Contains( "move" ) && duplicates.Contains( "back" )
               ? isRoom = false :
               isRoom = false;
            return isRoom;
        }
     
        return false;
    }
    private void OnPhraseRecognized(PhraseRecognizedEventArgs arg) {
        string clean = Regex.Replace( arg.text , "[^a-zA-Z0-9\x20]" , String.Empty );
        Debug.Log( "cleanWord " + clean );
        if (String.Equals( clean , allKeywords[0] ) && !isListening)
        {
            Debug.Log( "yoyoText" );
            isListening = true;
            receiveMsg = StartCoroutine( ListeningCo() );
        }
        if (isListening)
        {
            Debug.Log( "isListening" + isListening );
            StartCoroutine( CallTwoFuncsCo( clean ) );
            if (isLight)
            {
                TurnOnLight();
            }
            else if (isNotLight)
            {
                TurnOffLight();
            }
            else if (isRoom)
            {
                EnterRoom();
            }
            else if (isNotRoom)
            {
                ExitRoom();
            }
            else if (isDoor)
            {
                OpenDoor();
            }
            else if (isNotDoor)
            {
                CloseDoor();
            }
            else
            {
                Debug.Log( "No Action Found" );

            }
        }
    }
    private IEnumerator CallTwoFuncsCo(string str)
    {
        List<string> resultWords = FindWord( str );
        if (resultWords != null && duplicates.Count >= 2)
        {
            WordsPriority();
        }
        yield return null;
    }
    private void TurnOnLight()
    {
        Debug.Log( "turnOn" );
        if (thisLight)
        {
            // StopCoroutine( receiveMsg );      
            thisLight.intensity = 4.0f;
            isListening = false;
        }
    }
    private void TurnOffLight()
    {
        Debug.Log( "turnOff" );
        if (thisLight)
        {
            // StopCoroutine( receiveMsg );
            thisLight.intensity = 0f;
            isListening = false;
        }
    }
    private void OpenDoor()
    {
        Debug.Log( "openDoor" );
        // StopCoroutine( receiveMsg );
        // run animation
        if (thisDoor)
        {
            thisDoor.SetBool( "isOpen" , true );
            isListening = false;
        }

    }
    private void CloseDoor()
    {
        Debug.Log( "closeDoor" );
        // StopCoroutine( receiveMsg );
        // run animation
        if (thisDoor)
        {
            thisDoor.SetBool( "isOpen" , false );
            // add more if needed
            isListening = false;
        }
     
    }
    private void EnterRoom()
    {
        if (isRoom)
        {
            Debug.Log( "enter room" );
            // StopCoroutine( receiveMsg );
            // teleport or change the scene
            //isRoom.
            isListening = false;
        }
    }
    private void ExitRoom()
    {
        Debug.Log( "exit room" );
        if (thisRoom)
        {
            // StopCoroutine( receiveMsg );
            // teleport back or change the scene
            //isRoom
            isListening = false;
        }
     
    }
    IEnumerator ListeningCo()
    {
        isListening = true;
        yield return new WaitForSeconds( 9.0f );
        isListening = false;
        
    }
}
