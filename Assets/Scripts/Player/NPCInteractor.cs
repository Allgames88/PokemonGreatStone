using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractor : MonoBehaviour
{
    //Variable declarations
    public GameObject GeneralData;
    public GeneralData general;
    public Player_Movement playerMovement;
    public GameObject DialogManager;
    public GameObject oppDialogManager;
    public Vector2 Dir;
    public GameObject victim;
    public int CentTimesPress = 0;
    



    // Start is called before the first frame update
    void Start()
    {
        //we get the general options.
        GeneralData = GameObject.Find("GameManajer");
        general = GeneralData.GetComponent<GeneralData>();
        //Get The player Movement
        playerMovement = gameObject.GetComponent<Player_Movement>();
        //Get the Dialog Manager
        DialogManager = GameObject.Find("DialogManager");
        oppDialogManager = GameObject.Find("combatDialogManager");
        
        

    }

    // Update is called once per frame
    void Update()
    {
        playerMovement = gameObject.GetComponent<Player_Movement>();
            //If the player can move
            if(playerMovement.canMove){
                
                //When the action key is pressed, we will send a message to he NPC to do its thing.
                if(Input.GetKeyDown(general.actionKey) && CentTimesPress == 0){
                    CentTimesPress++;
                    //Depending on the direccion the player is looking. We will make a Raycast to different points.
                    if(playerMovement.Look == "up"){
                        Dir = new Vector2(0,0.16f);
                    }else if(playerMovement.Look == "down"){
                        Dir = new Vector2(0,-0.16f);
                    }else if(playerMovement.Look == "right"){
                        Dir = new Vector2(0.16f,0);
                    }else if(playerMovement.Look == "left"){
                        Dir = new Vector2(-0.16f,0);
                    }else{
                        Dir = new Vector2(0,0.16f);
                    }
                    //We do the raycast.
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, Dir, 0.16f, playerMovement.obstacles);
                    //If it hits an NPC.
                    if(hit){
                        if(hit.collider.tag == "NPC"){
                            //We send him a message.
                            victim = hit.transform.gameObject;
                            victim.SendMessage("speak");
                        }
                    }
                    
                }else{
                    //This is only a centinel.
                    CentTimesPress = 0;
                }
            }

        if(DialogManager.GetComponent<DialogManager>().talking == true || oppDialogManager.GetComponent<DialogManager>().talking == true || general.GetComponent<GeneralData>().inCombat == true){
            playerMovement.canMove = false;
        }else if(DialogManager.GetComponent<DialogManager>().talking == false && oppDialogManager.GetComponent<DialogManager>().talking == false && general.GetComponent<GeneralData>().inCombat == false){
            playerMovement.canMove = true;
        }

            
    }

}
