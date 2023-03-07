using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Manager : MonoBehaviour
{

    public bool combat_iniciated = false;
    public int turn;
    public PokemonListener myPoke;
    public PokemonListener oppPoke;
    public bool wildDisplayed;
    public bool playerDisplayed;
    public GeneralData general;
    GameObject oppDialogManajer; 
    GameObject CombatGUI;
    GameObject oppCombatGUI;
    GameObject combatCam;
    
    // Start is called before the first frame update
    void Start()
    {
        general = GameObject.Find("GameManajer").GetComponent<GeneralData>();
        oppDialogManajer = GameObject.Find("combatDialogManager"); 
        CombatGUI = GameObject.Find("CombatGUI");
        oppCombatGUI = GameObject.Find("opponent_stats");
        combatCam = GameObject.Find("BattleCamera");
    }

    // Update is called once per frame
    void Update()
    {
        if(combat_iniciated){
            CombatMain();
        }
    }


    void CombatMain(){
        //So, if the turn is 0, run a Coroutine that makes the reveal of the pokemons, both player and opponent.
        if(turn == 0){
            StartCoroutine("revelationTurn");
        }

        if(myPoke.decided && oppPoke.decided){
            StartCoroutine(CombatTurn());
        }


    }

    //Function to switch the combat mode, something easy to understand;
    public void switch_combat_mode(Pokemon? poke){
        if(combat_iniciated){
            wildDisplayed = false;
            playerDisplayed = false;
            combat_iniciated = false;
        }else{
            turn = 0;
            combat_iniciated = true;
        }
        if(poke != null){
            oppPoke.pokemon = poke;
        }
        

    }

    IEnumerator revelationTurn(){
        turn++;
        Debug.Log("Times done");
        //If the opponent pokemon has not been displayed just yet, then:
        if(!wildDisplayed){
            wildDisplayed = true;
            //Get the cameras
            GameObject revealCam = GameObject.Find("revealCamera");
            
            yield return new WaitUntil(() => general.camera == "combat");

            //Change the object color to black;
            Color32 black = new Color32(0,0,0,255);

            //Set the required pokemon scale, to the pokemon object.
            if(oppPoke.pokemon.opScale != null && oppPoke.pokemon.opScale.y > 0 && oppPoke.pokemon.opScale.x > 0){
                Vector3 scale = new Vector3(oppPoke.pokemon.opScale.x,oppPoke.pokemon.opScale.y,0);
                oppPoke.transform.localScale = scale;
            }else{
                Vector3 scale = new Vector3(1,1,0);
                oppPoke.transform.localScale = scale;
            }

            oppPoke.transform.gameObject.GetComponent<SpriteRenderer>().color = black;
            StartCoroutine(Functions.CameraSwitch(combatCam, revealCam, "rough",2));

            //Wait and change from the reveal camera, to the combatCamera.
            yield return new WaitForSeconds(1);

            //Do the thing where the object returns to its color, slowly.
            StartCoroutine(Functions.darkOut(oppPoke.transform.gameObject));

            //and change to the reveal camera.
            StartCoroutine(Functions.CameraSwitch(revealCam, combatCam, "soft",0.8f));
            

            //Wait a moment and send a message.
            yield return new WaitForSeconds(1);
            yield return new WaitUntil(() => general.camera == "combat");

            //Make the player know the pokemon that has appeared, by showing it on text, traduced.
            List<string> newList = new List<string>();
            string newStr = Functions.getTraduction("battle/wildFind.json",null);
            newList.Add(newStr.Replace("%pkm%", oppPoke.pokemon.ID));
            StartCoroutine(GUISay(newList,true));
        }
        //Get the combat Dialog manager, and wait until its hidden.
        yield return new WaitUntil(() => oppDialogManajer.GetComponent<defGuiListener>().hidden == true);

        if (!playerDisplayed){
            playerDisplayed = true;
            StartCoroutine(SwitchIn(1));
        }
    }



//Function to make the combat GUI say something;
    public IEnumerator GUISay(List<string> newList, bool manual){
        //oppDialogManajer.GetComponent<defGuiListener>().hidden = false;
        CombatGUI.SendMessage("Hide",SendMessageOptions.DontRequireReceiver);
        oppCombatGUI.SendMessage("Hide",SendMessageOptions.DontRequireReceiver);
        oppDialogManajer.SendMessage("Reveal",SendMessageOptions.DontRequireReceiver);
        oppDialogManajer.SendMessage("speak",newList,SendMessageOptions.DontRequireReceiver);
        //When, and i do not know how to do it, the DialogManager hides, then reveal the pokemon stats.
        yield return new WaitUntil(() => oppDialogManajer.GetComponent<defGuiListener>().hidden == true);
        if(manual != true){
            CombatGUI.SendMessage("Reveal",SendMessageOptions.DontRequireReceiver);
            oppCombatGUI.SendMessage("Reveal",SendMessageOptions.DontRequireReceiver);
        }
        
    }


    public IEnumerator SwitchIn(int position){
        Pokemon poke = Functions.rollPlayerPokemon(general.playerTeam);

            //Load the text and display the pokemon that the player is going to use.
            List<string> newList = new List<string>();
            string newStr = Functions.getTraduction("battle/myGo.json","random");
            newList.Add(newStr.Replace("%pkm%", poke.Name));
            StartCoroutine(GUISay(newList,true));

            //Wait until the thing stops displaying text.
            yield return new WaitUntil(() => oppDialogManajer.GetComponent<defGuiListener>().hidden == true);

            /*
                Here to put the player's character animation throwing the ball, when around 75% of the animation is played,
                display the correct pokeball doing a parabole, and when the parabole finishes, the ball animates open, and
                then the rest of the code can continue.
            
            */

            //Start player's animation
            GameObject character = GameObject.Find("player's character");
            GameObject ball = GameObject.Find("myBall");
            character.GetComponent<Animator>().enabled = true;
            character.GetComponent<Animator>().Rebind();
            

            //wait until animation plays at 75%
            //Yes, i did it, it was hard man.
            AnimationClip clip = Functions.FindAnimation(character.GetComponent<Animator>(), "Throw");
            
            float animLengh = clip.length * 0.70f;
            yield return new WaitForSeconds(animLengh);
            character.SendMessage("Hide",SendMessageOptions.DontRequireReceiver);

            //Do the pokeball work.
            Animator ballAnim = ball.GetComponent<Animator>();
            ball.GetComponent<SpriteRenderer>().enabled = true;
            ballAnim.speed = 1.5f;
            ballAnim.Play(poke.pokeball + "_throw");
            //Throw the pokeball, and wait a second, cuz' its the same time 
            StartCoroutine(Functions.ParabolicThrow(ball, myPoke.transform.position, 0.6f));
            yield return new WaitForSeconds(1f);
            
            ballAnim.Play(poke.pokeball + "_open");
            yield return new WaitForSeconds(0.8f);
            
            ballAnim.speed = 1f;
            //Animate some spinning particles or whatever...
            //yield return new WaitForSeconds(0.3f);
            
            /*
            -------------------------------------------------------------------------------------------
            */



            //wait a second, this one will be erased.
                //yield return new WaitForSeconds(1);

            //Trainer reveal, from red an tiny, to big and normal.
            myPoke.pokemon = poke;
            StartCoroutine(Functions.TrainerReveal(myPoke.transform.gameObject));
        yield return new WaitForSeconds(0.8f);
            ball.GetComponent<SpriteRenderer>().enabled = false;
            CombatGUI.SendMessage("Reveal",SendMessageOptions.DontRequireReceiver);
            oppCombatGUI.SendMessage("Reveal",SendMessageOptions.DontRequireReceiver);

    }

    public IEnumerator CombatTurn(){
        yield return null;
    }


    public void FinishCombatCamera(string reason){
        StartCoroutine(RealFinishCombatCamera(reason));
    }

    //Function used to finish the combat Camera.
    public IEnumerator RealFinishCombatCamera(string reason){
        if(reason == "flee"){

            StartCoroutine(GUISay(Functions.getDialog("/battle/testFlee.json").content[0].list,true));
            yield return new WaitUntil(() => oppDialogManajer.GetComponent<defGuiListener>().hidden == true);
        }

        myPoke.SendMessage("End");
        oppPoke.SendMessage("End");
        
        //Just set literally everything to how it was at the start.
        GameObject playerCamera = GameObject.Find("Main Camera");
        StartCoroutine(Functions.CameraSwitch(combatCam, playerCamera, "rough",2));
        general.camera = "player";
        general.inCombat = false;
        GameObject fieldTop = GameObject.Find("battlefield_up");


        
        CombatGUI.SendMessage("Hide");
        oppCombatGUI.SendMessage("Hide",null,SendMessageOptions.DontRequireReceiver);
        yield return new WaitUntil(() => (CombatGUI.GetComponent<GuiListener>().HidenCent == true && oppCombatGUI.GetComponent<defGuiListener>().hidden == true));
        yield return new WaitForSeconds(0.5f);
        fieldTop.SendMessage("Hide");
        switch_combat_mode(null);
        yield return new WaitForSeconds(1.2f);
        GameObject.Find("Character").GetComponent<Player_combat_detector>().canBattle = true;
    }
}
