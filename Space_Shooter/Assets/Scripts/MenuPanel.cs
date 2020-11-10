using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    public Button buttonStart;
    private Text textButton;
    private int iLoadLevel=0;


    public void StartGame()
    {
        LevelManager.instance.ClickSound_button();
        ContextFunc.Instance._loadLevel = iLoadLevel;
        LevelManager.instance.LoadScene("Game");    
    }

    private void Start()
    {
        textButton = buttonStart.transform.GetChild(0).GetComponent<Text>();
        if (ContextFunc.Instance._infinityGame)
        {
            textButton.text = "Survival game";
        }
        else
        {
            textButton.text = "Start game";
        }
    }

    private void Awake()
    {     
        MainMenuObj.SelectControlRetern += SelectControlReternHandler;
    }
    private void OnDisable()
    {
        MainMenuObj.SelectControlRetern -= SelectControlReternHandler;
    }

    private void SelectControlReternHandler(int lastSelect)
    {
        if (buttonStart)
            if (lastSelect==0 || lastSelect > ContextFunc.Instance._lastCompliteLevel + 1)
            { 
             buttonStart.interactable = false;   
            }
            else
            {            
                buttonStart.interactable = true;
            }
        iLoadLevel = lastSelect;
    }
    
    public void DeleteProgress()
    {
        LevelManager.instance.ClickSound_button();
        ContextFunc.Instance.DELETE_Progress();
    }

    public void Exit()
    {
        LevelManager.instance.ClickSound_button();
        LevelManager.instance.ExitGame();
    }
}
