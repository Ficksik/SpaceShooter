using UnityEngine;

[CreateAssetMenu(fileName = "New Script_Object", menuName = "Script_Object", order = 51)]
public class ScriptableObject : UnityEngine.ScriptableObject
{

    [SerializeField]
    private float _shipSpeed;
    [SerializeField]
    private int _shipHp;

    [SerializeField]
    private int _speedComet;
    [SerializeField]
    private float _cooldownFire = 0.12f;
    [SerializeField]
    private int _standartMissionOnelvl = 1000;
    [SerializeField]
    private float _cooldownSpawnComet = 1; //минимальное время на перерыв
    [SerializeField]
    private int _costBoomComet = 50;

    public ScriptableObject()
    {
        _speedComet = 1;
    }

    public int CostBoomComet => _costBoomComet;
    public float CooldownSpawnComet => _cooldownSpawnComet;
    public int StandartMissionOnelvl => _standartMissionOnelvl;
    public float CooldownFire => _cooldownFire;
    public int SpeedComet => _speedComet;

    public float ShipSpeed => _shipSpeed;

    public int ShipHp => _shipHp;
} 

