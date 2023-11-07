using QFramework;
using UnityEngine;

namespace ShootGame
{
    public class ViewController : MonoBehaviour, IController
    {
        public IArchitecture GetArchitecture()
        {
            return Global._interface;
        }
    }
}

