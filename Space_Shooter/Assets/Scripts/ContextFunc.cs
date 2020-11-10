using UnityEngine;

public class ContextFunc : MonoBehaviour
{
    public static ContextFunc Instance;
    public int _lastCompliteLevel; //номер последнего пройденного уровня
    public int _loadLevel;// Загружаемый уровень
    public bool _blTutorWasWatch; // Был ли просмотрен тутор

    public bool _infinityGame; // Здесь включается бесконечная игра(Автоматически переключится после окончания всех трех уровней)

    private bool _blFirstStart; //в первый раз зашли в игру?

    private void Awake()
    {
        Instance = this;
        _blFirstStart =LOAD_int("blFirstStart") == 1;

        if (!_blFirstStart) // если в первый раз зашли, то сохраняем эту переменную как true
        {
            SAVE_int("blFirstStart", 1);
        }
        else // иначе загружаем номер последнего уровеня
        {
            _lastCompliteLevel = LOAD_int("iLastCompliteLevel");
            _blTutorWasWatch = LOAD_int("blTutorWasWatch") == 1;
        }

        if (_lastCompliteLevel == 3) _infinityGame = true; //если прошли все уровни, то вкл бесконечная игра на выживание
    }


    public void SAVE_int(string keyname, int data)
    {
        PlayerPrefs.SetInt(keyname, data);
    }

    public int LOAD_int(string keyname)
    {
        return PlayerPrefs.GetInt(keyname, 0);
    }

    public void SAVE_float(string keyname, float data)
    {
        PlayerPrefs.SetFloat(keyname, data);
    }

    public float LOAD_float(string keyname)
    {
        return PlayerPrefs.GetFloat(keyname, 1.0f);
    }

    public void SAVE_string(string keyname, string str)
    {
        PlayerPrefs.SetString(keyname, str);
    }

    public string LOAD_string(string keyname)
    {
        return PlayerPrefs.GetString(keyname, "");
    }

    public void DELETE_Progress()
    {
        PlayerPrefs.DeleteAll();
        _infinityGame = false;
        SAVE_int("infinityGame", 0);
        Awake();
        LevelManager.instance.ReloadScene();
    }
}