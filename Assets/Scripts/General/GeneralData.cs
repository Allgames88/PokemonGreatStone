using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralData : MonoBehaviour
{
    public string camera;

    /* --------------- Different configurations, players will be able to chnge it in the future.*/

    public KeyCode actionKey = KeyCode.Z;
    public KeyCode runKey = KeyCode.Space;
    public KeyCode altRunKey = KeyCode.LeftShift;
    public float textDelay = 0.03f;
    public string lang;
    public character[] characterPool;
    public character actualChar;
    public bool Debuging;
    public float playerSpeed = 0.7f;
    public float playerSpeedRunning = 1.5f;

    //Saves and all of that.
    public pokeTeam playerTeam;
    public List<typeData> types;
    public string character = "she";

    

    //Internal config
    public bool inCombat;
    public GameObject monPrefab;
    public GameObject wildParticle;

    private void Start(){
        string content = File.ReadAllText(Application.streamingAssetsPath + "/Json/Movements/general.json");
        typeList list = new typeList();
        if(content != null){
            list = JsonUtility.FromJson<typeList>(content);
            types = list.list;
        }
        
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