using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public GameObject reciever;
    public string action;
    Vector3 mousePosition;
    public Color32 orColor;
    public string paramOne;
    

    private void OnMouseDown() {
        //If clicked and well configured, tell the father to exeute an specified function.
        if(reciever != null && reciever && action != null && action != ""){
            Color32 white = new Color32(255,255,255,255);
            gameObject.GetComponent<SpriteRenderer>().color = white;
            reciever.SendMessage(action);
        } 
    }

    //When mouse is over, dark a little.
    private void OnMouseEnter() {
        //Get the normal color
        orColor = gameObject.GetComponent<SpriteRenderer>().color;
        //Make a new color to store.
        Color32 newColor = new Color32(255,255,255,255);
        //Save the darker red color into an integer
        int color = orColor.r-60;
        //If the integer is lesser than 0 set to 0.
        if(color < 0){
            color = 0;
        }
        //Asign the color to the new color.
        newColor.r = (byte)color;
        //Repeat.
        color = orColor.b-60;
        if(color < 0){
            color = 0;
        }
        newColor.b = (byte)color;
        color = orColor.g-60;
        if(color < 0){
            color = 0;
        }
        newColor.g = (byte)color;
        
        //Assign the button the new darker color.
        gameObject.GetComponent<SpriteRenderer>().color = newColor;
    }

    //When mouse exits the button, return to normal color.
    public void OnMouseExit() {
        gameObject.GetComponent<SpriteRenderer>().color = orColor;
    }
    
}
