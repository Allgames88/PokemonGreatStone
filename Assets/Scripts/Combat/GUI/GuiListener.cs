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
    public List<GameObject> battleButtons;
    public int moveCount;
    PokemonListener pokemonListener;
    public GeneralData general;
    Pokemon poke;
    public GameObject prefab;

 

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
            
            //We are about to change all of this so... dont know, there is goind to be lots of errors...
            /*
                First, i need the prefab of a combat button, text, pp, and personal class... It will also
                have the script Button_as_children, so when the player presses the button, it will send a message here, and from here to the combatManager.

                When moveState is 1, hide the normal buttons, and display the combat ones, its easier than what i did.

                I might need to do a new script, the moveButton_listener, so the button changes depending on the movement it has inside...
            */

            foreach(GameObject gam in buttons){
                moveCount=0;
                Color32 newColor = new Color32(255,255,255,255);
                //Variable delcaration
                SpriteRenderer spr = gam.GetComponent<SpriteRenderer>();
                GameObject textObject = Functions.FindSpecificChild(gam, "Text");
                GameObject ppObject = Functions.FindSpecificChild(gam, "pps");
                TextMeshPro tmp = textObject.GetComponent<TextMeshPro>();
                ButtonAsChildren bac = gam.GetComponent<ButtonAsChildren>();
                if(gam.name == "Cancel Button" && ppObject == null){
                    bac.action = "MovementMenu";
                    tmp.SetText("");
                    gam.SetActive(false);
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
