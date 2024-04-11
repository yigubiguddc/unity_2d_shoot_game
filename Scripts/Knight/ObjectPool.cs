using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private static ObjectPool instance;     //�������������һ����̬ʵ��instance
    //����ϣ����ͬ��������Էֿ��洢������������£�ʹ���ֵ�ʮ�ֺ���,Dictionary�ǻ���hash mapʵ�ֵģ���c++�е�map������ͬ
    private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>(); //����ֱ��ʵ��������ֵ䣨objectPool�Ա�����ʹ�ã�
    //ʹ�ö�����洢string�����󣩵������ڶ��ֵ
    //Ϊ�˲��ô������ң�����һ��pool��Ϊ��������ĸ�����
    private GameObject pool;
    public static ObjectPool Instance//���еľ�̬�������ξ�̬ʵ��
    {
        get
        {
            if(instance == null)
            {
                instance = new ObjectPool();
            }
            return instance;    //����ʵ��
        }
    }
    public GameObject GetObject(GameObject prefab)
    {
        GameObject _object; //��ʱ����׼��ָ��prefabͨ��Instantiate�õ��Ŀ�¡����
        if(!objectPool.ContainsKey(prefab.name)||objectPool[prefab.name].Count == 0)
        {
            _object = GameObject.Instantiate(prefab);       //_object���ܴ����prefab����Ϊ��ʱ��clone
            PushObject(_object);                            //�����ֵ����Queue<GameObject>
            if(pool == null)
            {
                pool = new GameObject("ObjectPool");        //���ж��������ĸ��࣬Ϊ�˸��������������
            }
            GameObject child = GameObject.Find(prefab.name +"Pool");    //���࣬���Ի�ȡ���clone����
            if(!child)
            {
                child = new GameObject(prefab.name +"Pool");
                child.transform.SetParent(pool.transform);
            }
            _object.transform.SetParent(child.transform);
        }
        //�Ӷ�Ӧ�Ķ�����ȡ��һ����Ϸ���󡣴�ʱ����������Ѿ���������Ҫ�Ķ����������´����Ļ��Ǵӳ���ȡ���ģ���
        _object = objectPool[prefab.name].Dequeue();   
        _object.SetActive(true);
        return _object;
    }
    public void PushObject(GameObject prefab)
    {
        //���Ȼ�ȡ���Ѿ���ʱʹ����ϵ���Ʒ��
        string _name = prefab.name.Replace("(Clone)",string.Empty);  //ʵ��������Ʒ�����涼�����(Clone)��������������ʹ��string.Replace ��������ɿ��ַ���
        if(!objectPool.ContainsKey(_name))
        {
            objectPool.Add(_name, new Queue<GameObject>()); //Dictionary contains the function "Add".
        }
        objectPool[_name].Enqueue(prefab);
        prefab.SetActive(false);
    }
    //��������أ�ȫ��ȡ������
    public void DeactivateAllObjects()
    {
        // ����ÿ���������
        foreach (KeyValuePair<string, Queue<GameObject>> pair in objectPool)
        {
            // ���������е�ÿ������
            foreach (GameObject obj in pair.Value)
            {
                // ����������Ϊ�Ǽ���״̬
                obj.SetActive(false);
                // �����������£����ڶ����Ѿ��ڶ����У��������ǲ���Ҫ��ô��
            }
        }
    }
}
