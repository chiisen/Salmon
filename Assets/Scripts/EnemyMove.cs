using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [Header("碰撞時，是否減少生命值")]
    public bool LossLifes = true;

    [Header("碰撞時，是否播放音效")]
    public bool Sound = true;

    public float MoveSpeed = 1f;

    protected Vector3 _newPosition;

    [Header("要播放音效的物件")]
    public AudioSource _AudioSource;

    // 生成的敵人啟始與結束位置
    protected float enemyEnd = -6f;

    // 及時處理會無法播放音效
    [Header("延遲幾秒處理")]
    public float DelayTime = 1f;

    // 碰撞後是否隱藏
    [Header("碰撞後是否隱藏")]
    public bool Hide = false;

    // 重複碰撞
    [Header("重複碰撞")]
    public bool RepeatedCollision = false;

    protected Collider2D _col2D;

    protected float _OffsetTime = 0f;
    protected float _time = 0f;
    [Header("是否動中")]
    public bool play = true;

    void Start()
    {
        enemyEnd = LevelManager.Instance.EnemyEnd;

        _col2D = GetComponent<Collider2D>();

        _time = Time.time;
    }

    // 要有 Rigidbody2D 才會有作用
    protected virtual void OnTriggerEnter(Collider other)
    {
        TriggerEnter(other.gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        TriggerEnter(other.gameObject);
    }

    // 播放音效
    protected void PlaySound()
    {
        if (_AudioSource != null)
        {
            _AudioSource.Play();
        }
    }

    // 碰撞後隱藏
    protected void HideObject()
    {
        if (Hide == true)
        {
            // 先搬遠一點
            _newPosition.y -= 10000f;
            transform.position = _newPosition;
        }
    }
    
    protected void CheckRepeatedCollision()
    {
        if (RepeatedCollision == true)
        {
            return;
        }

        if (_col2D != null)
        {
            _col2D.enabled = false;
        }
    }

    protected virtual void TriggerEnter(GameObject collidingObject)
    {
        // we verify that the colliding object is a PlayableCharacter with the Player tag. If not, we do nothing.			
        if (collidingObject.tag != "Player") { return; }

        PlayableCharacter player = collidingObject.GetComponent<PlayableCharacter>();
        if (player == null) { return; }

        CheckRepeatedCollision();


        // 碰撞後隱藏
        HideObject();

        // 播放音效
        PlaySound();

        if (LossLifes == true)
        {
            // 減血量
            LevelManager.Instance.LossLifes();
        }
        else
        {
            // 加血量
            LevelManager.Instance.ObtainLifes();
        }
    }

    void Update ()
    {
        if (play == true)
        {
            if (_OffsetTime != Time.time)
            {
                _time += (Time.time - _OffsetTime);
                _OffsetTime = Time.time;
            }
        }

        _newPosition = transform.position;

        _newPosition.y -= (MoveSpeed * Time.deltaTime);

        transform.position = _newPosition;

        if (_newPosition.y < enemyEnd)
        {
            // 及時處理會無法播放音效
            StartCoroutine(StartCoroutineDelayDestroy(gameObject));
            //Destroy(gameObject);
        }
    }

    protected virtual IEnumerator StartCoroutineDelayDestroy(GameObject obj)
    {
        yield return new WaitForSecondsRealtime(DelayTime);

        Destroy(obj);
    }
}
