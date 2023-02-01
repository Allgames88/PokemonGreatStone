using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleGrass : MonoBehaviour
{
    //Variable declarations for pokemon fights. I might put this in a static class.
    public List<string> ID;
    public int level;

    //The player.
    public GameObject player;

    /*METHOD 1: Doesnt work.
        All the objects in a positive position move to 0 coordinate, and all the ones in a negative position, doesnt move, the bucles doesnt detect the numbers multiple of 0.16.
    */
    //<!ERASED>

    /*METHOD 2: GRID CODE SNAP
        We get the grid via code, and align the objects to the local position of the cell, really usfeull.
    */

    //VAriable declarations
    public GameObject GridContainer;
    public Grid grid;
    public GameObject BottomGrass;
    //The sprite renderer of the bottom leaves.
    public SpriteRenderer BTspR;
    public SpriteRenderer spR;

    //Variable used to know the differece between pokemons asigned in the last reroll, and this reroll;
    public int round;

    // Start is called before the first frame update
    void Start()
    {
        //Get the player.
        player = GameObject.Find("Character");

        //Get the grid
        GridContainer = GameObject.Find("Grid");
        grid = GridContainer.GetComponent<Grid>();
        //We get the cell position.
        Vector3Int cellPosition = grid.WorldToCell(transform.position);
        //We set the grass to that position.
        transform.position = grid.GetCellCenterWorld(cellPosition);
        //We low the grass 0.02f.
        transform.position = new Vector2(transform.position.x, transform.position.y-0.02f);
        //Get the sprite renderer.
        spR = gameObject.GetComponent<SpriteRenderer>();

        //Get the bottom part of the grass. This is used to control when the player steps on, or under the grass.
        BottomGrass = Functions.FindSpecificChild(this.gameObject, "ShorGrass");

        //Sprite renderer from the lower leaves.
        BTspR = BottomGrass.GetComponent<SpriteRenderer>();
    }/*Works*/

    void FixedUpdate() {
        //This is used to change the sorting layer from the leaves.
        if(Vector2.Distance(player.transform.position, transform.position) <= 0.32){
            //I dont know how, but looks like the sprite renderer from this gameObject, is now the sprite renderer from the bottom leaves, and upside down.
            if(player.transform.position.y >= transform.position.y){
                spR.sortingOrder = 1;
            }else{
                spR.sortingOrder = -1;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        //When character collides with this grass, then we will log the pokemon inside.
        if(other.gameObject.name == "Character"){
            if(ID[0] != "" && ID[0] != null){
                //This will be used for detecting future events.
                teamMessage data = new teamMessage();
                data.IDs = ID;
                data.level = level;
                player.SendMessage("InitFight", data, SendMessageOptions.DontRequireReceiver);
                ID.Clear();
                level = 0;

            }
        }
    }

}

public class teamMessage{
    public List<string> IDs;
    public int level;
}