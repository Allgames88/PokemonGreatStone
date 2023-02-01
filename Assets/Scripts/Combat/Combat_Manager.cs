using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Manager : MonoBehaviour
{

    public bool combat_iniciated = false;
    int rounds;
    public List<Pokemon> playerInGame;
    public List<Pokemon> opponentInGame;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(combat_iniciated){
            CombatMain();
        }
    }


    void CombatMain(){



    }

    public void switch_combat_mode(){
        if(combat_iniciated){
            combat_iniciated = false;
        }else{
            combat_iniciated = true;
        }
    }
}
