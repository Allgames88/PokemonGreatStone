using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/*
====================   Class Declarations   =======================================
*/
    //We create a class where to storage the data from the json file.
    [System.Serializable]
    public class GrassGroup{
        public int NumOfFights;
        public List<pokes> content;
    }

    //We create a class where to storage the array from the content in the JSON file.
    [System.Serializable]
    public class pokes{
        public string ID;
        public int percentage;
        public int maxlevel;
        public int minlevel;
        public int level;
    }

/*
================================================================================
*/

public class GrassManager : MonoBehaviour /* ======= Main Class =======*/
{   
    /*======== Variable declarations ========*/
    public GameObject player;
    public string path;
    public string ActualPath;
    public string contents;
    public int NumOf;
    public int round;

    public int StepsToUpdate;
    private int ActualStepsToUpdate;
    private int lastStepsCounted;

    // Start is called before the first frame update
    void Start()
    {   
        //PLayer.
        player = GameObject.Find("Character");
        
        //We get the location of the Json that configures the group of grass
        ActualPath = Application.streamingAssetsPath + path; /*In a future, the Application.dataPath will be a static variable in GeneralData*/
        //We call the function to assign pokemons to the sons of this object
        UpdateGrass();

    }

    //Function called each tick
    private void Update() {
        //Gets player's steps.
        lastStepsCounted = player.GetComponent<Player_Movement>().steps;

        //If the player has walked enough, reroll the wild pokemons;
        if(lastStepsCounted >= ActualStepsToUpdate){
            UpdateGrass();
        }
    }


    public void UpdateGrass(){
        lastStepsCounted = player.GetComponent<Player_Movement>().steps;
        ActualStepsToUpdate = lastStepsCounted + StepsToUpdate;
        //This is a counter, to know how many times, has the pokemon benn rerolled.
        round++;
        //This variable is used to count how many pokemon has been already added to the grass.
        int acumulation = 0;
        //We read all the content inside of that json file and transform it into a class.
        contents = File.ReadAllText(ActualPath);
        GrassGroup group;
        group = JsonUtility.FromJson<GrassGroup>(contents);
        //We start getting the number of fights that we can find in this group of grass.
        NumOf = group.NumOfFights;
        //Now we get all the grasses inside of this object.
        Transform[] Grasses = gameObject.GetComponentsInChildren<Transform>();
        //As long as there are still unassigned pokemon, the bucle will repeat.
        while (acumulation < NumOf)
        {
            //This bucle will be repeated for each possible pokemon in the group of grass.
            //For each Transform, in the list of transforms. BUT ITS getting all the transforms, including self..
            foreach (pokes item in group.content)
            {
                //This bucle will be repeated for each single object of grass in the group of grass.
                foreach(Transform child in Grasses){
                    //If the object inside the group of grass its called "Grass", and the max number of pokemons hasnt already been choosen, the code will be executed.
                    //And of course, the  transform choosen its not from this object.
                    if(NumOf > acumulation && child.gameObject.GetComponent<GrassManager>() == null){
                        //Will execute code only on objects that has the SingleGrass script.
                        if(child.gameObject.GetComponent<SingleGrass>()){

                            if(child.gameObject.GetComponent<SingleGrass>().round < round){
                                //Remove the pokemon from the grass if it has one.
                                child.gameObject.GetComponent<SingleGrass>().ID.Clear();
                                child.gameObject.GetComponent<SingleGrass>().level = 0;
                            }
                            //Gets the wild pokemon found in the grass;
                                /*pokes wildPokemon = new pokes();                      This code used to convert the json into objects, i dont need it now.
                                wildPokemon = JsonUtility.FromJson<pokes>(item);*/

                            pokes wildPokemon = item;
                            //Calculates the probability to add the pokemon to the grass.
                            float a = Random.Range(0, 100);
                            //If the Grass its been chosen to have a pokemon, an ID, and a Level will be added to the Grass
                            if(a <= wildPokemon.percentage ){
                                child.gameObject.GetComponent<SingleGrass>().ID.Clear();
                                //Just add an ID.
                                    child.gameObject.GetComponent<SingleGrass>().ID.Add(wildPokemon.ID);
                                
                                child.gameObject.GetComponent<SingleGrass>().level = Random.Range(wildPokemon.maxlevel, wildPokemon.minlevel);
                                child.gameObject.GetComponent<SingleGrass>().round = round;
                                acumulation++;
                            }
                        }
                    }
                }
            }

            //=========================   Debugs   =========================================
            //Debug.Log(group.content[0]);

            //==============================================================================
        }
        
    }

}


