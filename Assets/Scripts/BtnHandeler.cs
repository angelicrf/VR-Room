using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnHandeler : MonoBehaviour
{
    public GameObject thisText;
    public Button thisBtn;
    private void Update()
    {
        NewOnClick();
    }
    public void NewOnClick()
    {
        thisBtn.onClick.AddListener( delegate () {
            Debug.Log( "StartedClicked" );
            thisText.gameObject.SetActive( true );
        } );
    }
}
