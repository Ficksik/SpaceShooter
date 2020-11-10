using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour
{// мерцание панели
    float timerTick = 0;
    int numScin = 0;
    public Sprite[] scinWin;
    public Image PanelImage;


    void Update()
    {
        timerTick += Time.deltaTime;
        if (timerTick >= 1)
        {
            if (numScin == 0) numScin++;
            else numScin--;
            timerTick = 0;
            PanelImage.sprite=  scinWin[numScin];
        }
    }

}
