using QFramework;
using ShootGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSystemTest : ViewController
{
    // Start is called before the first frame update
    void Start()
    {
        var timer = this.GetSystem<ITimeSystem>();
        timer.AddTimer(1, true, () =>
        {
            print("²âÄáÂê");
        });
       

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
