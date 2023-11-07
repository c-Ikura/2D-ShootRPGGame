using QFramework;

namespace ShootGame
{
    public class Global : Architecture<Global>
    {
        protected override void Init()
        {
            RegisterSystem<IGunSystem>(new GunSystem());
            RegisterSystem<ITimeSystem>(new TimeSystem());
            RegisterSystem<IGameObjectPool>(new GameObjectPool());
            RegisterModel<IGunConfigModel>(new GunConfigModel());

        }
    }
}

