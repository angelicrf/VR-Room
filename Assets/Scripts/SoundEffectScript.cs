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
                thisAudio.Stop();
                isOff = false;
            }
        }
        else
        {
            Debug.Log( "No Audio Assigned" );

        }
    }
    IEnumerator WaitForSound(float _duration)
    {
        yield return new WaitForSeconds( _duration );
        print( "FinishAudio" );
        //onFinishSound.Invoke();
        isOn = false;
        isOff = true;
    }
}
