using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Movement
{   
    //This is just to get things ordered in the array, for example, we tell the pokemon we want to use this move,
    //then we tell him, use the move with the order x.
    public int? order;
    //The movement's type.
    public string type;
    //The movement's power.
    public int power;
    //The movement's accuracy.
    public int accuracy;
    //The movement0s category.
    public string cat;

    //The movement's traduction
    public Traduction traduction;

    //The movement effect.
    public string effect;
    //The movement max pps, its not supposed to change.
    public int pp;
    //The movement's pps that are supposed to change.
    public int actualPP;

}

[System.Serializable]
public class Traduction{
    //Traductions are stored in this one class.
    public Languaje name;
    public Languaje desc;
}

[System.Serializable]
public class Languaje{
   //The movement's traduction, its supposed to change depending on the player's languaje.
   public string spanish;
   public string english;
}

[System.Serializable]
public class typeList{
    public List<typeData> list;
}

[System.Serializable]
public class typeData{
    //The data inside of a type, for example, its colors and the doubles.
    public List<int> color;
    public List<int> colorB;
    public string name;
    public List<string> doubleTo;
    public List<string> halfTo;
    public List<string> inmuneTo;
}