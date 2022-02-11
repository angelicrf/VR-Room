using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Windows.Speech;
public class ProjectAlexaScript : MonoBehaviour
{
    //Prohibited to be Copied or used as a simmilar version
    [SerializeField] private XRDeviceSimulatorControls xrDeviceSimulatorControls;
    public Light lightSource;
    private string[] allKeywords = new string[] { "yoyo" , "turn on the light" , "turn off the light" };
    private KeywordRecognizer keywordRecognizer;
    private bool isListening = false;
    private Coroutine receiveMsg;

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
    private void OnPhraseRecognized(PhraseRecognizedEventArgs arg) {
        string clean = Regex.Replace( arg.text , "[^a-zA-Z0-9\x20]" , String.Empty );
        Debug.Log( "cleanWord " + clean );
        if (String.Equals( clean , allKeywords[0] ) && !isListening)
        {
            Debug.Log( "yoyoText" );
            //isListening = true;
            receiveMsg = StartCoroutine( ListeningCo());
        }
        if (isListening)
        {
            Debug.Log( "isListening" + isListening);
            if(String.Equals(clean, allKeywords[1] )){
                TurnOnLight();
            }else if(String.Equals( clean , allKeywords[2] )){
                TurnOffLight();
            }
        }
    }
    private void TurnOnLight()
    {
        Debug.Log( "turnOn" );
       // StopCoroutine( receiveMsg );      
        lightSource.intensity = 4.0f;
        isListening = false;
    }
    private void TurnOffLight()
    {
        Debug.Log( "turnOff" );
       // StopCoroutine( receiveMsg );
        lightSource.intensity = 0f;
        isListening = false;
    }
    IEnumerator ListeningCo()
    {
        
        isListening = true;
        yield return new WaitForSeconds( 9.0f );
        isListening = false;
    }
}
