using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokemonListener : MonoBehaviour
{
    //Variable Declaration
    public Pokemon pokemon;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Level;
    public TextMeshProUGUI PSs;
    public int maxPS;
    public int actualPS;
    public Slider psBar;
    public float diff;
    public bool AnimationPlayed;


    
    //Variables used to administrate the turns and the combat.
    public bool playerOwned;
    public bool decided;
    public bool hasAtacked;
    public Movement action;

    // Update is called once per frame
    void Update()
    {
        
        //Stat assignment, and var control.
        if(pokemon != null && pokemon.ID != null && pokemon.ID != ""){
            gameObject.GetComponent<SpriteRenderer>().enabled=true;
            //Changes the opponent pokemon display, to show its name.
            if(Name != null){
                Name.text = pokemon.Name;
            }
            //Displays the pokemon's level
            if(Level != null){
                Level.text = "Lvl: "+pokemon.Level;
            }
            //Displays the pokemon's HP.
            if(PSs != null){
                PSs.text = pokemon.builtStats.actualPS + " / "+pokemon.builtStats.PS;
            }
            //If there are a psBar
            if(psBar != null){
                psBar.minValue = 0;
                psBar.maxValue = pokemon.builtStats.PS;
                if(diff == 0){
                    diff = psBar.value - pokemon.builtStats.actualPS;
                }
            
            
                // If the value of the bar is greater than the actual pokemon's HP, decrease to equal always in one second.
                if(psBar.value > pokemon.builtStats.actualPS){
                    
                    //Get the difference between the bar value and pokemon's HP, that value will be added to the bar in the amount of time we specify, one second with no specification.
                    if(diff > 0){
                        psBar.value = psBar.value - diff*Time.deltaTime;
                    }else if(diff < 0){
                        psBar.value = psBar.value + diff*Time.deltaTime;
                    }
                    //This is only to make sure there are no unexpected infinite loops running around here.
                    if(psBar.value < pokemon.builtStats.actualPS+0.1f){
                        psBar.value = pokemon.builtStats.actualPS;
                    }
                    
                }else if(psBar.value < pokemon.builtStats.actualPS){
                    //Get the difference between the bar value and pokemon's HP, that value will be added to the bar in the amount of time we specify, one second with no specification.
                    if(diff > 0){
                        psBar.value = psBar.value + diff*Time.deltaTime;
                    }else if(diff < 0){
                        psBar.value = psBar.value - diff*Time.deltaTime;
                    }
                    
                    //This is only to make sure there are no unexpected infinite loops running around here.
                    if(psBar.value > pokemon.builtStats.actualPS-0.1f){
                        psBar.value = pokemon.builtStats.actualPS;
                    }
                    
                }else if(psBar.value == pokemon.builtStats.actualPS){
                    diff = 0;
                }
            }
        }
        

        //Setting the texture.
        /*---- Assigning the Texture to the Pokemon ----*/
        if(!playerOwned && pokemon != null){
            //IF the pokemon's static animation exists, play it.
            if(Functions.FindAnimation(GetComponent<Animator>(),pokemon.ID.ToLower()+"_static") != null && !AnimationPlayed){
                Debug.Log("Animation playing");
                Debug.Log(Functions.FindAnimation(GetComponent<Animator>(),pokemon.ID.ToLower()+"_static"));
                gameObject.GetComponent<Animator>().enabled=true;
                gameObject.GetComponent<Animator>().Play(pokemon.ID.ToLower()+"_static", 0, 1f);
                AnimationPlayed = true;
            //However, if it not exists, then just display the sprite.
            }else if(pokemon.sprite != null && !AnimationPlayed){
                Debug.Log("Animation not playing");
                gameObject.GetComponent<Animator>().enabled=false;
                gameObject.GetComponent<SpriteRenderer>().sprite = pokemon.sprite;
                AnimationPlayed = true;
            }
        }else if(pokemon != null){
            //IF the pokemon's static animation exists, play it.
            if(Functions.FindAnimation(GetComponent<Animator>(),pokemon.ID.ToLower()+"_my_static") != null && !AnimationPlayed){
                gameObject.GetComponent<Animator>().enabled=true;
                gameObject.GetComponent<Animator>().Play(pokemon.ID.ToLower()+"_my_static", 0, 1f);
                AnimationPlayed = true;
            //However, if it not exists, then just display the sprite.
            }else if(pokemon.Mysprite != null && !AnimationPlayed){
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                gameObject.GetComponent<SpriteRenderer>().sprite = pokemon.Mysprite;
                AnimationPlayed = true;
            }
        }
            
    }

    //Sets the Pokemon, that is all, just sets the Pokemon.
    public void setPokemon(Pokemon poke){
        pokemon=poke;
        diff = 0;
        if(!playerOwned){
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(0,0,0,255);
        }
        gameObject.GetComponent<Animator>().enabled=false;
        gameObject.GetComponent<SpriteRenderer>().enabled=true;
        AnimationPlayed = false;
    }

    //Ends the combat, so set variables to null, zero or "".
    public void End(){
        pokemon.sprite = null;
        gameObject.GetComponent<Animator>().enabled=false;
        AnimationPlayed = false;
        pokemon = null;
    }
}
