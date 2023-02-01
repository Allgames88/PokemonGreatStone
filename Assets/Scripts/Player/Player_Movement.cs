using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player_Movement : MonoBehaviour
{
    //Variable declarations.
    public Animator anim;
    public GeneralData general;
    public Vector2 Pos;
    public Vector2 NextPos;
    public float speed;
    public Vector2 Direccion;
    public LayerMask obstacles;
    public SpriteRenderer sprt;
    public string Look;
    public bool canMove;
    public int steps;
    public character character;
    public Sprite thisUp;
    public Sprite thisDown;
    public Sprite thisLeft;
    public Sprite thisRight;

    // Start is called before the first frame update
    void Start()
    {
        //--------------------- Clear code ------------------
            canMove = true;
            general = GameObject.Find("GameManajer").GetComponent<GeneralData>();
            character = general.actualChar;
        //----------------------- --------------- ------------------

        //We get the obstacles layer;
        obstacles = LayerMask.GetMask("obstacles");

        //WE get the animator, the sprite renderer.
        anim = GetComponent<Animator>();
        sprt = GetComponent<SpriteRenderer>();
        //Inicialice NextPos as our actual position.
        NextPos = transform.position;
        //And speed as the general option speed.
        speed = general.playerSpeed;

    }

    // Update is called once per frame
    void Update()
    {

        UpdateCharacter(anim);

        //When pressig space update speed to the runnning value.
        if(Input.GetKeyDown(general.runKey) || Input.GetKeyDown(general.altRunKey)){
            speed = general.playerSpeedRunning;
        }
        //When stop pressing space, update speed to the normal value.
        if(Input.GetKeyUp(general.runKey) || Input.GetKeyUp(general.altRunKey)){
            speed = general.playerSpeed;
        }
        Pos = transform.position;
        //Here to save the vertical and horizontal inputs.
        Direccion.x = Input.GetAxis("Horizontal");
        Direccion.y = Input.GetAxis("Vertical");
        //When pressing right, and we dont have to move yet:
        //***** With this code here, we make that the inputs value cant be higher than 0.5f or lower than -0.5f
            if(Direccion.x > 0.4f){
                Direccion.x = 0.4f;
            }
            else if(Direccion.y > 0.4f){
                Direccion.y = 0.4f;
            }
            else if(Direccion.x < -0.4f){
                Direccion.x = -0.4f;
            }
            else if(Direccion.y < -0.4f){
                Direccion.y = -0.4f;
            }
            //This is made so the walking things wont have much delay, but the neccesary.
        //********************************************************
        //If the axis input its on X its higher than 0.3f, that means that the playes has presseed "right".
        if(Direccion.x > 0.3f && NextPos == Pos && canMove){
            //Look for a collision where we are told.
            if(CheckCollision(new Vector2(0.16f,0))){
                //Confirmates that the player has made an step;
                Step();
                //If there is no collision, assign to NextPos the next position at the right, enable the animation, and play the animation of the player walking to the right.
                NextPos.x += 0.16f;
                anim.enabled = true;
                anim.Play("WalkRight");
                sprt.sprite = null;
            //However, if there is a collision, we stop the animations, and change the sprite to look right, just right.
            }else{
                anim.enabled = false;
                if(thisRight == null){
                    sprt.sprite = character.baseRight;
                    thisRight = character.baseRight;
                }else{
                    sprt.sprite = thisRight;
                }
                
            }
        //If the axis input its on X its lower than -0.3f, that means that the playes has presseed "left".
        }else if(Direccion.x < -0.3f && NextPos == Pos && canMove){
            //Look for a collision where we are told.
            if(CheckCollision(new Vector2(-0.16f,0))){
                //Confirmates that the player has made an step;
                Step();
                //If there is no collision, assign to NextPos the next position at the left, enable the animation, and play the animation of the player walking to the left.
                anim.enabled = true;
                anim.Play("WalkLeft");
                NextPos.x -= 0.16f;
                sprt.sprite = null;
            //However, if there is a collision, we stop the animations, and change the sprite to look left, just left.
            }else{
                
                anim.enabled = false;
                if(thisLeft == null){
                    sprt.sprite = character.baseLeft;
                    thisLeft = character.baseLeft;
                }else{
                    sprt.sprite = thisLeft;
                }
            }
        //If the axis input its on Y its higher than 0.3f, that means that the playes has presseed "Up".
        }else if(Direccion.y > 0.3f && NextPos == Pos && canMove){
            //Look for a collision where we are told.
            if(CheckCollision(new Vector2(0,0.16f))){
                //Confirmates that the player has made an step;
                Step();
                //If there is no collision, assign to NextPos the next position at the top, enable the animation, and play the animation of the player walking up.
                anim.enabled = true;
                anim.Play("WalkUp");
                NextPos.y += 0.16f;
                sprt.sprite = null;
            //However, if there is a collision, we stop the animations, and change the sprite to look up, just up.
            }else{
                anim.enabled = false;
                
                if(thisUp == null){
                    sprt.sprite = character.baseUp;
                    thisUp = character.baseUp;
                }else{
                    sprt.sprite = thisUp;
                }

            }
        //If the axis input its on Y its lower than -0.3f, that means that the playes has presseed "Down".
        }else if(Direccion.y < -0.3f && NextPos == Pos && canMove){
            //Look for a collision where we are told.
            if(CheckCollision(new Vector2(0,-0.16f))){
                //Confirmates that the player has made an step;
                Step();
                //If there is no collision, assign to NextPos the next position at the bottom, enable the animation, and play the animation of the player walking down.
                anim.enabled = true;
                anim.Play("WalkDown");
                NextPos.y -= 0.16f;
                sprt.sprite = null;
            //However, if there is a collision, we stop the animations, and change the sprite to look down, just Down.
            }else{
                anim.enabled = false;
                
                if(thisDown == null){
                    sprt.sprite = character.baseDown;
                    thisDown = character.baseDown;
                }else{
                    sprt.sprite = thisDown;
                }
            }
        //And if non of the Axis Input have changed, then the player will look, at the last point he was looking. This part its important, makes the player stops when there its no collisions and its not moving.
        }else if(NextPos == Pos && canMove){
            anim.enabled = false;
            if(Look == "up"){
                
                if(thisUp == null){
                    sprt.sprite = character.baseUp;
                    thisUp = character.baseUp;
                }else{
                    sprt.sprite = thisUp;
                }

            }
            if(Look == "down"){
                
                if(thisDown == null){
                    sprt.sprite = character.baseDown;
                    thisDown = character.baseDown;
                }else{
                    sprt.sprite = thisDown;
                }

            }
            if(Look == "left"){
                
                if(thisLeft == null){
                    sprt.sprite = character.baseLeft;
                    thisLeft = character.baseLeft;
                }else{
                    sprt.sprite = thisLeft;
                }

            }
            if(Look == "right"){
                
                if(thisRight == null){
                    sprt.sprite = character.baseRight;
                    thisRight = character.baseRight;
                }else{
                    sprt.sprite = thisRight;
                }

            }
        }else if(!canMove){
            anim.enabled = false;
            if(Look == "up"){
                
                if(thisUp == null){
                    sprt.sprite = character.baseUp;
                    thisUp = character.baseUp;
                }else{
                    sprt.sprite = thisUp;
                }

            }
            if(Look == "down"){
                
                if(thisDown == null){
                    sprt.sprite = character.baseDown;
                    thisDown = character.baseDown;
                }else{
                    sprt.sprite = thisDown;
                }

            }
            if(Look == "left"){
                
                if(thisLeft == null){
                    sprt.sprite = character.baseLeft;
                    thisLeft = character.baseLeft;
                }else{
                    sprt.sprite = thisLeft;
                }

            }
            if(Look == "right"){
                
                if(thisRight == null){
                    sprt.sprite = character.baseRight;
                    thisRight = character.baseRight;
                }else{
                    sprt.sprite = thisRight;
                }

            }
        }

        //Here, the player will move to the NextPosition choosen.
        transform.position = Vector2.MoveTowards(transform.position, NextPos, speed * Time.deltaTime);

        //Here we will store the directions the player is looking to, also used by other scripts.
        if(Direccion.x > 0 && NextPos == Pos && canMove){
            Look = "right";
        }else if(Direccion.x < 0 && NextPos == Pos && canMove){
            Look = "left";
        }else if(Direccion.y > 0 && NextPos == Pos && canMove){
            Look = "up";
        }else if(Direccion.y < 0 && NextPos == Pos && canMove){
            Look = "down";
        }else{
            
        }


        //There is a bit of delay on the player stopping moving, im going to try to fix it.
        //if(Input.GetKeyUp())
        //No lo necesité.
    }

    public void Step(){
        //adds one to the steps counter;
        steps++;
    }

    //Function that checks for an obstacle collision.
    public bool CheckCollision(Vector2 a){
        //We decalre raycast, for the obstacle layer.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, a, 0.16f, obstacles);
        //If hit, player wont move.
        if( hit ){
            //Debug.Log(hit.collider.tag);
            Debug.DrawRay(Pos, a, Color.red, 1f);
            return false;
        //If not hit, player will move.
        }else{
            Debug.DrawRay(Pos, a, Color.yellow, 1f);
            return true;
        }
        
    }

    void UpdateCharacter(Animator anim){

        //Estados del controlador en un array:
        AnimatorOverrideController aoc = new AnimatorOverrideController(anim.runtimeAnimatorController);
        anim.runtimeAnimatorController = aoc;
        aoc["Throw"] = character.Throw;
        aoc["WalkUp"] = character.WalkUp;
        aoc["WalkDown"] = character.WalkDown;
        aoc["WalkLeft"] = character.WalkLeft;
        aoc["WalkRight"] = character.WalkRight;
    }

}
