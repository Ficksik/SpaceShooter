using System.Collections;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{ // движение до трех слоев фона

    [SerializeField] private enum ProjectMode { MoveX = 0, MoveY = 1 };
    [SerializeField] private ProjectMode _projectMode = ProjectMode.MoveX;

    [SerializeField] private MeshRenderer _firstBg;
    [SerializeField] private float _firstBgSpeed = 0.01f;

    [SerializeField] private MeshRenderer _secondBg;
    [SerializeField] private float _secondBgSpeed = 0.05f;
  
    [SerializeField] private MeshRenderer _thirdBg;
    [SerializeField] private float _thirdBgSpeed = 0.1f;
    [SerializeField] private bool _menu;
    private int[] plunetNumb = new int[] {3,5,1 }; // номера планет для уровеней
    private Vector2 _savedFirst;
    private Vector2 _savedSecond;
    private Vector2 _savedThird;
    private bool _play = true;
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");
    

    void Start()
    {
        if (ContextFunc.Instance._infinityGame)
        {
             _secondBg.material.mainTexture = LoadTexture(18+Random.Range(0, 16));
        }
        else
        {
            if(!_menu)
             _secondBg.material.mainTexture = LoadTexture(18+plunetNumb[ContextFunc.Instance._loadLevel-1]);
        }
        if (_firstBg) _savedFirst = _firstBg.sharedMaterial.GetTextureOffset(MainTex);
        if (_secondBg) _savedSecond = _secondBg.sharedMaterial.GetTextureOffset(MainTex);
        if (_thirdBg) _savedThird = _thirdBg.sharedMaterial.GetTextureOffset(MainTex);
        StartCoroutine(MoveUpdate());
    }

    void Move(MeshRenderer mesh, Vector2 savedOffset, float speed)
    {
        Vector2 offset;
        float tmp = Mathf.Repeat(Time.time * speed, 1);
        if (_projectMode == ProjectMode.MoveY) offset = new Vector2(savedOffset.x, tmp);
        else offset = new Vector2(tmp, savedOffset.y);
        mesh.sharedMaterial.SetTextureOffset(MainTex, offset);
    }


    private IEnumerator MoveUpdate()
    {
        while (_play)
        {
            if (_firstBg) Move(_firstBg, _savedFirst, _firstBgSpeed);
            if (_secondBg) Move(_secondBg, _savedSecond, _secondBgSpeed);
            if (_thirdBg) Move(_thirdBg, _savedThird, _thirdBgSpeed);
            yield return null;
        }
    }
    void OnDisable()
    {
        if (_firstBg) _firstBg.sharedMaterial.SetTextureOffset(MainTex, _savedFirst);
        if (_secondBg) _secondBg.sharedMaterial.SetTextureOffset(MainTex, _savedSecond);
        if (_thirdBg) _thirdBg.sharedMaterial.SetTextureOffset(MainTex, _savedThird);
    }

    private Texture2D LoadTexture(int index)
    {
        return   Resources.Load<Sprite>("planets16/planet_"+index ).texture;
    }
}