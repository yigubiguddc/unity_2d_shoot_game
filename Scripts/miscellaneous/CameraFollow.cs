using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]       //���л���ǩ����c#�е�����(attribute)
public struct MapBound      //�ṹ��������ͼ�߽�
{
    public float xMin;
    public float yMin;
    public float xMax;
    public float yMax;
}
public class CameraFollow : MonoBehaviour
{
    //����ֱ����д����ͷ����
    public Transform target;
    private float smoothTime = 0.1f;
    private float _offsetZ;     //z��ƫ��ֵ
    private Vector3 _currentVelocity;
    public MapBound mapBound;
    private void Start()
    {
        if(target ==null)
        {
            Debug.Log("Can't find the player, please check it!");
            return;
        }
        _offsetZ = (transform.position - target.position).z;
        //����Ҫ��һ�㣺_offsetZ ��ֵͨ�� (transform.position - target.position).z ����õ���Ҳ��������ͷ�ĳ�ʼλ�ü�ȥĿ�����ĳ�ʼλ�õ�Z�Ჿ�֣���ʾ����ͷ��Ŀ�������Z���ϵĳ�ʼƫ������
        //���ǵĳ�ʼƫ������������ͷ��target�����Ϸ�������ֻҪ���_offsetZ�����仯���������FixUpdate�ж��ᱻ����
    }
    public void FixedUpdate()
    {
        if (target == null) return;
        Vector3 targetPosition = target.position + Vector3.forward * _offsetZ;
        Vector3 newPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
        //newPosition.x = Mathf.Clamp(newPosition.x, mapBound.xMin, mapBound.xMax);
        //newPosition.y = Mathf.Clamp(newPosition.y, mapBound.yMin, mapBound.yMax);
        //������Ƕ������λ�ý��������ģ�ͨ�����������׼ȷ���꣬
        transform.position = newPosition;
    }

}
