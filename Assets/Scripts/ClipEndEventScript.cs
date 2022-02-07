using System.Collections;
using UnityEditor;
using UnityEngine;

public class ClipEndEventScript: MonoBehaviour
{
    [SerializeField] private Animator thisAnim;

    public void GetClipTime(string eventFunc)
    {
        for (int i = 0; i < thisAnim.runtimeAnimatorController.animationClips.Length; i++)
        {
            AnimationClip clip = thisAnim.runtimeAnimatorController.animationClips[i];
            AnimationEvent animationEndEvent = new AnimationEvent();
            animationEndEvent.time = clip.length;
            animationEndEvent.functionName = eventFunc;
            animationEndEvent.stringParameter = clip.name;

            clip.AddEvent( animationEndEvent );
        }
    }
    public void UnLooping(Animator curAnim, bool isOn)
    {
        for (int i = 0; i < curAnim.runtimeAnimatorController.animationClips.Length; i++)
        {
            AnimationClip clip = curAnim.runtimeAnimatorController.animationClips[i];
            var settings = AnimationUtility.GetAnimationClipSettings( clip );
            settings.loopTime = isOn;
            AnimationUtility.SetAnimationClipSettings( clip , settings );
        }
    }
    private void CompleteDefenderEvent()
    {
        StartCoroutine( DefenderCo() );
    }
    private IEnumerator DefenderCo()
    {      

        thisAnim.SetBool( "isBack" , false );
        thisAnim.gameObject.GetComponent<Collider>().isTrigger = false;
        yield return new WaitForSeconds( 7.0f );
        UnLooping( thisAnim , false );
        //yield return new WaitForSeconds( 2.0f );
    }
}
