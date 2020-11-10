using UnityEngine;
using UnityEngine.UI;

public class ManagerPanel : MonoBehaviour
{
   [SerializeField] private Image _panelWin;
   [SerializeField]  private Image _panelLose;
   [SerializeField]   private Image _panelHp;
   [SerializeField]  private Image _panelMission;
   [SerializeField]  private Text _scoretxt;
   [SerializeField]  private Image _slider;

    private void Start()
    {
        //подписка на рассылку уведомлений
        GameManagerSpace.EventWin += EventWinHandler;
        GameManagerSpace.RestartGameNow += RestartGameNowHandler;
        GameManagerSpace.EventLose += EventLoseHandler;
        Player.OverheadEvent += SetValueSlider;
    }

    private void SetActivePanel_Lose()
    {
        if (_panelWin) _panelWin.gameObject.SetActive(false);
        if (_panelLose) _panelLose.gameObject.SetActive(true);
        if (_panelHp) _panelHp.gameObject.SetActive(false);
        if (_panelMission) _panelMission.gameObject.SetActive(false);
    }

    private void SetActivePanel_Win()
    {
        if (_panelWin) _panelWin.gameObject.SetActive(true);
        if (_panelLose) _panelLose.gameObject.SetActive(false);
        if (_panelHp) _panelHp.gameObject.SetActive(false);
        if (_panelMission) _panelMission.gameObject.SetActive(false);
        if (_scoretxt) _scoretxt.text = "Score: "+GameManagerSpace.Instance.Score;
    }

    private void SetActivePanel_Hp()
    {
        if (_panelWin) _panelWin.gameObject.SetActive(false);
        if (_panelLose) _panelLose.gameObject.SetActive(false);
        if (_panelHp) _panelHp.gameObject.SetActive(true);
        if (_panelMission) _panelMission.gameObject.SetActive(!ContextFunc.Instance._infinityGame);
    }

    public void GoMenu()
    {
        LevelManager.instance.ClickSound_button();
        LevelManager.instance.LoadScene("Menu");
    }

    private void EventWinHandler()
    {//event
        SetActivePanel_Win();
    }
    private void RestartGameNowHandler()
    {//event
        SetActivePanel_Hp();
    }
    private void EventLoseHandler()
    {//event
        if (!ContextFunc.Instance._infinityGame)
        {
            SetActivePanel_Lose();
        }
        else
        {
            SetActivePanel_Win();
        }
    }

    private void SetValueSlider(float value)
    {
        _slider.fillAmount = value;
    }
    void OnDisable()
    {
        GameManagerSpace.EventWin -= EventWinHandler;
        GameManagerSpace.RestartGameNow -= RestartGameNowHandler;
        GameManagerSpace.EventLose -= EventLoseHandler;
        Player.OverheadEvent -= SetValueSlider;
    }
}
