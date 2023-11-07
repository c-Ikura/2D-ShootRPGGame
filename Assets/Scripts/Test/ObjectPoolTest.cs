using QFramework;
using ShootGame;
using UnityEngine;

public class ObjectPoolTest :ViewController
{
    public IGameObjectPool pool;
    private void Start()
    {
        pool = this.GetSystem<IGameObjectPool>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            pool.Get("Bullet_Single");
        }
    }
}
