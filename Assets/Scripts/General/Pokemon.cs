using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is how a pokemon can be represented.
[System.Serializable]
public class Pokemon
{   
    //The opponent's pokemon's scale:
    public Scale opScale;
    //The pokemon's ID.
    public string ID;
    //The pokemon's texture.
    public string texture;
    //The pokemon's texture on the player's Side.
    public string Mytexture;
    //The Pokemon's default Name:
    public string Name;
    //The Pokemon lvl;
    public int Level;
    //The pokemon life stat;
    public Stats stats;
    //The moves that the pokemon can have.
    public MoveCondition[] movePool;


        //Stats of an already built pokemon.
        public builtStats builtStats;


    //Other type of variables the Pokemon must have.
    public Sprite sprite;
    public Sprite Mysprite;
    //The type of pokeball it was caught with.
    public string pokeball;

}

//This class just stores scales.
[System.Serializable]
public class Scale{
    public float x;
    public float y;
}

//This class just stores scales.
[System.Serializable]
public class Stats{
    public int PS;
}

//This class just stores scales.
[System.Serializable]
public class builtStats{
    public int PS;
    public int actualPS;
    
    //The moves that the pokemon has;
    public Movement[] moves = {null,null,null,null};
}

//This class just stores scales.
[System.Serializable]
public class MoveCondition{
    public string move;
    public string type;
    public int level;
    public bool egg;
}