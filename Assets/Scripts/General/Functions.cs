using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Functions : MonoBehaviour
{

    //Function used to find an specific child by name, inside of an object.
    //First parameter is the father element of the element you want to get.
    //Second parameter its the name of the child.
    public static GameObject FindSpecificChild(GameObject obj,string name){
        //Father transform.
        Transform trans = obj.transform;
        //Children transform.
        Transform childTrans = trans.Find(name);
            
        //If found, return GameObject.
        if(childTrans != null){
            //Debug.Log(childTrans.gameObject);
            return childTrans.gameObject;
        }

        return null;
    }
    
    //Function used to control when an object its supposed to be on top or under other. Wont work with Tile decoration.
    public static void ControlProfundityWith(GameObject obj, GameObject player){
        SpriteRenderer spR = obj.GetComponent<SpriteRenderer>();

        if(player.transform.position.y >= obj.transform.position.y){
            spR.sortingOrder = 1;
        }else{
            spR.sortingOrder = -1;
        }
    }

    //Function used to find a pokemon file, inside of the Pokemon repository, it will search for folders, with the name of the ID, and access the file, and return the wanted pokemon, or the path at least;
    public static Pokemon FindPokemonByID(string ID){
        string[] FolderList = Directory.GetDirectories(Application.streamingAssetsPath + "/Pokemons");
        //Builds a pokemon object.
        Pokemon pokemon = new Pokemon();
        foreach (string str in FolderList){
            //If the name of a folder matches the ID, that is the one folder that we need
            FileInfo file = new FileInfo(str);
            if(file.Name == ID){
                //Gets the folder path, and the Json path.
                string path = Application.streamingAssetsPath + "/Pokemons"+"/"+ID+"/"+ID.ToLower()+".json";
                path = path.Replace("\\","/");
                string JsonValue = File.ReadAllText(path);
                //Loads data from the Json path
                pokemon = JsonUtility.FromJson<Pokemon>(JsonValue);
                pokemon.ID = ID;
                //Returns the pokemon with the data loaded.
                return pokemon;
           }
        }
        return pokemon;
    }

    //Function used to open the Items, GUI.
    //It will detect if player is on a fight, or if its not.
    public static void OpenItems(){
        Debug.Log("Items Opened");
    }

    //Function used to try to run.
    //It will detect if player is on a fight, or if its not.
    public static void RunFromFight(){
        GameObject combatManager = GameObject.Find("BattleManager");
        combatManager.SendMessage("FinishCombatCamera","flee");
        Debug.Log("Niguerundaio Polnarefuuuuu");
    }

    //Function used to open the pokemon menu, it will work differently depending on the situation.
    public static void OpenPokemonMenu(){
        Debug.Log("Change, now");
    }

    //Function to assign stats, based on the pokemon level, Evs and Ivs, and stats, of course.
    public static Pokemon BuildPokemon(Pokemon pokemon){
        //The formula to calculate Pokemon's stats.
        //The finished formulae: pokemon.builtStats.PS = Mathf.Floor(0.01f*(2*pokemon.stats.PS/*+IVs+Mathf.Floor(0.25*EVs)*/*pokemon.Level)+pokemon.Level+10);
        float operation = (0.01f*((2*pokemon.stats.PS)*pokemon.Level)+pokemon.Level+10);
        pokemon.builtStats.PS = (int)Mathf.Floor(operation);
        pokemon.builtStats.actualPS = pokemon.builtStats.PS;
        List<string> movesArr = new List<string>();
        foreach (MoveCondition item in pokemon.movePool){
            if(item.type=="level"){
                if(pokemon.Level >= item.level){
                    movesArr.Add(item.move);
                }
            }  
            //More conditions will be added in a future.
        }

        for( int x = 0; x < pokemon.builtStats.moves.Length; x++){
            if(movesArr.Count > 0){
                string choosen = movesArr[Random.Range(0,movesArr.Count)];
                movesArr.Remove(choosen);
                pokemon.builtStats.moves[x] = Functions.BuildMovement(choosen);
            }
        }
        

        if(pokemon.pokeball == null){
            pokemon.pokeball = "pokeball";
        }

        //ASSIGN THE TEXTURES:
        pokemon.sprite = Functions.CatchTexture(pokemon, pokemon.texture);
        pokemon.Mysprite = Functions.CatchTexture(pokemon, pokemon.Mytexture);
        return pokemon;
    }

    //Static function to build a movement.
    public static Movement BuildMovement(string move){
        Movement newMove = new Movement();
        string path = (Application.streamingAssetsPath+"/Json/Movements/"+move+".json");
        string fileText = File.ReadAllText(path);
        newMove = JsonUtility.FromJson<Movement>(fileText);
        newMove.actualPP = newMove.pp;
        return newMove;
    }

    //Static function used to dark any object, and 
    public static IEnumerator darkOut(GameObject someObject){
        if(someObject.GetComponent<SpriteRenderer>() != null){
            //Make a new color
            Color32 color = someObject.GetComponent<SpriteRenderer>().color;
            someObject.GetComponent<SpriteRenderer>().color = color;
            //yield return new WaitForSeconds(1);
            //Repeat until the object has its normal color.
            while (color.r < 255){
                yield return new WaitForSeconds(0.0001f);
                someObject.GetComponent<SpriteRenderer>().color = color;
                int newColor = color.r+4;
                color.r = (byte)newColor;
                newColor = color.g+4;
                color.g = (byte)newColor;
                newColor = color.b+4;
                color.b = (byte)newColor;
                if(color.r >= 252){
                    color.r = 255;
                    color.g = 255;
                    color.b = 255;
                }
            }
        }else{
            Debug.LogError("No se ha encontrado el SpriteRenderer sobre el objeto: "+someObject.name+".");
        }
    }

    //Function to make new Camera transitions at the same time cameras change..
    public static IEnumerator CameraSwitch(GameObject Camera1, GameObject Camera2, string type, float seconds){

        Vector2 orPos;
        float orScale;
        float scaleDifPlus;
        float scaleDifMinus;
        float speed;

        //If the type of transition is rough, just change the cameras.
        if(type == "rough"){
            Camera1.GetComponent<Camera>().enabled = false;
            Camera2.GetComponent<Camera>().enabled = true;

        }else if (type == "soft"){
            orPos = Camera1.transform.position;
            scaleDifPlus = Camera1.GetComponent<Camera>().orthographicSize - Camera2.GetComponent<Camera>().orthographicSize;
            scaleDifMinus = Camera2.GetComponent<Camera>().orthographicSize - Camera1.GetComponent<Camera>().orthographicSize;
            speed = (Vector3.Distance(Camera1.transform.position, Camera2.transform.position)/seconds)*Time.deltaTime;
            orScale = Camera1.GetComponent<Camera>().orthographicSize;

            //While the distance between the cameras is grater than 0, move camera A to camera B
            while( Vector3.Distance(Camera1.transform.position, Camera2.transform.position) > 0 && Camera1.GetComponent<Camera>().orthographicSize != Camera2.GetComponent<Camera>().orthographicSize){

                //Moving, the transaction will always last as much seconds as the player says.
                Camera1.transform.position = Vector3.MoveTowards(
                    //Origin position
                    Camera1.transform.position,
                    //Destiny position
                    Camera2.transform.position,
                    //Speed of the camera A moving, depending of the seconds we specify.
                    speed
                    );
                //Scaling to the other Camera.
                //If the scale of the Camera 1 is bigger than the scale of Camera 2, then decrease until the scale is the same.
                if(Camera1.GetComponent<Camera>().orthographicSize > Camera2.GetComponent<Camera>().orthographicSize){
                    //Scale camera A to the size of Camera B, depending of how many seconds we specify.
                    Camera1.GetComponent<Camera>().orthographicSize = Camera1.GetComponent<Camera>().orthographicSize + (scaleDifPlus/seconds)*Time.deltaTime;
                }
                
                //If the scale of the Camera 1 is minor than the scale of Camera 2, then increase until the scale is the same.
                if(Camera1.GetComponent<Camera>().orthographicSize < Camera2.GetComponent<Camera>().orthographicSize){
                    Camera1.GetComponent<Camera>().orthographicSize = Camera1.GetComponent<Camera>().orthographicSize + (scaleDifMinus/seconds)*Time.deltaTime;
                }

                yield return new WaitForSeconds(0.0001f);
            }

            //Change cameras finally.
            yield return new WaitForSeconds(0.01f);
            Camera1.GetComponent<Camera>().enabled = false;
            Camera2.GetComponent<Camera>().enabled = true;
            Camera1.transform.position = orPos;
            Camera1.GetComponent<Camera>().orthographicSize = orScale;
        }

    }


    //Function used to find an anmation in an animator. Works properly.
    public static AnimationClip FindAnimation (Animator animator, string name) {
        //For each animationclip in the animator, if the name of the clip is the same as the name searched, return the animation clip, else, reutn null.
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == name){
                return clip;
            }
        }

        return null;
    }

    //Function to load a pokemon's texture from path.
    public static Sprite CatchTexture(Pokemon pokemon, string pokeTexture){
        string path = Application.streamingAssetsPath + "/Pokemons/"+pokemon.ID;
            //Makes a new Texture.
            Texture2D texture = new Texture2D(2,2);

            //If the file of the pokmeon's texture in the opponent's side exists on its propper path:
            if(System.IO.File.Exists(path+"/"+pokeTexture)){
                //Get the bytes of the file
                byte[] fileData;
                fileData = System.IO.File.ReadAllBytes(path+"/"+pokeTexture);
                //If there is nothing wrong with the texture, gives the texture to the opponent pokemon
                if(texture){
                    //Changes the type of texture.
                    texture.filterMode = FilterMode.Point;
                    //Loads the texture.
                    texture.LoadImage(fileData);
                    //Info log.
                    //Debug.Log("Ancho de textura: "+texture.width+", Alto de textura: "+texture.height+", FilterMode: " + texture.filterMode);
                    //Makes a new sprite, with the dimensions of the textures, and adds it to the opponent.
                    Rect rect = new Rect(0, 0, texture.width, texture.height);
                    return Sprite.Create(texture,rect, new Vector2(0.5f, 0.5f));
                }else{
                    Debug.LogError("Texture not found");
                    return null;
                }
            }else{
                Debug.LogError("Texture not existing");
                return null;
            }
    }

    //Function used to get the first aviable pokemon inside of a Pokemon team.
    public static Pokemon rollPlayerPokemon(pokeTeam team){
        if(team.pokeA.builtStats.actualPS > 0){
            return team.pokeA;
        }else if(team.pokeB.builtStats.actualPS > 0){
            return team.pokeB;
        }else if(team.pokeC.builtStats.actualPS > 0){
            return team.pokeC;
        }else if(team.pokeD.builtStats.actualPS > 0){
            return team.pokeD;
        }else if(team.pokeE.builtStats.actualPS > 0){
            return team.pokeE;
        }else if(team.pokeF.builtStats.actualPS > 0){
            return team.pokeF;
        }

        return null;
    }

    //From red to normal, changing size also.
    public static IEnumerator TrainerReveal(GameObject pokeObject){
        if(pokeObject.GetComponent<SpriteRenderer>() != null){
            //Make a new color
            Color32 color = pokeObject.GetComponent<SpriteRenderer>().color;
            Color32 red = new Color32(180,0,0,255);
            Vector3 size = new Vector3(0,0,0);
            Vector3 orSize = pokeObject.transform.localScale;
            pokeObject.transform.localScale = size;
            pokeObject.GetComponent<SpriteRenderer>().color = red;
            yield return new WaitForSeconds(0.3f);
            //Repeat until it has its normal Size
            while(size.x < orSize.x){
                if(size.x < orSize.x){
                size.x = size.x+4*Time.deltaTime;
                }
                if(size.y < orSize.y){
                    size.y = size.y+4*Time.deltaTime;
                }
                pokeObject.transform.localScale = size;
                yield return new WaitForSeconds(0.0001f);
            }
            //Repeat until the object has its normal color.
            while (red.b < 255){
                if(red.r <= 251){
                    int newColor = red.r+1;
                    red.r = (byte)newColor;
                }
                if(red.b <= 251){
                    int newColor = red.b+2;
                    red.b = (byte)newColor;
                }
                if(red.g <= 251){
                    int newColor = red.g+2;
                    red.g = (byte)newColor;
                }
                if(red.b >= 252){
                    red.r = 255;
                    red.g = 255;
                    red.b = 255;
                }
                pokeObject.GetComponent<SpriteRenderer>().color = red;                
                yield return new WaitForSeconds(0.0001f);
            }
            
        }else{
            yield return new WaitForSeconds(0.00001f);
        }
    }


    //Throw an object to a position, making a parabole.
    public static IEnumerator ParabolicThrow(GameObject throwable, Vector3 end, float time){

        float timer = 0f;
        Vector3 startPos = throwable.transform.position;

        if(time > 0){
            Vector3 middleTop = new Vector3(end.x, end.y+2f,end.z);
            while(timer <= 0.5f){
                //float height = Mathf.Sin(Mathf.PI * timer)*2;
                throwable.transform.position = Vector3.Lerp(startPos, middleTop, timer);
                timer += 1f*(Time.deltaTime/time);
                yield return new WaitForSeconds(0.000001f);
            }
            Vector3 middlePoint = Vector3.Lerp(startPos, middleTop, 0.5f);
            timer = 0f;
            while(timer <= 1f){
                throwable.transform.position = Vector3.Lerp(middlePoint, end, timer);
                timer += 1*(Time.deltaTime/time);
                yield return new WaitForSeconds(0.000001f);
            }
            //throwable.transform.position = end;
        }else{
            Debug.LogError("'time' parameter has to be higher than 0.");
        }
    }

    //Function used to get A SINGLE string, depending on the traduction.
    //jsonPath, the relative path from Json/Dialog/, from the streamingAssets.
    public static string getTraduction(string jsonPath, string? option){
        string res;
        GeneralData general = GameObject.Find("GameManajer").GetComponent<GeneralData>();
        jsonPath = (Application.streamingAssetsPath + "/Json/Dialog/" + general.lang + "/"+ jsonPath);
        dialog dial = new dialog();
        JsonUtility.FromJsonOverwrite(File.ReadAllText(jsonPath),dial);
        if(dial != null){
            if(option != null && option == "random"){
                int random = Random.Range(0, dial.content.Count );
                Debug.Log("El texto aleatorio elejido  entre 0 y "+ dial.content.Count +" es el número " + random);
                res = dial.content[random].list[0];
            }else{
                res = dial.content[0].list[0];
            }
            return res;
            
        }else{
            return "Traduction not Found";
        }
    }

    //Function used to get A WHOLE DIALOG, depending on the traduction.
    public static dialog getDialog(string jsonPath){
        GeneralData general = GameObject.Find("GameManajer").GetComponent<GeneralData>();
        jsonPath = (Application.streamingAssetsPath + "/Json/Dialog/" + general.lang + "/"+ jsonPath);
        dialog dial = new dialog();
        JsonUtility.FromJsonOverwrite(File.ReadAllText(jsonPath),dial);
        if(dial != null){
            return dial;
        }else{
            return null;
        }
    }

}
