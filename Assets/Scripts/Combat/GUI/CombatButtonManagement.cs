using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatButtonManagement : MonoBehaviour
{

    public CombatButtonManagement good_cop;
    public CombatButtonManagement bad_cop;
    public bool BadCop;
    public bool moveState;
    public PokemonListener listener;
    public Combat_Manager manager;
    GeneralData general;
    public List<GameObject> moveButtons;
    // This function will run only once, when the object is instantiated, or if its active, at the start.
    void Start() {
        general = GameObject.Find("GameManajer").GetComponent<GeneralData>();
        listener = manager.myPoke;
        if(BadCop){
            gameObject.SetActive(false);
        }
        
    } 

    //
    // Update is called every frame, if the MonoBehaviour is enabled.
    //
    void Update()
    {
        //When pressed the movement buttons, or the cancel button, do things or not.
        //When its time to show the movement buttons.
        if(moveState){
            //If this object is the one that has the movements:
            //  -- Disable the normal buttons.
            //If its not:
            //  -- Enable the buttons.
            if(BadCop){
                good_cop.transform.gameObject.SetActive(false);
            }else{
                bad_cop.transform.gameObject.SetActive(true);
            }
        }else{

            //That way, when the bad cop is inactive, the good cop can wake it up, and when the good cop is inactive, the bad cops wakes it up.
            if(BadCop){
                good_cop.transform.gameObject.SetActive(true);
            }else{
                bad_cop.transform.gameObject.SetActive(false);
            }
        }


        //Now, if this is the bad cop, (The one with the movements.), we have to update the buttons to show the data of the movements.
        //But here are some questions first:
            /* 1º 
                If we dont want to make a new variable for each button, how do we access them?

                Solution 1: Array of buttons (object 0, to movement 0, and so...).
                Solution 2: Raw-coding.
            */
        if(BadCop){
            for(int x = 0; x < moveButtons.Count; x++){
                if(listener.pokemon.builtStats.moves[x] != null){
                    moveButtons[x].GetComponent<moveButton_listener>().move = listener.pokemon.builtStats.moves[x];
                }
                
            }
        }


    }




    //Function to switch between buttons...
    void MovementMenu(){
        if(moveState){
            moveState = false;
            if(BadCop){
                good_cop.moveState = false;
            }else{
                bad_cop.moveState = false;
            }
        }else{
            moveState = true;
            if(BadCop){
                good_cop.moveState = true;
            }else{
                bad_cop.moveState = true;
            }
        }
    }

    void RunFromFight(){
        string reason = "flee";
        manager.transform.gameObject.SendMessage("FinishCombatCamera",reason);
    }
}
