using UnityEngine;

namespace Bronya
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>   //泛型编程,T必须是MonoSingleton<T>或其子类的实例
    {
        public static T Instance{get; private set;} //只能在基类对这个实例赋值
        protected virtual void Awake()  //受保护的虚方法
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

