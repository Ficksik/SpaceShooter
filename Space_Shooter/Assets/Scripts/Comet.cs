using System.Collections;
using UnityEngine;

public class Comet : MonoBehaviour
{
   [SerializeField] private GameObject _particlesSystemGameObj;
   [SerializeField] private  int _speedComet;
   public int SpeedCommet
   {
       set => _speedComet = value;
   }

   private int _iRotateComet = 1;//-1 -влево, 1 - вправо
   private float _widthCam => SpawnCometAndBullet.Instance.WidthCam;
   private Vector2 CentrCam => SpawnCometAndBullet.Instance.CentrCam;
    private IEnumerator Move()
    {
        while ((transform.position.y <
                CentrCam.y -_widthCam))
        {
            float speedBoost = ((float)GameManagerSpace.Instance.Score / 500); // чем больше счет, тем больше скорость астеройдов
     
            speedBoost += _speedComet;
            var pos = transform.position;
            transform.Rotate(new Vector3(0,0,Time.deltaTime*_iRotateComet),Space.World); // вращаем комету при падении
            transform.position = new Vector3(pos.x, pos.y - Time.deltaTime* (speedBoost), 0);
            yield return null;
        }
        SpawnCometAndBullet.Instance.PutGameObjectToPool(gameObject,true);
    }
    private void OnDisable()
    {
        //отписка
        GameManagerSpace.EventWin -= EventWinHandler;
        GameManagerSpace.EventLose -= EventLoseHandler;
        //если комета уничтожена в пределах видимости, то создается эффект взрыва
        if (transform.position.y > CentrCam.y - _widthCam &&  !GameManagerSpace.Instance.IsGameOver)
        {
            GameObject obj = Instantiate(_particlesSystemGameObj);
            obj.transform.position = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //столкновение с пулей
        if (collision.gameObject.CompareTag("bullet"))
        {

            if (!GameManagerSpace.Instance.IsGameOver)
            {
                LevelManager.instance.ClickSound_boomComet();
                GameManagerSpace.Instance.Score +=  GameManagerSpace.Instance._serialScrObj.CostBoomComet;
            }
            SpawnCometAndBullet.Instance.PutGameObjectToPool(gameObject,true);
            SpawnCometAndBullet.Instance.PutGameObjectToPool(collision.gameObject,false,true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //столкновение с кораблем
        if (collision.gameObject.CompareTag("Player"))
        {
            LevelManager.instance.ClickSound_DTPCometAndShip();
            SpawnCometAndBullet.Instance.PutGameObjectToPool(gameObject, true);
            Player.MyHp--;
        }
    }
    private void Start()
    {
        //подписка на рассылку 
        GameManagerSpace.EventWin += EventWinHandler;
        GameManagerSpace.EventLose += EventLoseHandler;
        _iRotateComet = Random.Range(-12, 12);//случаная скорость вращения
        StartCoroutine(Move());
    }

    private void EventWinHandler()
    {//event
        SpawnCometAndBullet.Instance.PutGameObjectToPool(gameObject, true);
    }

    private void EventLoseHandler()
    {//event
        SpawnCometAndBullet.Instance.PutGameObjectToPool(gameObject, true);
    }
}
