using UnityEngine;
using UnityEngine.UI;


public class LevelUi : MonoBehaviour
{ 
    // Message Interface
    public delegate void SelectEventHandler (int numbLevel);
    public static event SelectEventHandler SelectEvent;
    //
    public int _iNumbLevel = 1;
    private Image _bgImage;
    private Text _textLevel;
    private Sprite[] _bgSprites;
    private GameObject _lockObj;

    private void Awake()
    {
        //подписка
        MainMenuObj.StartEvent += StartEventHandler;
        MainMenuObj.SelectControlRetern += SelectControlReternHandler;
        
       _bgImage = transform.GetChild(0).GetComponent<Image>();
       _lockObj = transform.GetChild(1).gameObject;
        _textLevel = transform.GetChild(2).GetComponent<Text>();

        if (_textLevel) _textLevel.text ="Level "+ _iNumbLevel;
    }

    private void StartEventHandler(Sprite[] bgSpritesEvent)
    {//при старте получаем масссив 2 звезд(серой и оранжевой)
        _bgSprites = bgSpritesEvent;
        if (ContextFunc.Instance._lastCompliteLevel >= _iNumbLevel) // настройки отображения пройденных уровней
        {
            if (_lockObj) _lockObj.SetActive(false);
            if (_bgImage) _bgImage.sprite = _bgSprites[1];
            if (_textLevel) _textLevel.gameObject.SetActive(true);
        }
        else if (ContextFunc.Instance._lastCompliteLevel == _iNumbLevel -1) //доступ к уровню,котороый надо пройти
        {
           if(_lockObj) _lockObj.SetActive(false);
            if (_bgImage) _bgImage.sprite = _bgSprites[0];
            if (_textLevel) _textLevel.gameObject.SetActive(true);
        }
        else //блокировать уровни, которые еще далеко
        {
            if (_lockObj) _lockObj.SetActive(true);
            if (_bgImage) _bgImage.sprite = _bgSprites[0];
            if (_textLevel) _textLevel.gameObject.SetActive(false);
        }
    }
    void OnDisable()
    {//отписка
        MainMenuObj.StartEvent -= StartEventHandler;
        MainMenuObj.SelectControlRetern -= SelectControlReternHandler;
    }

    private void Select()
    {
        if (_lockObj.activeSelf == false)
        {//изменять цвет при выборе уровня
            Vector4 Color;
            Color = new Vector4(253 / 255.0f, 255 / 255.0f, 172 / 255.0f, 1);
            _bgImage.color = Color;
            
        }
    }
    private void Deselect()
    { //возвращать цвет при выборе другого уровня
        _bgImage.color = new Color(255, 255, 255);
    }

    private void SelectControlReternHandler(int lastSelect)
    {
        if(lastSelect== _iNumbLevel) //выбранный уровеня это я?
        {
            Select();
        }
        else
        {
            Deselect();
        }
    }

}
