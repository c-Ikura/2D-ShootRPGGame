using QFramework;

namespace ShootGame
{
    public class AutoRelease : ViewController
    {
        public float delayTime;
        public string pathName;
        private void OnEnable()
        {
            this.GetSystem<ITimeSystem>().AddTimer(delayTime, false, () =>
            {
                this.GetSystem<IGameObjectPool>().Release(pathName, this.gameObject);
            });
        }
    }
}

