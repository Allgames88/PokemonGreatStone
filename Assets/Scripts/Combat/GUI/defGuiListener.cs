using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class defGuiListener : MonoBehaviour
{
    public Vector3 hidenPos;
    public Vector3 revealPos;
    public bool hidden;
    public bool debug;
    public int speed;

    private void Update() {
        if(debug){
            Debug.Log("posX: "+transform.position.x+" posY: "+transform.position.y);
            debug = false;
        }
    }

    //Function to reveal the combat opponent GUI.
    public void Reveal(){
        StartCoroutine(actualReveal());
        
    }

    //X: -6.7f
    public IEnumerator actualReveal(){
        if(speed <= 0){
            while (transform.position != revealPos){
                transform.position = Vector2.MoveTowards(transform.position,revealPos,9f*Time.deltaTime);
                yield return new WaitForSeconds(0.0001f);
            }
        }else{
            while (transform.position != revealPos){
                transform.position = Vector2.MoveTowards(transform.position,revealPos,speed*Time.deltaTime);
                yield return new WaitForSeconds(0.0001f);
            }
        }
        yield return new WaitForSeconds(0.0001f);
        hidden = false;
        
        
    }

    //Function to hide the combat GUI.
    public void Hide(){
        StartCoroutine(actualHide());
        
    }

    //X: -10.8f Y: 1.2f
    public IEnumerator actualHide(){
        if(speed <= 0){
            while (transform.position != hidenPos){
                transform.position = Vector2.MoveTowards(transform.position,hidenPos,9f*Time.deltaTime);
                yield return new WaitForSeconds(0.0001f);
            }
        }else{
            while (transform.position != hidenPos){
                transform.position = Vector2.MoveTowards(transform.position,hidenPos,speed*Time.deltaTime);
                yield return new WaitForSeconds(0.0001f);
            }
        }
        yield return new WaitForSeconds(0.0001f);
        hidden = true;
    }
}