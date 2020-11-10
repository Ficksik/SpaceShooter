using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuObj : MonoBehaviour
{    // Message Interface
    public delegate void StartEventHandler(Sprite[] bgSpritesEvent);
    public static event StartEventHandler StartEvent;
    public delegate void SelectControlEventHandler(int lastSelect);
    public static event SelectControlEventHandler SelectControlRetern;

    //
    private Camera camera;
    private int lastSelectLevel = 0;
    public Sprite[] bgSprites;

    void Start()
    {
        camera = Camera.main;
        if (StartEvent != null)
        {
            StartEvent(bgSprites); //разослать при старте массив с двумя звездами
        }

        lastSelectLevel = ContextFunc.Instance._lastCompliteLevel+1;
        if (SelectControlRetern != null)
        {
            SelectControlRetern(lastSelectLevel); //выбрать при старте уровень ,который предстоит пройти
        }
    }


    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            var p = camera.ScreenToWorldPoint(Input.mousePosition);

            var hit2D = Physics2D.Raycast(p, Vector2.zero); // Vector2.zero если нужен рейкаст именно под курсором
            if(hit2D.transform)
            if (hit2D.transform.GetComponent<LevelUi>() != null)
            {   
                int iSelectLevel = hit2D.transform.GetComponent<LevelUi>()._iNumbLevel;
                  
                if (SelectControlRetern != null)
                {
                        if (lastSelectLevel != iSelectLevel)
                        {
                            SelectControlRetern(iSelectLevel);
                            lastSelectLevel = iSelectLevel;
                            LevelManager.instance.ClickSound_button();
                        }
                }
            }
        }
    }
 
}


