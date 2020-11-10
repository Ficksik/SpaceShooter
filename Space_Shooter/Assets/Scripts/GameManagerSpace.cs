using UnityEngine;
using UnityEngine.UI;

public class GameManagerSpace : MonoBehaviour
{ 
    // Message Interface
    public delegate void EventWinHandler();
    public delegate void RestartGameNowHandler();
    public delegate void EventLoseGameHandler();

    public static event EventWinHandler EventWin;
    public static event RestartGameNowHandler RestartGameNow;
    public static event EventLoseGameHandler EventLose;
    //
    public static GameManagerSpace Instance;
    public ScriptableObject _serialScrObj;
    public Text _hptext; 
    public Text _scoretext;
    public Text _missionScoretxt;
    private  bool _isGameOver ;

    public bool IsGameOver => _isGameOver;

    private  int _score ;
    public int Score
    {
        get => _score;
        set => _score = value;
    }

    public GameObject[] _prefComet;
    private float _timer;
    private float _limitTimer = 1;//таймер cooldown создания кометы
    private int _loadLevel ; //номер загружаемого уровеня

    private void Awake()
    {
        Instance = this;
        _loadLevel = ContextFunc.Instance._loadLevel;
    }
    private void Start()
    {//подписка на рассылку уведомлений
        Player.StartEvent += Player_StartEventHandler;
        Player.ChangeHealthEvent += Player_ChangeHealthEventHandler;
        Player.DeathEvent += Player_DeathEventHandler;
        _score = 0;
        _isGameOver = false;
        if (_missionScoretxt)
            if (!ContextFunc.Instance._infinityGame) // если все уровни прошли и играем в режим на выживание ,выключаем табло миссии
            {
                _missionScoretxt.text = _serialScrObj.StandartMissionOnelvl * _loadLevel + "";
            }
            else
            {
                _missionScoretxt.text = "∞";
            }
        _limitTimer = _serialScrObj.CooldownSpawnComet;
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= Random.Range(_limitTimer, _limitTimer+0.5f) && !_isGameOver)
        {//спавн кометы
            SpawnCometAndBullet.Instance.GetGameObjectFromPool(_prefComet[Random.Range(0, _prefComet.Length)], true);
            _timer = 0;
        }

    }

    void FixedUpdate()
    {
        if (_scoretext) _scoretext.text = _score + ""; //отображение счета
        if (!_isGameOver&& !ContextFunc.Instance._infinityGame && _score >= _serialScrObj.StandartMissionOnelvl * _loadLevel)
            WinGame();//победа!
    }
    

    public void Restart()
    {
        LevelManager.instance.ClickSound_button();

        if (RestartGameNow != null)
        {
            RestartGameNow(); // рассылка сообщения
        }
        _score = 0;
        _isGameOver = false;
    }

    public void WinGame()
    {
        LevelManager.instance.ClickSound_WinMusic();
        if (ContextFunc.Instance._lastCompliteLevel + 1 == ContextFunc.Instance._loadLevel)
        {//сохраниение прохождения уровня
            ContextFunc.Instance._lastCompliteLevel++;
            ContextFunc.Instance.SAVE_int("iLastCompliteLevel", ContextFunc.Instance._lastCompliteLevel);
        }
        if (EventWin != null)
        {
            EventWin();// рассылка сообщения
        }
        _isGameOver = true;
    }




    private void Player_DeathEventHandler()
    {
        LevelManager.instance.ClickSound_GameOver();
        _isGameOver = true;
        if (EventLose != null)
        {
            EventLose(); // рассылка сообщения
        }
    }

    private void Player_ChangeHealthEventHandler(int healthValue)
    {
       if(_hptext) _hptext.text = healthValue.ToString();
    }

    private void Player_StartEventHandler()
    {
        //  StateInfo.text = "Im going";
    }

    void OnDisable()
    {
        Player.StartEvent -= Player_StartEventHandler;
        Player.ChangeHealthEvent -= Player_ChangeHealthEventHandler;
        Player.DeathEvent -= Player_DeathEventHandler;
    }
}

