using UnityEngine;
public class EasyCollect : MonoBehaviour
{
    [SerializeField] private Transform PlayerEntity;
    public LayerMask WhatToFollow;
    private Vector3 _currentVelocity;
    public float Radius = 2.5f;
    //CollectSpeed
    public float SmoothTime = 0.1f;

    private void Update()
    {
        DetectPlayers();
    }
    //��Ҽ��
    private void DetectPlayers()
    {
        Collider2D[] follows = Physics2D.OverlapCircleAll(transform.position, Radius,WhatToFollow);
        //ѭ������tag��ǩ�Ƿ������
        foreach (var follow in follows)
        {
            if (follow.CompareTag("Player"))
            {
                // ����ÿһ����⵽��Player����������Ҫ�Ĳ���
                //Debug.Log("Detected a Player named: " + follow.name);
                Vector3 newposition = Vector3.SmoothDamp(transform.position, follow.transform.position, ref _currentVelocity,SmoothTime);
                transform.position = newposition;
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
