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
        //public ParticleSystem foundParticles;

        //StartCoroutine(Functions.ParabolicThrow(GameObject.Find("myBall"),GameObject.Find("myBall").transform.position,1f));



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
        //Position ofcombat: -2.7
        //Position inCombat: -4.6
    }

    //Function to start fights.
    //1º: The ID of the opponent pokemon.
    //2º: The Level of the opponent pokemon.
    public void InitFight(teamMessage data){
                general.inCombat = true;
        //An id, or a group of them is needed.
        string path = Application.streamingAssetsPath + "/Pokemons/"+data.IDs[0];
        Debug.Log(path + " exists?: "+ Directory.Exists(path));
        
        if(Directory.Exists(path)){
            revealOpponent(data.IDs, data.level);
        }else{
            List<string> errorList = new List<string>();
            errorList.Add("MissingNo");
            revealOpponent(errorList, 1);
        }
        StartCoroutine(StartCombatCamera(1));
        
        
        

    }

    //Function used to change the cameras, between the player, and the fighting stage.
    public IEnumerator StartCombatCamera(int seconds){
        in_combat_character.GetComponent<Animator>().enabled = false;
        myBall.GetComponent<SpriteRenderer>().enabled = false;
        
        in_combat_character.GetComponent<SpriteRenderer>().sprite = general.actualChar.baseInCombat;

        in_combat_character.SendMessage("Reveal",null,SendMessageOptions.DontRequireReceiver);
        myPoke.GetComponent<SpriteRenderer>().enabled = false;
        //Hide the text box from the Combat GUI.
        oppDialogManajer.SendMessage("Hide",null,SendMessageOptions.DontRequireReceiver);
        //Wait one second
        yield return new WaitForSeconds(seconds);
        //Switch to the reveal Camera.
        GameObject playerCamera = GameObject.Find("Main Camera");
        StartCoroutine(Functions.CameraSwitch(playerCamera, revealCamera, "rough",2));
        //Move the top.
        fieldTop.SendMessage("Reveal");
        //Wait
        yield return new WaitForSeconds(seconds);
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
        
        
        //Move top of the field to -0.04.
        //While the top of the field is not properly placed, fight must not iniciate.
        
    }

    //First parameter is the gameObject to change the texture.
    //Second is the Pokemon ID.
    public void revealOpponent(List<string> IDs, int lvl){
        float lerping = 1f/(IDs.Count+1);
        float lerptime = 0f + lerping;
        Debug.Log(lerping);
        for (int x = 0; x < IDs.Count; x++){

            Debug.Log(IDs.Count);
            GameObject pointInnit = GameObject.Find("SpawnPointInnit");
            GameObject pointEnd = GameObject.Find("SpawnPointEnd");
            GameObject pokemon = Instantiate (general.monPrefab,Vector3.Lerp(pointInnit.transform.position, pointEnd.transform.position, lerptime),Quaternion.identity);
            pokemon.GetComponent<SpriteRenderer>().sortingOrder = x;
            GameObject foundParticles = Functions.FindSpecificChild(pokemon,"Particle System");
            pokemon.transform.parent = fieldTop.transform;
            foundParticles.transform.parent = pokemon.transform;
            opponents.Add(pokemon);
            wild_particles.Add(foundParticles);
            
        
            //gets the pokemon.
                Pokemon pokemonData = Functions.FindPokemonByID(IDs[x]);
                //Sets the pokemon's lvl.
                pokemonData.Level = lvl;
                //Builds the opponent pokemon stats.
                pokemonData = Functions.BuildPokemon(pokemonData);
                //Assigns the pokemon.
                pokemon.SendMessage("setPokemon",pokemonData);
                if(Functions.rollPlayerPokemon(general.playerTeam) != null){
                    myPoke.SendMessage("setPokemon",Functions.rollPlayerPokemon(general.playerTeam));
                }else{
                    Pokemon pokeAlert = Functions.FindPokemonByID("MissingNo");
                    pokeAlert.Level = 1;
                    pokeAlert = Functions.BuildPokemon(pokeAlert);
                    myPoke.SendMessage("setPokemon",pokeAlert);
                }
        
            

            if(pokemonData != null){
                /*---- Changing the scale to the one required by the Pokemon needed. ----*/
                //Which means, that the scale of certain pokemons, will be bigger or lesser than other's.
                //Make a new pokemon class.
                //If the pokemonData has the scale value
                if(pokemonData.opScale != null && pokemonData.opScale.x > 0 && pokemonData.opScale.y > 0){
                    //Then change the pokemon's scale
                    Vector3 scale = new Vector3(pokemonData.opScale.x, pokemonData.opScale.y, 0);
                    pokemon.transform.localScale = scale;
                    //pokemon.transform.localScale.y = pokeData.opScale.y;
                }else{
                    Vector3 scale = new Vector3(1, 1, 0);
                    pokemon.transform.localScale = scale;
                }
                
            }
            lerptime += lerping;
        }
        
    }

    //Function used to finish the combat Camera.
    public void FinishCombatCamera(){
        try {
            //Set to null the opponent pokemon.
            Pokemon pokenull = new Pokemon();
            //Sets the pokemon's ID to null or empty.
            pokenull.ID = "";
            //Sets the pokemon.
            opponents[0].SendMessage("setPokemon",pokenull);
            myPoke.SendMessage("setPokemon",pokenull);
        }catch (Exception e){
            Debug.Log("Un mensaje de error ha ocurrido de manera esperada: "+e);
        }
        
        //Just set literally everything to how it was at the start.
        GameObject playerCamera = GameObject.Find("Main Camera");
        StartCoroutine(Functions.CameraSwitch(combatCamera, playerCamera, "rough",2));
        general.GetComponent<GeneralData>().inCombat = false;
        GameObject fieldTop = GameObject.Find("battlefield_up");
        fieldTop.SendMessage("Hide");
        positionedTop = false;
        CombatGUI.SendMessage("Hide");
        oppCombatGUI.SendMessage("Hide",null,SendMessageOptions.DontRequireReceiver);
        myPoke.SendMessage("End");
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

    public IEnumerator GUISay(List<string> newList, bool manual){
        CombatGUI.SendMessage("Hide");
        oppCombatGUI.SendMessage("Hide");
        oppDialogManajer.SendMessage("speak",newList,SendMessageOptions.DontRequireReceiver);
        //When, and i do not know how to do it, the DialogManager hides, then reveal the pokemon stats.
        yield return new WaitUntil(() => oppDialogManajer.GetComponent<defGuiListener>().hidden == true);
        if(manual != true){
            CombatGUI.SendMessage("Reveal");
            oppCombatGUI.SendMessage("Reveal");
        }
        
    }
    
}

