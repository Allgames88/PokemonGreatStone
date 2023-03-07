using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


/*
    ====================   Class Declarations   =======================================
    */
    [System.Serializable]
    public class Lista{
        public List<string> list;
    }


    //A class to store the definied conversation of the npc.
    [System.Serializable]
    public class dialog{
        public string name;
        public List<Lista> content;
    }


    /*
    ================================================================================
*/

public class defNPC : MonoBehaviour
{
    



    //Variable declarations
    public GameObject GridContainer;
    public GameObject DialogManager;
    public Grid grid;
    public string jsonPath;
    public int count;
    public GameObject player;
    public GeneralData general;


     //We make an object where store the data from the dialog json.
        public dialog dial;

    // Start is called before the first frame update
    void Start()
    {

        //IMPORTANT: Get the player.
        player = GameObject.Find("Character");

        //We make sure to make a new object.
        dial = new dialog();

        //We get the grid object, and its grid component.
        GridContainer = GameObject.Find("Grid");
        grid = GridContainer.GetComponent<Grid>();
        DialogManager = GameObject.Find("DialogManager");

        //We move the npc, to the center of its closer grid cell.
        Vector3Int cellPosition = grid.LocalToCell(transform.localPosition);
        transform.localPosition = grid.GetCellCenterLocal(cellPosition);
        transform.position = new Vector2(transform.position.x, transform.position.y);

        //Count starts as 0.
        count = 0;
        //We store it in the object;
        dial = Functions.getDialog(jsonPath);



    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, player.transform.position) <= 0.32f){
            //Function used to know when object its supposed to be over or under the player, or an specific object.
            Functions.ControlProfundityWith(this.gameObject, player);
        }
    }
    //Function that will say whatever the NPC have to say.
    void speak(){
        if(dial.content.Count > 0){
            //This will make the bucle of dialogs active, setting count to 0 when it reaches the count of words in the list.
            if(count >= dial.content.Count){
                count = 0;
            }
            
            //If it have something to say, it will say  it.
            if(dial.content[0].list.Count != 0){
                //If there is a dialogManager:
                if(GameObject.Find("DialogManager") != null){
                    //We just send to the NPC the texts it have to say.
                    Lista newContent = dial.content[count];
                    DialogManager.SendMessage("speak",newContent.list);
                    count++;
                }else{

                }
            //However, if it doesnt have to say anything.
            }else{
                Debug.Log("No tiene nada que decir");
            }

            
        }else{
            Debug.Log("No tiene nada que decir");
        }
        
    }
}
