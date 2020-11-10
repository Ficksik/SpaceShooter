using System.Collections;
using UnityEngine;

public class Tutor : MonoBehaviour
{
    Animator _anim;
    private static readonly int CallTutorial = Animator.StringToHash("CallTutorial");

    private void OnEnable()
    {
        if (!ContextFunc.Instance._blTutorWasWatch)
        {
            _anim=transform.GetChild(0).GetComponent<Animator>();
            _anim.SetTrigger(CallTutorial);
            ContextFunc.Instance.SAVE_int("blTutorWasWatch", 1);
            StartCoroutine(WainEndTutor());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator WainEndTutor()
    {
        yield return new WaitForSeconds(1);
        var info = _anim.GetCurrentAnimatorStateInfo(0);
        if (info.IsName ("TutorialAnimation"))
        {
            StartCoroutine(WainEndTutor());           
        }
        else
        {
            Destroy(gameObject);
        }
    }


}
