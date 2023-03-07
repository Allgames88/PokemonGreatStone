using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;



public class Player_combat_detector : MonoBehaviour
{
    //Variable delaration.
        //Cameras
        public Camera combatCam;
        public GameObject combatCamera;
        public GameObject revealCamera;

        //GameObjects
        public GameObject player;
        public GeneralData general;
        public Combat_Manager combatManager;
        public List<GameObject> opponents;
        public List<GameObject> wild_particles;
        public GameObject myPoke;
        public GameObject fieldTop;
        public GameObject CombatGUI;
        public GameObject oppCombatGUI;
        public GameObject oppDialogManajer;
        public GameObject in_combat_character;
        public GameObject myBall;
        public Vector3 myBallPos;
        //Others
        public bool positionedTop;
        public bool darkOutCalled;
        public bool canBattle;




    // Start is called before the first frame update
    void Start()
    {
        GameObject BattleGUI = GameObject.Find("BattleUI");
        combatManager = GameObject.Find("GameManajer").GetComponent<Combat_Manager>();
        combatCam.enabled=false;
        player = GameObject.Find("Character");
        general = GameObject.Find("GameManajer").GetComponent<GeneralData>();
        myPoke = GameObject.Find("MyPoke");
        fieldTop = GameObject.Find("battlefield_up");
        CombatGUI = GameObject.Find("CombatGUI");
        positionedTop = false;
        myBall = GameObject.Find("myBall");
        myBallPos = myBall.transform.position;
        canBattle = true;
        //Position ofcombat: -2.7
        //Position inCombat: -4.6
    }

    //Function to start fights.
    //1º: The ID of the opponent pokemon.
    //2º: The Level of the opponent pokemon.
    public void InitFight(teamMessage data){
        if(canBattle){
            canBattle = false;
            Debug.Log("Fight initiated");
            general.inCombat = true;
            
            
            //An id, or a group of them is needed.
            if(data.isTrainer){
                //Do things if its a trainer, its to test when npc's are finallized.
            }else{
                //Variable pokemon.
                Pokemon loadedPokemon = new Pokemon();
                //Gets pokemon path.
                string path = Application.streamingAssetsPath + "/Pokemons/"+data.ID;
                Debug.Log(path + " exists?: "+ Directory.Exists(path));
                //If the path exists, loads the opponent
                if(Directory.Exists(path)){
                    Debug.Log("The pokemon to reveal is a: " +data.ID);
                    loadedPokemon = Functions.FindPokemonByID(data.ID);
                    loadedPokemon.Level = data.level;
                    loadedPokemon = Functions.BuildPokemon(loadedPokemon);
                    //revealOpponent(data.ID, data.level);
                }else{
                    string errorBackup = "MissingNo";
                    loadedPokemon = Functions.FindPokemonByID(errorBackup);
                    loadedPokemon.Level = 1;
                    loadedPokemon = Functions.BuildPokemon(loadedPokemon);
                    //revealOpponent(errorBackup, 1);
                }

                //Enable the combat Manager
                GameObject CombatManager = GameObject.Find("BattleManager");
                CombatManager.SendMessage("switch_combat_mode",loadedPokemon);
                
            }
            
            StartCoroutine(StartCombatCamera(1));
        }
    }

    //Function used to change the cameras, between the player, and the fighting stage.
    public IEnumerator StartCombatCamera(int seconds){
        /* ------------ We need to hide all elements to set ready all the camera, so the combatManager can play with it.*/
        //Hide combatGUIS, the thing that displays pokemon's stats.
        //oppCombatGUI.SendMessage("Hide",null,SendMessageOptions.DontRequireReceiver);
        CombatGUI.SendMessage("Hide",null,SendMessageOptions.DontRequireReceiver);

        //Hidd the ball. Maybe i should instantiate it whenever i need, i should erase it?
        myBall.GetComponent<SpriteRenderer>().enabled = false;
        
        //Hidd the dialog manager.
        //Hide the text box from the Combat GUI.
        oppDialogManajer.SendMessage("Hide",null,SendMessageOptions.DontRequireReceiver);

        //Hide the player's pokemon.
        myPoke.GetComponent<SpriteRenderer>().enabled = false;

        //Stop player's character animation.
        in_combat_character.GetComponent<Animator>().enabled = false;
        //Update character's sprite.
        in_combat_character.GetComponent<SpriteRenderer>().sprite = general.actualChar.baseInCombat;
        //Reveal player's character's sprite
        in_combat_character.SendMessage("Reveal",null,SendMessageOptions.DontRequireReceiver);
        //oppCombatGUI.SendMessage("Hide",null,SendMessageOptions.DontRequireReceiver);
  
        
        //Wait one second
        yield return new WaitForSeconds(seconds);
        //Switch to the reveal Camera.
        GameObject playerCamera = GameObject.Find("Main Camera");
        StartCoroutine(Functions.CameraSwitch(playerCamera, combatCamera, "rough",2));
        //Move the top.
        fieldTop.SendMessage("Reveal");
        general.camera = "combat";
        //Wait
        yield return new WaitForSeconds(seconds);
        

        /* -------------- From here, the rest of this function will be changed to another class, to another function.
                          It really doesnt make sense of why is this here, i mean, built in the same function that prepares and changes the camera.
        */
        
        /*
            //Examine the opponent.
            StartCoroutine(examinePokemon());
            yield return new WaitForSeconds(seconds*2);
            //Once the pokemon has veen examinated, reveal the text box, saying that there is one opponent.
            oppDialogManajer.SendMessage("Reveal",null,SendMessageOptions.DontRequireReceiver);
            yield return new WaitUntil(() => oppDialogManajer.GetComponent<defGuiListener>().hidden == false);
            List<string> newList = new List<string>();
            newList.Add("Un "+opponents[0].GetComponent<PokemonListener>().pokemon.Name+ " salvaje ha aparecido!");
            newList.Add("Adelante "+myPoke.GetComponent<PokemonListener>().pokemon.Name+ "!");
            StartCoroutine(GUISay(newList,true));
            yield return new WaitUntil(() => oppDialogManajer.GetComponent<defGuiListener>().hidden == true);
            in_combat_character.GetComponent<Animator>().enabled = true;
            in_combat_character.GetComponent<Animator>().Play("Throw",-1,0f);
            yield return new WaitForSeconds(0.8f);
            in_combat_character.SendMessage("Hide",null,SendMessageOptions.DontRequireReceiver);
            yield return new WaitForSeconds(0.2f);
            myBall.GetComponent<SpriteRenderer>().enabled = true;
            StartCoroutine(Functions.ParabolicThrow(myBall,myPoke.transform.position,0.7f));
            yield return new WaitUntil(() => in_combat_character.GetComponent<defGuiListener>().hidden == true);
            in_combat_character.GetComponent<Animator>().enabled = false;
            myPoke.GetComponent<SpriteRenderer>().enabled = true;
            StartCoroutine(Functions.TrainerReveal(myPoke));
            yield return new WaitForSeconds(0.3f);
            myBall.GetComponent<SpriteRenderer>().enabled = false;
            myBall.transform.position = myBallPos;
            yield return new WaitForSeconds(0.5f);
            CombatGUI.SendMessage("Reveal");
            oppCombatGUI.SendMessage("Reveal");
            combatManager.switch_combat_mode();
        */
        
        
        //Move top of the field to -0.04.
        //While the top of the field is not properly placed, fight must not iniciate.
        
    }

    //First parameter is the gameObject to change the texture.
    //Second is the Pokemon ID.
    public void revealOpponent(string ID, int lvl){
            GameObject pokemon = GameObject.Find("Opponent");
            GameObject foundParticles = Functions.FindSpecificChild(pokemon,"Particle System");
            pokemon.transform.parent = fieldTop.transform;
            opponents.Add(pokemon);
            wild_particles.Add(foundParticles);
            
        
            //gets the pokemon.
                Pokemon pokemonData = Functions.FindPokemonByID(ID);
                //Sets the pokemon's lvl.
                pokemonData.Level = lvl;
                //Builds the opponent pokemon stats.
                pokemonData = Functions.BuildPokemon(pokemonData);
                //Assigns the pokemon.
                pokemon.SendMessage("setPokemon",pokemonData);

                //If the player has a pokemon in its team, then set the first aviable.
                if(Functions.rollPlayerPokemon(general.playerTeam) != null){
                    myPoke.SendMessage("setPokemon",Functions.rollPlayerPokemon(general.playerTeam));
                //If not, then 
                }else{
                    Pokemon pokeAlert = Functions.FindPokemonByID("MissingNo");
                    pokeAlert.Level = 1;
                    pokeAlert = Functions.BuildPokemon(pokeAlert);
                    myPoke.SendMessage("setPokemon",pokeAlert);
                }
                
        
            

            if(pokemonData != null){
                Debug.Log("Gets here");
                /*---- Changing the scale to the one required by the Pokemon needed. ----*/
                //Which means, that the scale of certain pokemons, will be bigger or lesser than other's.
                //Make a new pokemon class.
                //If the pokemonData has the scale value
                if(pokemonData.opScale != null && pokemonData.opScale.x > 0 && pokemonData.opScale.y > 0){
                    //Then change the pokemon's scale
                    Vector3 scale = new Vector3(pokemonData.opScale.x, pokemonData.opScale.y, 0);
                    Debug.Log("Scale to set is: " + pokemonData.opScale.x + "x, " + pokemonData.opScale.y + "y");
                    pokemon.transform.localScale = scale;
                    //pokemon.transform.localScale.y = pokeData.opScale.y;
                }else{
                    Vector3 scale = new Vector3(1, 1, 0);
                    pokemon.transform.localScale = scale;
                }
                
            } 
    }

    

    public IEnumerator examinePokemon(){
        foreach (GameObject item in opponents)
        {
            StartCoroutine(Functions.darkOut(item));
        }
        
        yield return new WaitForSeconds(1f);
        //ParticleSystem particle = wild_particles[0].GetComponent<ParticleSystem>();
        foreach (GameObject item in wild_particles)
        {
            ParticleSystem particle = item.GetComponent<ParticleSystem>();
            particle.Play();
        }
        
        StartCoroutine(Functions.CameraSwitch(revealCamera, combatCamera, "soft",0.5f));
        yield return new WaitForSeconds(0.5f);
        
    }

    
    
}

