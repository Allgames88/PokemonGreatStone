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
    public bool goHide;
    public bool goReveal;
    public int turn;

    private void Update() {
        if(debug){
            Debug.Log("posX-local: "+transform.localPosition.x+" posY-local: "+transform.localPosition.y);
            Debug.Log("Must move towards - posX: "+revealPos.x+" posY: "+revealPos.y);
            debug = false;
        }

        if(goHide){
            Hide();
            goHide = false;
        }

        if(goReveal){
            Reveal();
            goReveal = false;
        }
    }

    //Function to reveal the combat opponent GUI.
    public void Reveal(){
        StopAllCoroutines();
        StartCoroutine(actualReveal());

    }

    //X: -6.7f
    public IEnumerator actualReveal(){
        hidden = false;
        if(speed <= 0){
            while (transform.position != revealPos){
                transform.position = Vector2.MoveTowards(transform.position,revealPos,10f*Time.deltaTime);
                yield return new WaitForSeconds(0.0001f);
            }
        }else{
            while (transform.position != revealPos){
                transform.position = Vector2.MoveTowards(transform.position,revealPos,speed*Time.deltaTime);
                yield return new WaitForSeconds(0.0001f);
            }
        }
        yield return new WaitForSeconds(0.0001f);
        
        
        
    }

    //Function to hide the combat GUI.
    public void Hide(){
        StopAllCoroutines();
        StartCoroutine(actualHide());
    }

    //X: -10.8f Y: 1.2f
    public IEnumerator actualHide(){
        hidden = true;
        if(speed <= 0){
            while (transform.position != hidenPos){
                transform.position = Vector2.MoveTowards(transform.position,hidenPos,10f*Time.deltaTime);
                yield return new WaitForSeconds(0.0001f);
            }
        }else{
            while (transform.position != hidenPos){
                transform.position = Vector2.MoveTowards(transform.position,hidenPos,speed*Time.deltaTime);
                yield return new WaitForSeconds(0.0001f);
            }
        }
        yield return new WaitForSeconds(0.0001f);
        
    }

    IEnumerator stopAll(){
        yield return new WaitForSeconds(2f);
        StopAllCoroutines();
    }
}