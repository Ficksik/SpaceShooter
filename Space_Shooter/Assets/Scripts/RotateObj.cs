using System.Collections;
using UnityEngine;

public enum Axis
{
    X,
    Y,
    Z
}

public class RotateObj : MonoBehaviour
{
    public Axis _axis;
    public float _speedRotate = 1;

    private void OnEnable()
    {
        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        switch ((int)_axis)
        {
            case 0:
                while (true)
                {
                    transform.Rotate(Time.deltaTime * _speedRotate, 0, 0, Space.World);
                    yield return null;
                }
            case 1:
                while (true)
                {
                    transform.Rotate(0, 0, Time.deltaTime * _speedRotate, Space.World);
                    yield return null;
                }
            case 2:
                while (true)
                {
                    transform.Rotate(0, 0, Time.deltaTime * _speedRotate, Space.World);
                    yield return null;
                }
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
