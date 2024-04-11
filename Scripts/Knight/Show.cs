using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Show : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(ShowGouPai());
        }
    }
    IEnumerator ShowGouPai()
    {

        _animator.SetBool(AnimatorHash.IsShow, true);
        yield return new WaitForSeconds(2);
        _animator.SetBool(AnimatorHash.IsShow, false);
    }
}
