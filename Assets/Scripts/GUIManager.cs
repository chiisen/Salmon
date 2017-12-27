using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class GUIManager : Singleton<GUIManager>
{
    public Text Timer;
    public Text Lifes;
    public Text Distance;

    [Header("生命愛心顯示物件")]
    public GameObject HeartsContainer;

    [Header("單一個生命愛心顯示物件")]
    public GameObject ImageHearts;

    protected List<GameObject> ListImageHearts = new List<GameObject>();

    [Header("是否顯示生命上限")]
    public bool ShowMaxLifes = true;

    protected int _MaxLifes = 3;
    protected int _Lifes = 3;
    protected int _MaxDistance = 999; 
    protected int _Distance = 999;


    // 遊戲結束時間
    protected float _EndTime = 60f;
    protected float _Time = 0f;
    protected Action _TimesUp = null;
    protected float _offsetTime = 0f;

    protected virtual void Start()
    {
        _MaxLifes = LevelManager.Instance.Lifes;
        _MaxDistance = LevelManager.Instance.Distance;
        _EndTime = LevelManager.Instance.EndTime;

        _TimesUp = () => { StartCoroutine(StartCoroutineTimesUp());
            // 紀錄時間
            int TotalTime_ = PlayerPrefs.GetInt("TotalTime");
            int offsetTime_ = (int)_offsetTime;
            TotalTime_ += offsetTime_;
            PlayerPrefs.SetInt("TotalTime", TotalTime_);
        };
    }

    protected virtual IEnumerator StartCoroutineTimesUp()
    {
        yield return new WaitForSecondsRealtime(0.3f);

        LevelManager.Instance.GoToNextLevel();

        yield return null;
    }

    public virtual void UpdateDistance(int distance)
    {
        if (Distance != null)
        {
            bool bUpdate_ = false;
            if (_Distance != distance)
            {
                bUpdate_ = true;
                _Distance = distance;
            }

            if (bUpdate_ != true)
            {
                if (distance >= 0)
                {
                    Distance.text = "剩下 " + distance.ToString("#0") + " 公尺";
                }
            }
        }
    }

    void InstantiateHearts(int lifes)
    {
        if (Lifes == null)
        {
            return;
        }

        if (ListImageHearts.Count > 0)
        {
            return;
        }

        // 初始化 只做一次
        for (int i = 0; i < lifes; ++i)
        {
            if (ImageHearts == null)
            {
                continue;
            }

            GameObject instance_ = (GameObject)Instantiate(ImageHearts);
            if (HeartsContainer == null)
            {
                continue;
            }

            instance_.transform.SetParent(HeartsContainer.transform, false);
            RectTransform tran_ = instance_.GetComponent<RectTransform>();
            Vector3 hearts_ = new Vector3(tran_.localPosition.x, tran_.localPosition.y, tran_.localPosition.z);
            hearts_.x = i * 128f;
            tran_.localPosition = hearts_;
            ListImageHearts.Add(instance_);
        }
    }

    public virtual void UpdateLifes(int lifes)
    {
        if (Lifes != null)
        {
            InstantiateHearts(lifes);

            bool bUpdate_ = false;
            if (_Lifes != lifes)
            {
                bUpdate_ = true;

                if(HeartsContainer != null)
                {

                    if (_Lifes > lifes)
                    {
                        // 扣血
                        int offset_ = _Lifes - lifes;
                        for (int i = 0; i < offset_; i++)
                        {
                            int index_ = ListImageHearts.Count - 1;
                            if (index_ >= 0)
                            {
                                GameObject obj_ = ListImageHearts[index_];
                                ListImageHearts.RemoveAt(index_);
                                Destroy(obj_);
                            }
                        }
                    }
                    else
                    {
                        // 加血
                        int offset_ = lifes - _Lifes;
                        for (int i = 0; i < offset_; i++)
                        {
                            if (ImageHearts == null)
                            {
                                continue;
                            }

                            GameObject instance_ = (GameObject)Instantiate(ImageHearts);
                            if (HeartsContainer == null)
                            {
                                continue;
                            }

                            instance_.transform.SetParent(HeartsContainer.transform, false);
                            RectTransform tran_ = instance_.GetComponent<RectTransform>();
                            Vector3 hearts_ = new Vector3(tran_.localPosition.x, tran_.localPosition.y, tran_.localPosition.z);
                            hearts_.x = ListImageHearts.Count * 128f;
                            tran_.localPosition = hearts_;
                            ListImageHearts.Add(instance_);
                        }
                    }
                }

                _Lifes = lifes;
            }

            if (ShowMaxLifes == true)
            {
                //顯示生命上限
                if (bUpdate_ != true)
                {
                    Lifes.text = "生命:" + lifes.ToString("#0") + " / " + _MaxLifes.ToString("#0");
                }
            }
            else
            {
                //不顯示生命上限
                if (bUpdate_ != true)
                {
                    Lifes.text = "生命:" + lifes.ToString("#0");
                }
            }
        }
    }

    protected virtual void Update()
    {
        if (_Time != 0)
        {
            _offsetTime = Time.time - _Time;
            if (Timer != null)
            {
                float offsetTime_ = _EndTime - _offsetTime;
                if (offsetTime_ >= 0f)
                {
                    int offset_ = (int)offsetTime_;
                    Timer.text = string.Format("剩下 {0} 秒", offset_);
                }
            }
            if (_offsetTime > _EndTime)
            {
                if (_TimesUp != null)
                {
                    _TimesUp();
                    _TimesUp = null;
                }
            }
        }
        else
        {
            _Time = Time.time;
        }
    }
}
