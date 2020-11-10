using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnCometAndBullet : MonoBehaviour
{
    public static SpawnCometAndBullet Instance;
    private Dictionary<string, LinkedList<GameObject>> _poolsDictionary;
    private Transform _staticDeactivatedObjectsParentComet;
    public Transform _deactivatedObjectsParentPublComet;
    private Transform _staticDeactivatedObjectsParentBullet;
    public Transform _deactivatedObjectsParentPublBullet;
    [SerializeField] private GameObject _duloObj;

    private float _widthCam;

    public float WidthCam => _widthCam;

    private Vector2 _centrCam;
    public Vector2 CentrCam => _centrCam;

    private int _speedComet = 1;
    private Camera _cam;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _poolsDictionary = new Dictionary<string, LinkedList<GameObject>>();
        _speedComet = GameManagerSpace.Instance._serialScrObj.SpeedComet;
        _cam = Camera.main;
        if (_cam != null)
        {
            _centrCam = _cam.transform.position;
            _widthCam = _cam.orthographicSize * _cam.aspect;
            _staticDeactivatedObjectsParentComet = _deactivatedObjectsParentPublComet;
            _staticDeactivatedObjectsParentBullet = _deactivatedObjectsParentPublBullet;
            Init(_staticDeactivatedObjectsParentComet, true);
            Init(_staticDeactivatedObjectsParentBullet, false, true);
        }
    }

    private void Init(Transform pooledObjectsContainer, bool isComet = false, bool isBullet = false)
    {
        //создание пула
        if (isComet)
        {
            _staticDeactivatedObjectsParentComet = pooledObjectsContainer;
        }

        if (isBullet)
        {
            _staticDeactivatedObjectsParentBullet = pooledObjectsContainer;
        }
    }

    public GameObject GetGameObjectFromPool(GameObject prefab, bool isComet = false, bool isBullet = false)
    {
        //вытащить obj из пула,если он есть, если нет - создать
        if (!GameManagerSpace.Instance.IsGameOver)
        {
            GameObject result = null;

            if (!_poolsDictionary.ContainsKey(prefab.tag))
            {
                _poolsDictionary[prefab.tag] = new LinkedList<GameObject>();
            }

            if (isComet)
            {
                SpriteRenderer sR;
                Vector3 wind;
                float rnd = Random.Range(0.185f, 0.34785f); //случайный Scale кометы
                int rndqua = Random.Range(0, 360); //случайный угол вращения кометы
                if (_poolsDictionary[prefab.tag].Count > 0)
                {
                    if (_poolsDictionary[prefab.tag].First.Value)
                    {
                        result = _poolsDictionary[prefab.tag].First.Value;
                        sR = result.GetComponent<SpriteRenderer>();
                        wind = sR.bounds.extents;
                        result.transform.position =
                            new Vector3(
                                Random.Range(_centrCam.x - _widthCam + wind.x, _centrCam.x + _widthCam - wind.x),
                                _centrCam.y + _widthCam, 0);

                        result.transform.rotation = new Quaternion(0, 0, rndqua, 360);
                        result.transform.localScale = new Vector3(rnd, rnd, rnd);
                        result.GetComponent<Comet>().SpeedCommet = _speedComet;
                        _poolsDictionary[prefab.tag].Remove(result);
                        result.SetActive(true);
                    }
                    else _poolsDictionary.Clear();

                    return result;
                }

                result = Instantiate(prefab, _staticDeactivatedObjectsParentComet, true);
                sR = result.GetComponent<SpriteRenderer>();
                wind = sR.bounds.extents;
                result.GetComponent<Comet>().SpeedCommet = _speedComet;
                result.transform.rotation = new Quaternion(0, 0, rndqua, 360);
                result.transform.localScale = new Vector3(rnd, rnd, rnd);
                result.name = prefab.name;
                result.transform.position =
                    new Vector3(Random.Range(_centrCam.x - _widthCam + wind.x, _centrCam.x + _widthCam - wind.x),
                        _centrCam.y + _widthCam, 0);

                return result;
            }
            else if (isBullet)
            {
                if (_poolsDictionary[prefab.tag].Count > 0)
                {
                    result = _poolsDictionary[prefab.tag].First.Value;
                    result.transform.position = _duloObj.transform.position;
                    _poolsDictionary[prefab.tag].Remove(result);
                    result.SetActive(true);
                    return result;
                }

                result = Instantiate(prefab, _staticDeactivatedObjectsParentBullet, true);
                result.transform.position = _duloObj.transform.position;
                result.name = prefab.name;
                return result;
            }
            else
            {
                return null;
            }
        }
        else
            return null;
    }

    public void PutGameObjectToPool(GameObject target, bool isComet = false, bool isBullet = false)
    {
        // выключить объект и добавить его в словарь
        if (!GameManagerSpace.Instance.IsGameOver)
        {
            if (isComet)
            {
                target.transform.parent = _staticDeactivatedObjectsParentComet;
            }

            if (isBullet)
            {
                target.transform.parent = _staticDeactivatedObjectsParentBullet;
            }

            _poolsDictionary[target.tag].AddFirst(target);
            target.SetActive(false);
        }
        else
        {
            if (!_poolsDictionary.ContainsKey(target.tag))
            {
                _poolsDictionary.Remove(target.tag);
            }

            Destroy(target);
        }
    }
}