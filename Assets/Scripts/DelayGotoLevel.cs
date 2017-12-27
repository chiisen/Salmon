using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DelayGotoLevel : MonoBehaviour
{
    public Text Timer;
    public Text Title;

    [Header("手機觸控螢幕")]
    public ETCTouchPad TouchPad = null;

    // ETCJoystick 在 Unity 上的 Inspector 上的 Visible 設定不能關閉，不然會偵測不到訊號。
    [Header("搖桿")]
    public ETCJoystick Joystick = null;

    protected float _Time = 0f;

    [Header("延遲幾秒到下一關")]
    public float DelayTime = 5f;
    [Header("指定的下一關")]
    public string NextLevel = "Level2";

    [Header("初始化 TotalTime")]
    public bool TotalTime = false;

    protected int _TotalTime = 0;

    void Start ()
    {
        if (TotalTime == true)
        {
            // 初始化玩家遊戲總時間
            PlayerPrefs.SetInt("TotalTime", 0);
        }
        else
        {
            _TotalTime = PlayerPrefs.GetInt("TotalTime");
            if (Title != null)
            {
                Title.text = string.Format("遊戲總時間為 {0} 秒", _TotalTime);
            }
        }
        StartCoroutine(StartCoroutineDelayGotoLevel(NextLevel));

#if UNITY_ANDROID

        if (Joystick != null)
        {
            Joystick.gameObject.SetActive(false);
        }

        if (TouchPad != null)
        {
            TouchPad.OnDownLeft.AddListener(() => {
                ButtonUp();
            });

            TouchPad.OnDownRight.AddListener(() => {
                ButtonUp();
            });
        }

#endif // UNITY_ANDROID

#if UNITY_STANDALONE_WIN

        if (TouchPad != null)
        {
            TouchPad.gameObject.SetActive(false);
        }

        // ETCJoystick 在 Unity 上的 Inspector 上的 Visible 設定不能關閉，不然會偵測不到訊號。
        if (Joystick != null)
        {
            Joystick.OnDownLeft.AddListener(() => {
                ButtonUp();
            });

            Joystick.OnDownRight.AddListener(() => {
                    ButtonUp();
                });
        }

#endif // UNITY_STANDALONE_WIN
    }

    public virtual void ButtonUp()
    {
        LevelManager.Instance.GotoLevel(NextLevel);
    }

    protected virtual void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetButtonUp("Left")) { ButtonUp(); }
        if (Input.GetButtonUp("Right")) { ButtonUp(); }

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
