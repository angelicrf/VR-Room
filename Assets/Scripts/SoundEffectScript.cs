using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SoundEffectScript : MonoBehaviour
{
    public AudioSource thisAudio;
    public bool isOn { get; set; } = false;
    public bool isOff { get; set; } = false;
   // private UnityEvent onFinishSound;
   public void HitSoundEffect()
    {
      
        if (thisAudio)
        {

            if (isOn && !isOff)
            {
                thisAudio.Play();
                StartCoroutine( WaitForSound( thisAudio.clip.length ) );            
            }
            if (isOff && !isOn)
            {
                Debug.Log( "audio Stoped" );
                thisAudio.Stop();
                isOff = false;
            }
        }
        else
        {
            Debug.Log( "No Audio Assigned" );

        }
    }
    public void FinishAudio()
    {
        isOn = false;
        isOff = false;
        thisAudio.Stop();
    }
    IEnumerator WaitForSound(float _duration)
    {
        yield return new WaitForSeconds( _duration );
        print( "audio going to stop" );
        //onFinishSound.Invoke();
        isOn = false;
        isOff = true;
    }
}
