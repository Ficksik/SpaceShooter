using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed=8;
    private IEnumerator Move()
    {
        while (transform.position.y > SpawnCometAndBullet.Instance.CentrCam.y + SpawnCometAndBullet.Instance.WidthCam)
        {
            transform.position += new Vector3(0,Time.deltaTime*_speed,0); //двигаем пулю вверх
            yield return null;
        }
        SpawnCometAndBullet.Instance.PutGameObjectToPool(gameObject,false,true);
    }
    private void Start()
    {
        //подписка на рассылку событий
        GameManagerSpace.EventWin += EventWinHandler;
        GameManagerSpace.EventLose += EventLoseHandler;
        StartCoroutine(Move());
    }
    void OnDisable()
    { //отписка
        GameManagerSpace.EventWin -= EventWinHandler;
        GameManagerSpace.EventLose -= EventLoseHandler;
    }
    private void EventWinHandler()
    {//event
        SpawnCometAndBullet.Instance.PutGameObjectToPool(gameObject, false, true);
    }
    private void EventLoseHandler()
    {//event
        SpawnCometAndBullet.Instance.PutGameObjectToPool(gameObject, false, true);
    }
}
