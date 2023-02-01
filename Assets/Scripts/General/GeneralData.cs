using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralData : MonoBehaviour
{
    public float playerSpeed = 0.7f;
    public float playerSpeedRunning = 1.5f;
    public KeyCode actionKey = KeyCode.Z;
    public KeyCode runKey = KeyCode.Space;
    public KeyCode altRunKey = KeyCode.LeftShift;
    public float textDelay = 0.03F;
    public bool inCombat;
    public bool Debuging;
    public character[] characterPool;
    public character actualChar;
    public string lang = "spanish";

    public GameObject monPrefab;
    public GameObject wildParticle;

    //Saves and all of that.
    public pokeTeam playerTeam;

    public string character = "she";

    private void Start(){
        textDelay = 0.03F;
        DontDestroyOnLoad(this.gameObject);
        //Only for now:
        Pokemon parasect = Functions.FindPokemonByID("Parasect");
        parasect.Level = 100;
        parasect = Functions.BuildPokemon(parasect);
        playerTeam.pokeA = parasect;
        
        
    }

    private void Update() {
        foreach(character ch in characterPool){
            if(ch.name == character){
                actualChar = ch;
            }
        }
    }
    
    

}

    //Class for saving teams.
    [System.Serializable]
    public class pokeTeam {
        public Pokemon pokeA;
        public Pokemon pokeB;
        public Pokemon pokeC;
        public Pokemon pokeD;
        public Pokemon pokeE;
        public Pokemon pokeF;
    }

    //Class for Characters.
    [System.Serializable]
    public class character{
        public string name;
        public Sprite baseInCombat;
        public AnimationClip Throw;
        public AnimationClip WalkUp;
        public AnimationClip WalkDown;
        public AnimationClip WalkRight;
        public AnimationClip WalkLeft;
        public Sprite baseDown;
        public Sprite baseUp;
        public Sprite baseLeft;
        public Sprite baseRight;
    }