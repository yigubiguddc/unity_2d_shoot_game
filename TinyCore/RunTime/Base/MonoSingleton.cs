using UnityEngine;

namespace Bronya
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>   //���ͱ��,T������MonoSingleton<T>���������ʵ��
    {
        public static T Instance{get; private set;} //ֻ���ڻ�������ʵ����ֵ
        protected virtual void Awake()  //�ܱ������鷽��
        {
            if(Instance == null)
            {
                Instance = this as T;
            }
            else
            {
                Debug.LogError($"Get two instances OF this class{GetType()}");
            }
        }
    }
}

