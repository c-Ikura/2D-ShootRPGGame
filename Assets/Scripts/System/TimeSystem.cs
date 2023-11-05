using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Instrumentation;
using UnityEditor.Build;
using UnityEngine;

namespace ShootGame
{
    public class Timer
    {
        private float delayTime;
        private float finishTime;
        public float curTime { get;private set; }

        public bool isLoop;
        private bool isFinish;
        public bool IsFinish => isFinish;

        public Action onFinish;

        public void Start(float delayTime, bool isLoop, Action onFinish)
        {
            this.delayTime = delayTime;
            this.isLoop = isLoop;
            this.onFinish = onFinish;
            isFinish = false;
            finishTime = Time.time + delayTime;
        }
       
        public void Stop()=>isFinish = true;
        public void TimeCheck()
        {
            curTime = finishTime - Time.time;
            if (isFinish) return;
            if (Time.time < finishTime) return;
            if (!isLoop) Stop();
            else finishTime = Time.time + delayTime;
            onFinish?.Invoke();
        }

    }

    public interface ITimeSystem:ISystem
    {
        Timer AddTimer(float delayTime, bool isLoop, Action onFinish);
    }
    public class TimeSystem : AbstractSystem, ITimeSystem
    {

        private List<Timer> updateList = new List<Timer>();
        private Queue<Timer> timerQueue = new Queue<Timer>();
        protected override void OnInit()
        {
            GameObject timeSystem = new GameObject("TimeSystem");
            var time = timeSystem.AddComponent<TimerBehaviour>();
            time.onUpdate += Update;
        }
        public Timer AddTimer(float delayTime, bool isLoop, Action onFinish)
        {
            var timer = timerQueue.Count == 0 ? new Timer() : timerQueue.Dequeue();
            timer.Start(delayTime, isLoop, onFinish);
            
            updateList.Add(timer);

            return timer;
        }
        private void Update()
        {
            if(updateList.Count == 0) return;

            for (int i = 0; i < updateList.Count; i++)
            {
                if (updateList[i].IsFinish)
                {
                    timerQueue.Enqueue(updateList[i]);
                    updateList.RemoveAt(i);                   
                    continue;
                }
                updateList[i].TimeCheck();
            }
        }

        public class TimerBehaviour : MonoBehaviour
        {
            public Action onUpdate;
            private void Update()
            {
                onUpdate?.Invoke(); 
            }
        }
    }
}


