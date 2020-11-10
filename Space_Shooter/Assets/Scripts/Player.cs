using System.Collections;
using UnityEngine;



public class Player : MonoBehaviour
{
    // Message Interface
    public delegate void StartEventHandler();
    public delegate void ChangeHealthEventHandler(int health);
    public delegate void DeathEventHandler();
    public delegate void OverheatEventHandler(float flOverhead);

    public static event StartEventHandler StartEvent;
    public static event ChangeHealthEventHandler ChangeHealthEvent;
    public static event DeathEventHandler DeathEvent;
    public static event OverheatEventHandler OverheadEvent;
    //
    private float _mySpeed;
    public static int MyHp;
    private ScriptableObject _scrObj;
    private Rigidbody2D _rb;
    public ParticleSystem _engineFire;
    private float _widthCam; // ширина камеры
    private float _hightCam; // ширина камеры
    private Vector2 _centrCam; // центр камеры (это ее пивот)
    float _minX, _maxX, _minY, _maxY;
    private Camera _cam;
    private float _overHeat ;//перегрев
    private bool _blockFire ;

    public GameObject _prefBullet;
    public ParticleSystem[] _sopla;
    private Vector3 _startPos;

    private SpriteRenderer _sR;
    private Vector3 _width;

    private float _cooldown ;
    private float _maxTimeCooldown ;

    void Start()
    {
        _startPos = transform.position;
        _scrObj = GameManagerSpace.Instance._serialScrObj;
        _maxTimeCooldown =_scrObj.CooldownFire;
        _cam = Camera.main;
        _mySpeed = _scrObj.ShipSpeed;
        MyHp = _scrObj.ShipHp;
        _rb = GetComponent<Rigidbody2D>();
        _sR = GetComponent<SpriteRenderer>();
        var aspect = _cam.aspect;
        _widthCam = _cam.orthographicSize * aspect;
        _centrCam = _cam.transform.position;

        _minX = _centrCam.x - _widthCam;
        _maxX = _centrCam.x + _widthCam;
        _minY = _centrCam.y - _widthCam / aspect;
        _maxY = _centrCam.y + _widthCam / aspect;

        _width = _sR.bounds.extents; // получаем размеры объекта (вернее половины ширины, высоты, глубины)
        
            //подписка на рассылку уведомлений
            GameManagerSpace.EventWin += EventWinHandler;
            GameManagerSpace.RestartGameNow += RestartGameNowHandler;
            GameManagerSpace.EventLose += EventLoseHandler;
        
        if (StartEvent != null)
        {
            StartEvent();
        }

        StartCoroutine(ChangeHealth()); //запуск рассылки
        StartCoroutine(WaitFire());
        StartCoroutine(WaitMoveButton());
    }
    void OnDisable()
    {
        GameManagerSpace.EventWin -= EventWinHandler;
        GameManagerSpace.RestartGameNow -= RestartGameNowHandler;
        GameManagerSpace.EventLose -= EventLoseHandler;
    }
    private void EventWinHandler()
    {//event
        transform.position = _startPos;
        MyHp = _scrObj.ShipHp;
    }
    private void RestartGameNowHandler()
    {//event
        transform.position = _startPos;
        MyHp = _scrObj.ShipHp;
        StartCoroutine(ChangeHealth());
    }
    private void EventLoseHandler()
    {//event
        transform.position = _startPos;
        MyHp = _scrObj.ShipHp;
    }

    private IEnumerator WaitMoveButton()
    {
        while (true)
        {
            if (!GameManagerSpace.Instance.IsGameOver)
            {
                float moveHor = Input.GetAxis("Horizontal");
                float moveVert = Input.GetAxis("Vertical");

                // Эффекты
                if (moveVert > 0) _engineFire.Play();
                else _engineFire.Stop();
                if (moveVert < 0) { _sopla[3].Play(); _sopla[2].Play(); }
                else
                {
                    _sopla[3].Stop(); _sopla[2].Stop();
                }
                if (moveHor > 0) _sopla[0].Play();
                else _sopla[0].Stop();
                if (moveHor < 0) _sopla[1].Play();
                else _sopla[1].Stop();

                transform.rotation = new Quaternion(0, 0, 0, 0);
                //измените вектор движения
                //z останется 0, что-бы корабль не двигался по z-оси
                _rb.velocity = new Vector2(moveHor * _mySpeed * 1.2f, moveVert * _mySpeed);

                transform.position = CalcPosition();
            }

            yield return null;
        }
        
    }

    private Vector3 CalcPosition()
    {
        var pos = transform.position;
        if (pos.x < _minX + _width.x)
        {
            pos = new Vector3(_minX + _width.x, pos.y, 0);
        }
        else if (pos.x > _minX - _width.x)
        {
            pos = new Vector3(_minX - _width.x, pos.y, 0);
        }

        if (pos.y < _minY + _width.y)
        {
            pos = new Vector3(pos.x, _minY + _width.y, 0);
        }
        else if (pos.y > _minY - _width.y)
        {
            pos = new Vector3(pos.x, _minY - _width.y, 0);
        }
        return pos;
    }

    private IEnumerator WaitFire()
    {
        if (Input.GetMouseButton(0) && !GameManagerSpace.Instance.IsGameOver)
        {
            if (_cooldown <= 0 && !_blockFire) //выстрел по нажатию левой кнопки мыши и истечении таймера отката
            {
                LevelManager.instance.ClickSound_blaster();
                SpawnCometAndBullet.Instance.GetGameObjectFromPool(_prefBullet, false, true);
                _cooldown = _maxTimeCooldown;
            }
            else _cooldown -= Time.deltaTime;
            if (_overHeat < 1 && !_blockFire)
                _overHeat += Time.deltaTime/2;
            if(_overHeat>=1)
            {
                _overHeat = 1;
                _blockFire = true;
                StartCoroutine(WaitToCool());
            }
        }
        else
        {
            if(_overHeat>0&& !_blockFire)
                _overHeat -= Time.deltaTime;
        }

        yield return null;
    }

    IEnumerator WaitToCool()
    {
        yield return new WaitForSeconds(3f);
        _overHeat = 0;
        _blockFire = false;
    }

    IEnumerator ChangeHealth()
    {//пока есть жизни сообщать обэтом в рассылке
        yield return new WaitForSeconds(0.02f);

        if (ChangeHealthEvent != null)
        {
            ChangeHealthEvent(MyHp);
        }
        if(OverheadEvent != null)
        {
            OverheadEvent(_overHeat);
        }
        if (MyHp == 0)
        {
            if (DeathEvent != null)
            {
                DeathEvent();
            }
        }
        else
        {
            StartCoroutine(ChangeHealth());
        }
    }
}


