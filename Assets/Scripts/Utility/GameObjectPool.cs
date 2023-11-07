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
        /// ��ȡ
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
                Debug.Log("��ǰ��������� " + pools.Count);               
            }

            return gameObj;
        }

        private void AddPool(string objectPath)
        {
            GameObject poolObj = new GameObject(objectPath);
            viewObjPools.Add(objectPath, poolObj);

            pools.Add(objectPath, new ObjectPool<GameObject>(
                () =>//��������ʱ����
                {

                    var obj = GameObject.Instantiate(Resources.Load<GameObject>(objectPath));
                    obj.transform.SetParent(viewObjPools[objectPath].transform);
                    return obj;
                },
                (obj) =>//��ȡ����ʱ����
                {
                    obj.SetActive(true);
                },
                (obj) =>//��������ʱ����
                {
                    obj.SetActive(false);
                },
                (obj) =>//���ٶ���ʱ����
                {
                    GameObject.Destroy(obj);
                }
                , true, 10, 1000));
                
            poolObj.transform.SetParent(viewObjPools["GameObjectPool"].transform);
        }
           
    
        /// <summary>
        /// ���뻺�����
        /// </summary>
        public void Release(string objectPath,GameObject gameObject)
        {
            if(pools.TryGetValue(objectPath, out var pool))
            {
                Debug.Log("����");
            
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

