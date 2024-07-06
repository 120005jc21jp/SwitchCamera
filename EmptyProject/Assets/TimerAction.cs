using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 超シンプルな簡易的な予約実行構造.
// これもstatic,シングルトン.
public class TimerAction : MonoBehaviour
{
    [System.Serializable]
    public class Timer
    {
        public float bomb = 0;
        public System.Action action;
    }


    public static void TimerSet(float wait, System.Action action)
    {
        if(stInstance==null)
        {
            stInstance = new GameObject().AddComponent<TimerAction>();
        }
        stInstance.AddTimer(Time.realtimeSinceStartup+wait,action);
    }


    private static TimerAction stInstance = null;


    [UnityEngine.SerializeField] private System.Collections.Generic.List<Timer> timers = null;



    // Update is called once per frame
    void Update()
    {
        if(timers!=null)
        {
            for(int i=0; i<timers.Count; ++i)
            {
                Timer timer = timers[i];
                if(timer==null)
                {
                    timers.RemoveAt(i);
                    --i;
                    continue;
                }

                if(timer.bomb<Time.realtimeSinceStartup)
                {
                    timer.action?.Invoke();

                    timers.RemoveAt(i);
                    --i;
                    continue;
                }
            }
        }
    }

    void AddTimer(float bomb, System.Action action)
    {
        if(timers==null)
        {
            timers = new List<Timer>();
        }
        timers.Add(new Timer(){bomb=bomb,action=action});
    }
}
