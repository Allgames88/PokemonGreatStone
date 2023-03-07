using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;

public class GuiListener : MonoBehaviour
{

    public bool HidenCent;
    public int movesState = 0;
    public GameObject children;
    public List<GameObject> buttons;
    public Sprite red_button;
    public Sprite blue_button;
    public Sprite poke_button;
    public Sprite gray_button;
    public Sprite blank_button;
    public int moveCount;
    PokemonListener pokemonListener;
    public GeneralData general;
    private int colorcent;
    Pokemon poke;

 

    // Start is called before the first frame update
    void Start()
    {
        pokemonListener = GameObject.Find("MyPoke").GetComponent<PokemonListener>();
        poke = pokemonListener.pokemon;
        
        general=GameObject.Find("GameManajer").GetComponent<GeneralData>();
        Hide();
        if(Functions.FindSpecificChild(gameObject, "Atack Button") != null){
            buttons.Add(Functions.FindSpecificChild(gameObject, "Atack Button"));
            buttons.Add(Functions.FindSpecificChild(gameObject, "Item Button"));
            buttons.Add(Functions.FindSpecificChild(gameObject, "Pokemon Button"));
            buttons.Add(Functions.FindSpecificChild(gameObject, "Run Button"));
            buttons.Add(Functions.FindSpecificChild(gameObject, "Cancel Button"));
            

        }
    }

    void Update()
    {
        
        
        if(movesState == 0){
            
            colorcent=0;

            foreach(GameObject gam in buttons){
                moveCount=0;
                Color32 newColor = new Color32(255,255,255,255);
                //Variable delcaration
                SpriteRenderer spr = gam.GetComponent<SpriteRenderer>();
                GameObject textObject = Functions.FindSpecificChild(gam, "Text");
                GameObject ppObject = Functions.FindSpecificChild(gam, "pps");
                TextMeshPro tmp = textObject.GetComponent<TextMeshPro>();
                ButtonAsChildren bac = gam.GetComponent<ButtonAsChildren>();
                //If the button is the Atack button, set its sprite and text.
                if(gam.name == "Atack Button" && gam.GetComponent<SpriteRenderer>().sprite != red_button){
                    spr.sprite = red_button;
                    bac.action = "MovementMenu";
                    tmp.SetText("Fight");
                    spr.color = newColor;
                }else if(gam.name == "Item Button" && gam.GetComponent<SpriteRenderer>().sprite != blue_button){
                    spr.sprite = blue_button;
                    bac.action = "OpenItems";
                    tmp.SetText("Items");
                    spr.color = newColor;
                }else if(gam.name == "Pokemon Button" && gam.GetComponent<SpriteRenderer>().sprite != poke_button){
                    spr.sprite = poke_button;
                    bac.action = "OpenPokemonMenu";
                    tmp.SetText("Pokemon");
                    spr.color =newColor;
                }else if(gam.name == "Run Button" && gam.GetComponent<SpriteRenderer>().sprite != gray_button){
                    spr.sprite = gray_button;
                    bac.action = "RunFromFight";
                    tmp.SetText("Run");
                    spr.color = newColor;
                }else if(gam.name == "Cancel Button" && ppObject == null){
                    bac.action = "MovementMenu";
                    tmp.SetText("");
                    gam.SetActive(false);
                    spr.color = bac.orColor;
                }
                if(gam.name != "Cancel Button"){
                    ppObject.SetActive(false);
                }

                    
                
            }

        }else{
            
            foreach(GameObject gam in buttons){
                if(moveCount == 4){
                    moveCount = 0;
                }

                try{

                    SpriteRenderer spr = gam.GetComponent<SpriteRenderer>();
                    GameObject textObject = Functions.FindSpecificChild(gam, "Text");
                    GameObject ppObject = Functions.FindSpecificChild(gam, "pps");
                    TextMeshPro PPtmp = ppObject.GetComponent<TextMeshPro>();
                    TextMeshPro tmp = textObject.GetComponent<TextMeshPro>();
                    ButtonAsChildren bac = gam.GetComponent<ButtonAsChildren>();

                    if(poke.builtStats.moves[moveCount] != null){
                        Color32 newColor = new Color32();
                        if(poke.builtStats.moves[moveCount].type == "Poison"){
                            newColor = new Color32(204,0,204,255);
                        }
                        if(poke.builtStats.moves[moveCount].type == "Bug"){
                            newColor = new Color32(153,255,51,255);
                        }
                        if(poke.builtStats.moves[moveCount].type == "Grass"){
                            newColor = new Color32(51,255,51,255);
                        }
                        if(poke.builtStats.moves[moveCount].type == "Normal"){
                            newColor = new Color32(255,255,255,255);
                        }
                        if(poke.builtStats.moves[moveCount].type == "Psiquic"){
                            newColor = new Color32(255,51,153,255);
                        }
                        if(poke.builtStats.moves[moveCount].type == "Fire"){
                            newColor = new Color32(255,51,51,255);
                        }
                        if(poke.builtStats.moves[moveCount].type == "Water"){
                            newColor = new Color32(51,153,255,255);
                        }
                        if(poke.builtStats.moves[moveCount].type == "Electric"){
                            newColor = new Color32(255,255,51,255);
                        }
                        if(poke.builtStats.moves[moveCount].type == "Ghost"){
                            newColor = new Color32(102,0,102,255);
                        }
                        if(poke.builtStats.moves[moveCount].type == "Dark"){
                            newColor = new Color32(67,53,43,255);
                        }
                        if(poke.builtStats.moves[moveCount].type == "Fighting"){
                            newColor = new Color32(200,55,16,255);
                        }
                        if(poke.builtStats.moves[moveCount].type == "Fairy"){
                            newColor = new Color32(255,55,255,255);
                        }
                    
                        
                        if(gam.name == "Cancel Button"){
                            bac.action = "MovementMenu";
                            tmp.SetText("");
                            gam.SetActive(true);
                        }else{
                            spr.sprite = blank_button;
                            ppObject.SetActive(true);
                            if(poke.builtStats.moves[moveCount] != null){
                                if(general.lang == "spanish"){
                                    tmp.SetText(poke.builtStats.moves[moveCount].traduction.name.spanish);
                                }else if (general.lang == "spanish"){
                                    tmp.SetText(poke.builtStats.moves[moveCount].traduction.name.english);
                                }else{
                                    tmp.SetText("--");
                                }

                                PPtmp.SetText(poke.builtStats.moves[moveCount].actualPP + "/" + poke.builtStats.moves[moveCount].pp);
                                if(colorcent == 0){
                                    spr.color = newColor;
                                }
                                bac.orColor=newColor;
                                
                            }else{
                                tmp.SetText("--");
                            }
                            bac.action = "OpenItems";
                        }
                    }else{

                        spr.sprite = blank_button;
                        tmp.SetText("--");
                        bac.action = "OpenItems";
                    }
                }
                catch (Exception e){
                    if(gam.name == "Cancel Button"){
                        Debug.Log("Got here on "+gam.name);
                        SpriteRenderer spr = gam.GetComponent<SpriteRenderer>();
                        GameObject textObject = Functions.FindSpecificChild(gam, "Text");
                        TextMeshPro tmp = textObject.GetComponent<TextMeshPro>();
                        ButtonAsChildren bac = gam.GetComponent<ButtonAsChildren>();

                        bac.action = "MovementMenu";
                        tmp.SetText("");
                        gam.SetActive(true);
                    }
                    
                }
                moveCount++;
                
                
            }
            moveCount=0;
            colorcent++;

        }
    }

    //Function to hide the combat GUI.
    public void Hide(){
        if(!HidenCent){
            StartCoroutine(actualHide());
            HidenCent = true;
            movesState = 0;
        }
    }

    public IEnumerator actualHide(){
        while (transform.position.x < 3.7f){
            transform.position = new Vector2(transform.position.x+9f*Time.deltaTime,2f);
            yield return new WaitForSeconds(0.0001f);
        }
    }

    //Function to reveal the combat GUI.
    public void Reveal(){
        StartCoroutine(actualReveal());
    }

    public IEnumerator actualReveal(){
        while (transform.position.x > -4.7f){
            transform.position = new Vector2(transform.position.x-9f*Time.deltaTime,0.3f);
            yield return new WaitForSeconds(0.0001f);
        }
        HidenCent = false;
    }
        
        

    //These four functions are just so the buttons can access them.
    /*********************************************/
    public void OpenItems(){
        Functions.OpenItems();
    }

    public void MovementMenu(){
        if(movesState == 0){
            movesState = 1;
        }else if(movesState == 1){
            movesState = 0;
        }else if(movesState == 2){
            movesState = 1;
        }
    }

    public void RunFromFight(){
        Functions.RunFromFight();
    }

    public void OpenPokemonMenu(){
        Functions.OpenPokemonMenu();
    }
    /*********************************************/
}
