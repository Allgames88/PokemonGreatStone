using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentGUI : MonoBehaviour
{

    //Function to reveal the combat opponent GUI.
    public void Reveal(){
        StartCoroutine(actualReveal());
    }

    public IEnumerator actualReveal(){
        while (transform.position.x < -6.7f){
            transform.position = new Vector2(transform.position.x+9f*Time.deltaTime,1.2f);
            yield return new WaitForSeconds(0.0001f);
        }
        
    }

    //Function to hide the combat GUI.
    public void Hide(){
        StartCoroutine(actualHide());
    }

    public IEnumerator actualHide(){
        while (transform.position.x > -10.8f){
            transform.position = new Vector2(transform.position.x-9f*Time.deltaTime,1-2f);
            yield return new WaitForSeconds(0.0001f);
        }
    }
}
