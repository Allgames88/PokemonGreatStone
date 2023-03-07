using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    //Variable declarations.
    public GameObject DialogBox;
    public Text dialog;
    public Text NextUI;
    public TextMeshProUGUI TMPdialog;
    public TextMeshProUGUI TMPNextUI;
    public GeneralData general;
    public bool CoroutineWork;

    public bool talking;
    public int count;
    
    //The mustAct variable its used to know when this script must read the action key input or not.
    public bool mustAct;

    public List<string> Lista;

    // Start is called before the first frame update
    void Start()
    {
        CoroutineWork = true;
        general = GameObject.Find("GameManajer").GetComponent<GeneralData>();
        talking = false;
        DialogBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(general.actionKey) && talking && mustAct){
            Debug.Log("Ocurrido");
            speak(null);
        }else{
            mustAct = true;
        }
        
    }
    //This funtion will show on screen the texts.
    public void speak(List<string> text){
        CoroutineWork = false;
        //Make the player unable to make different events.
        mustAct = false;
        //If text is not null, we change the name of the variable? I dont get why.
        if(text != null){
            Lista = text;
        }

        if(dialog != null){
            //If the dialogBox is active, and if there is no more text to show, we are going to close it, and allow player to move.
            if(DialogBox.activeSelf){
                //If the dialog hasnt finished speaking, we will put the whole text on display.
                if(dialog.text != Lista[count-1]){
                    CoroutineWork = false;
                    dialog.text = Lista[count-1];
                //If it has finished, if there is no more text to show, we are closing it.
                }else{
                    if(count >= Lista.Count){
                        //Close the dialog box
                        DialogBox.SetActive(false);
                        //Tell the player that the conversation has finished.
                        talking = false;
                        //Empty the dialog;
                        dialog.text = "";
                        gameObject.SendMessage("Hide",null,SendMessageOptions.DontRequireReceiver);
                    //If there is text to show, get back up and show it.
                    }else{
                        //Show the action key
                        NextUI.text = ("Press \"" + general.actionKey.ToString() + "\" to continue.");
                        //Empty the dialog
                        dialog.text = "";
                        //Stop any working coroutine
                        CoroutineWork = false;
                        //Start the coroutine.
                        StartCoroutine(SlowlyShowText(dialog, Lista[count],0.03F));
                        count++;
                    }
                }
            
            //If its not active, we are activating it, and showing the needed text.
            }else{
                //If there is something to say.
                if(Lista.Count != 0){
                    //Activate the DialogBox
                    DialogBox.SetActive(true);
                    //We tell the player that a conversation has started.
                    talking = true;
                    //EMpty the dialog Box.
                    dialog.text = "";
                    //Set the count to 0
                    count = 0;
                    //Tell the player which action key its using.
                    NextUI.text = ("Press \"" + general.actionKey.ToString() + "\" to continue.");
                    //We close the SlowlyShowText coroutine if its active.
                    CoroutineWork = false;
                    //Show the text.
                    StartCoroutine(SlowlyShowText(dialog, Lista[0],general.textDelay));
                    //Add one to the count, so the next text will be displayed.
                    count++;
                }

            }
        }else if(TMPdialog != null){
            //If the dialogBox is active, and if there is no more text to show, we are going to close it, and allow player to move.
            if(DialogBox.activeSelf){
                //If the dialog hasnt finished speaking, we will put the whole text on display.
                if(TMPdialog.text != Lista[count-1]){
                    CoroutineWork = false;
                    TMPdialog.text = Lista[count-1];
                //If it has finished, if there is no more text to show, we are closing it.
                }else{
                    if(count >= Lista.Count){
                        //Close the dialog box
                        DialogBox.SetActive(false);
                        //Tell the player that the conversation has finished.
                        talking = false;
                        //Empty the dialog;
                        TMPdialog.text = "";
                        gameObject.SendMessage("Hide",null,SendMessageOptions.DontRequireReceiver);
                    //If there is text to show, get back up and show it.
                    }else{
                        //Show the action key
                        TMPNextUI.text = ("Press \"" + general.actionKey.ToString() + "\" to continue.");
                        //Empty the dialog
                        TMPdialog.text = "";
                        //Stop any working coroutine
                        CoroutineWork = false;
                        //Start the coroutine.
                        StartCoroutine(TMPSlowlyShowText(TMPdialog, Lista[count],0.03F));
                        count++;
                    }
                }
            
            //If its not active, we are activating it, and showing the needed text.
            }else{
                //If there is something to say.
                if(Lista.Count != 0){
                    //Activate the DialogBox
                    DialogBox.SetActive(true);
                    //We tell the player that a conversation has started.
                    talking = true;
                    //EMpty the dialog Box.
                    TMPdialog.text = "";
                    //Set the count to 0
                    count = 0;
                    //Tell the player which action key its using.
                    TMPNextUI.text = ("Press \"" + general.actionKey.ToString() + "\" to continue.");
                    //We close the SlowlyShowText coroutine if its active.
                    CoroutineWork = false;
                    //Show the text.
                    StartCoroutine(TMPSlowlyShowText(TMPdialog, Lista[0],general.textDelay));
                    //Add one to the count, so the next text will be displayed.
                    count++;
                }

            }
        }
            
        
    }
    //Coroutine used to show text at a determined speed.
    public IEnumerator SlowlyShowText(Text dialog, string txt, float txtSpeed){
        CoroutineWork = true;
            for(var x = 0; x < txt.Length && CoroutineWork; x++){
                dialog.text += txt[x];
                yield return new WaitForSeconds(txtSpeed);
            }
    }

    //Coroutine used to show text at a determined speed.
    public IEnumerator TMPSlowlyShowText(TextMeshProUGUI dialog, string txt, float txtSpeed){
        CoroutineWork = true;
            for(var x = 0; x < txt.Length && CoroutineWork; x++){
                dialog.text += txt[x];
                yield return new WaitForSeconds(txtSpeed);
            }
    }
}