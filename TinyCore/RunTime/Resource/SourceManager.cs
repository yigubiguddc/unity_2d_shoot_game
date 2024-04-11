using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Bronya
{
    //��Դ������
    //1.AssetBundleManager
    //2.ObjectManager
    //3.ResourceManager �Բ���Ҫʵ��������Դ���й���(��Ƶ��)
    //4.Pool
    //��Ȼ�ǳ���ܹ����ڴ�ؿ϶�������Ƶļ򵥣��������ʵ��һ����Դ�����ü�������
    public class SourceManager : MonoSingleton<SourceManager>   //���൥��ģʽ�̳�
    {
       /* public static SourceManager Instance;
        private void Awake()
        {
            Instance = this;
        }*/

        //1.ͬ������
        public T Load<T>(string path) where T : Object //unity�����
        {
            T resource = Resources.Load<T>(path);
            //1.Audio Text Json and so on,no use to be Instantiate(ʵ����)
            //GameObject
            return resource is GameObject ? Instantiate(resource) : resource;
        }

        //�޷���ֵ����Ϊ���ڲ�ִ������Դ���سɹ��ص�
        public void LoadAsync<T>(string path, UnityAction<T> callback) where T : Object
        {
            //
            StartCoroutine(LoadAsyncInternal(path,callback));
        }

        //2.�첽����Asynchronous Loading
        private IEnumerator LoadAsyncInternal<T>(string path, UnityAction<T> callback) where T : Object     //����Where����Լ��T�����ͣ�ֻ��TʱObjectʱ������
        {
        //ʹ�������ṩ��LoadAsync�������첽��Դ���ط�����
            ResourceRequest resourceRequest = Resources.LoadAsync<T>(path);
            yield return resourceRequest;   //��Դ�������֮��Ż����¼�������
            if(resourceRequest.asset is GameObject)
            {
                //Instantiate
                //�ص�����
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