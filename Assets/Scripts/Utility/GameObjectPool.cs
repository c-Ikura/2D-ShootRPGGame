using QFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace ShootGame
{
    public interface IGameObjectPool:ISystem
    {
        void Release(string objectPath, GameObject gameObject);
        GameObject Get(string objectPath);
    }
    public class GameObjectPool : AbstractSystem,IGameObjectPool
    {
        private Dictionary<string,ObjectPool<GameObject>> pools = new Dictionary<string,ObjectPool<GameObject>>();
        private Dictionary<string,GameObject> viewObjPools = new Dictionary<string,GameObject>();


        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public GameObject Get(string objectPath)
        {
            GameObject gameObj = null;

            if (pools.TryGetValue(objectPath, out var pool))
            {
                gameObj = pool.Get();
            }
            else
            {
                AddPool(objectPath);

                gameObj = pools[objectPath].Get();
                Debug.Log("当前对象池数量 " + pools.Count);               
            }

            return gameObj;
        }

        private void AddPool(string objectPath)
        {
            GameObject poolObj = new GameObject(objectPath);
            viewObjPools.Add(objectPath, poolObj);

            pools.Add(objectPath, new ObjectPool<GameObject>(
                () =>//创建对象时调用
                {

                    var obj = GameObject.Instantiate(Resources.Load<GameObject>(objectPath));
                    obj.transform.SetParent(viewObjPools[objectPath].transform);
                    return obj;
                },
                (obj) =>//获取对象时调用
                {
                    obj.SetActive(true);
                },
                (obj) =>//存入对象池时调用
                {
                    obj.SetActive(false);
                },
                (obj) =>//销毁对象时调用
                {
                    GameObject.Destroy(obj);
                }
                , true, 10, 1000));
                
            poolObj.transform.SetParent(viewObjPools["GameObjectPool"].transform);
        }
           
    
        /// <summary>
        /// 进入缓存池子
        /// </summary>
        public void Release(string objectPath,GameObject gameObject)
        {
            if(pools.TryGetValue(objectPath, out var pool))
            {
                Debug.Log("回收");
            
                pool.Release(gameObject);
            }
            else
            {
                AddPool(objectPath);
            }

        }

        protected override void OnInit()
        {
            GameObject gameObjectPool = new GameObject("GameObjectPool");
            viewObjPools.Add("GameObjectPool", gameObjectPool);
        }
    }
}

