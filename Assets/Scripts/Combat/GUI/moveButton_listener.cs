using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class moveButton_listener : MonoBehaviour
{

    //Public Variable declaration
    public Movement move;
    public typeData type;
    public int colorDif;
    public bool mouseOn;
    public PokemonListener listener;
    public SpriteRenderer OverlayOne;
    public SpriteRenderer OverlayTwo;
    public List<GameObject> hidingObjects;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI PP;
    public GeneralData general;

    //Private variable declaration.
    Color32 pressedColor;

    // Start is called before the first frame update
    void Start()
    {
        general = GameObject.Find("GameManajer").GetComponent<GeneralData>();
    }

    // Update is called once per frame
    void Update()
    {
            /*
                ------------    LOGS    ------------
            */

        //If there is a movement stored:
        if(move.type != null && move.type != ""){
            //Get the type data:
            type = Functions.getTypeByName(move.type);
            //Color change:
                //If The mouse is not over the button:
                if(!mouseOn){
                    OverlayOne.color = new Color32((byte) type.color[0], (byte) type.color[1], (byte) type.color[2],(byte) type.color[3]);
                    OverlayTwo.color = new Color32((byte) type.colorB[0],(byte) type.colorB[1],(byte) type.colorB[2],(byte) type.colorB[3]);
                //On the other hand, if the mouse is over the button:
                }else{
                    List<int> RGBs = new List<int>();
                    RGBs.Add(type.color[0]-colorDif);
                    RGBs.Add(type.color[1]-colorDif);
                    RGBs.Add(type.color[2]-colorDif);
                    RGBs.Add(type.colorB[0]-colorDif);
                    RGBs.Add(type.colorB[1]-colorDif);
                    RGBs.Add(type.colorB[2]-colorDif);

                    //Check for the colors to not be lesser than 0
                    for(int x = 0; x < RGBs.Count; x++){
                        if(RGBs[x] < 0){
                            RGBs[x] = 0;
                        }
                    }
                    OverlayOne.color = new Color32((byte) RGBs[0], (byte) RGBs[1], (byte) RGBs[2],(byte) type.color[3]);
                    OverlayTwo.color = new Color32((byte) RGBs[3],(byte) RGBs[4],(byte) RGBs[5],(byte) type.colorB[3]);
                }
                    
                foreach (GameObject item in hidingObjects){
                    SpriteRenderer spr = item.GetComponent<SpriteRenderer>();
                    spr.enabled = true;
                }

            //Update name and so...

            if(general.lang == "spanish"){
                Name.text = move.traduction.name.spanish;
            }else if(general.lang == "english"){
                Name.text = move.traduction.name.english;
            }

            PP.text = ("Pp: " + move.actualPP + "/" + move.pp);
            
        }else{
            //If not, i think i might make the button blank, or dissappear...
                OverlayOne.color = new Color32(50, 50, 50, 50);
                OverlayTwo.color = new Color32(50, 50, 50, 50);
                Name.text = "";
                PP.text = "";
                foreach (GameObject item in hidingObjects){
                    SpriteRenderer spr = item.GetComponent<SpriteRenderer>();
                    spr.enabled = false;
                }
        }
        
    }

    //When mouse is over, set bool to true:
    private void OnMouseEnter() {
        mouseOn = true;
    }

    //When mouse exits, set bool to false:
    private void OnMouseExit() {
        mouseOn = false;
    }

    //When clicks, if the mouse is over the button:
    private void OnMouseDown() {
        //If clicked and well configured, tell the father to exeute an specified function.
        Debug.Log("Hey, it works!");
        transform.parent.gameObject.SendMessage("MovementMenu");
        mouseOn = false;
        listener.action = move;
        

    }
}

