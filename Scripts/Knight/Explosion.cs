using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("��ȡ�������Ͷ�������")]
    private Animator _animator;
    private AnimatorStateInfo _info;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _info = _animator.GetCurrentAnimatorStateInfo(0);
        if(_info.normalizedTime > 1)
        {
            //Destroy(gameObject);
            ObjectPool.Instance.PushObject(gameObject);
        }
    }
}
