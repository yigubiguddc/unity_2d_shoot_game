using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private static ObjectPool instance;     //首先声明该类的一个静态实例instance
    //我们希望不同的物体可以分开存储，在这种情况下，使用字典十分合理,Dictionary是基于hash map实现的，与c++中的map并不相同
    private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>(); //这里直接实例化这个字典（objectPool以备后续使用）
    //使用独立额存储string（对象）的数量众多的值
    //为了不让窗口杂乱，创建一个pool作为所有物体的父物体
    private GameObject pool;
    public static ObjectPool Instance//公有的静态属性修饰静态实例
    {
        get
        {
            if(instance == null)
            {
                instance = new ObjectPool();
            }
            return instance;    //返回实例
        }
    }
    public GameObject GetObject(GameObject prefab)
    {
        GameObject _object; //临时对象，准备指向prefab通过Instantiate得到的克隆对象
        if(!objectPool.ContainsKey(prefab.name)||objectPool[prefab.name].Count == 0)
        {
            _object = GameObject.Instantiate(prefab);       //_object接受传入的prefab，成为临时的clone
            PushObject(_object);                            //将这个值传入Queue<GameObject>
            if(pool == null)
            {
                pool = new GameObject("ObjectPool");        //所有对象池物体的父类，为了更加有秩序而创建
            }
            GameObject child = GameObject.Find(prefab.name +"Pool");    //子类，可以获取多个clone对象
            if(!child)
            {
                child = new GameObject(prefab.name +"Pool");
                child.transform.SetParent(pool.transform);
            }
            _object.transform.SetParent(child.transform);
        }
        //从对应的队列中取出一个游戏对象。此时，这个对象已经是我们想要的对象（无论是新创建的还是从池中取出的）。
        _object = objectPool[prefab.name].Dequeue();   
        _object.SetActive(true);
        return _object;
    }
    public void PushObject(GameObject prefab)
    {
        //首先获取到已经暂时使用完毕的物品名
        string _name = prefab.name.Replace("(Clone)",string.Empty);  //实例化的物品名后面都会加上(Clone)，所以我们首先使用string.Replace 把这个换成空字符串
        if(!objectPool.ContainsKey(_name))
        {
            objectPool.Add(_name, new Queue<GameObject>()); //Dictionary contains the function "Add".
        }
        objectPool[_name].Enqueue(prefab);
        prefab.SetActive(false);
    }
    //遍历对象池，全部取消激活
    public void DeactivateAllObjects()
    {
        // 遍历每个对象队列
        foreach (KeyValuePair<string, Queue<GameObject>> pair in objectPool)
        {
            // 遍历队列中的每个对象
            foreach (GameObject obj in pair.Value)
            {
                // 将对象设置为非激活状态
                obj.SetActive(false);
                // 但在这个情况下，由于对象已经在队列中，所以我们不需要这么做
            }
        }
    }
}
