using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Movement
{   
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
