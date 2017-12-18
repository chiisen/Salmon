using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DelayGotoLevel : MonoBehaviour
{
    public Text Timer;
    public Text Title;

    protected float _Time = 0f;

    [Header("延遲幾秒到下一關")]
    public float DelayTime = 5f;
    [Header("指定的下一關")]
    public string NextLevel = "Level2";
    [Header("標題字串")]
    public string TitleString = "Title";

    void Start ()
    {
        if (Title != null)
        {
            Title.text = TitleString;
        }
        StartCoroutine(StartCoroutineDelayGotoLevel(NextLevel));
    }

    protected virtual void Update()
    {
        if (_Time != 0)
        {
            float offsetTime_ = Time.time - _Time;
            if (Timer != null)
            {
                float timer_ = DelayTime - offsetTime_;
                if (timer_ > 0f)
                {
                    Timer.text = timer_.ToString("#0");
                }
            }
        }
        else
        {
            _Time = Time.time;
        }
    }

    protected virtual IEnumerator StartCoroutineDelayGotoLevel(string levelName)
    {
        yield return new WaitForSecondsRealtime(DelayTime);

        LevelManager.Instance.GotoLevel(NextLevel);
    }
}
