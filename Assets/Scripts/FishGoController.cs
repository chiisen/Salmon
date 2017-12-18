using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGoController : FreeLeftToRightMovement
{
    [Header("玩家是否自動移動")]
    public bool AotuMove = true;

    [Header("碰撞換色")]
    public bool ChangeColor = true;

    protected List<bool> _KeyBuffer = new List<bool>();

    [Header("延遲時間")]
    public float DelayTime = 1f;
    protected float _Time = 0f;

    protected SpriteRenderer _renderer;

    // 要有 Rigidbody2D 才會有作用
    protected virtual void OnTriggerEnter(Collider other)
    {
        TriggerEnter(other.gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        TriggerEnter(other.gameObject);
    }

    protected virtual void TriggerEnter(GameObject collidingObject)
    {
        if (ChangeColor == false)
        {
            return;
        }

        if (_renderer != null)
        {
            _renderer.color = Color.red;

            StartCoroutine(StartCoroutineResetColor(gameObject));
        }
    }

    protected virtual IEnumerator StartCoroutineResetColor(GameObject obj)
    {
        yield return new WaitForSecondsRealtime(1f);

        if (_renderer != null)
        {
            _renderer.color = Color.white;
        }
    }

    protected override void Start()
    {
        base.Start();

        _renderer = GetComponent<SpriteRenderer>();

        // 取得關卡設定的旗標狀態
        AotuMove = LevelManager.Instance.AotuMove;

        if (AotuMove == false)
        {
            // 手動移動 先關閉移動
            LevelManager.Instance.WaterFlows(false);
        }
    }

    // 手動移動
    protected void ManualMove()
    {
        // 有沒有按下按鈕
        bool bButtonUp_ = false;

        if (Input.GetButtonDown("Left"))
        {
            _KeyBuffer.Add(false);

            bButtonUp_ = true;
        }

        if (Input.GetButtonDown("Right"))
        {
            _KeyBuffer.Add(true);

            bButtonUp_ = true;
        }

        // 有按下按鈕要處理的事件
        if (bButtonUp_ == true)
        {
            if (_KeyBuffer.Count >= 2)
            {
                if (_KeyBuffer[_KeyBuffer.Count - 1] != _KeyBuffer[_KeyBuffer.Count - 2])
                {
                    // 河岸開始流動
                    LevelManager.Instance.WaterFlows(true);

                    // 距離減一
                    LevelManager.Instance.LessDistance();

                    _Time = Time.time;
                }
            }
        }

        if (_Time != 0f && Time.time - _Time > DelayTime)
        {
            // 河岸停止流動
            LevelManager.Instance.WaterFlows(false);

            _Time = 0f;

            _KeyBuffer.Clear();
        }

        // 不要讓 KeyBuffer 無限增長
        if (_KeyBuffer.Count > 2)
        {
            _KeyBuffer.RemoveAt(0);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (AotuMove == false)
        {
            // 手動移動
            ManualMove();
        }
    }
}
