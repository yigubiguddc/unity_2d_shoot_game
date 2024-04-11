using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Bronya
{
    //资源管理器
    //1.AssetBundleManager
    //2.ObjectManager
    //3.ResourceManager 对不需要实例化的资源进行管理(音频等)
    //4.Pool
    //既然是程序架构，内存池肯定不会设计的简单，最起码会实现一个资源的引用计数功能
    public class SourceManager : MonoSingleton<SourceManager>   //基类单例模式继承
    {
       /* public static SourceManager Instance;
        private void Awake()
        {
            Instance = this;
        }*/

        //1.同步加载
        public T Load<T>(string path) where T : Object //unity中组件
        {
            T resource = Resources.Load<T>(path);
            //1.Audio Text Json and so on,no use to be Instantiate(实例化)
            //GameObject
            return resource is GameObject ? Instantiate(resource) : resource;
        }

        //无返回值，因为在内部执行了资源加载成功回调
        public void LoadAsync<T>(string path, UnityAction<T> callback) where T : Object
        {
            //
            StartCoroutine(LoadAsyncInternal(path,callback));
        }

        //2.异步加载Asynchronous Loading
        private IEnumerator LoadAsyncInternal<T>(string path, UnityAction<T> callback) where T : Object     //这里Where用来约束T的类型，只有T时Object时才满足
        {
        //使用引擎提供的LoadAsync方法（异步资源加载方法）
            ResourceRequest resourceRequest = Resources.LoadAsync<T>(path);
            yield return resourceRequest;   //资源加载完成之后才会往下继续运行
            if(resourceRequest.asset is GameObject)
            {
                //Instantiate
                //回调方法
                callback(Instantiate(resourceRequest.asset)as T);
            }
            else
            {
                callback(resourceRequest.asset as T);
            }
        }
        //public Popuptext popupText;
    }
}