using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOnTouch : MonoBehaviour
{
    [Header("要播放音效的物件")]
    public AudioSource _AudioSource;

    // 重複碰撞
    [Header("重複碰撞")]
    public bool RepeatedCollision = false;

    protected Collider2D _col2D;

    void Start()
    {
        _col2D = GetComponent<Collider2D>();
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

    protected virtual void TriggerEnter(GameObject collidingObject)
    {
        // we verify that the colliding object is a PlayableCharacter with the Player tag. If not, we do nothing.			
        if (collidingObject.tag != "Player") { return; }

        PlayableCharacter player = collidingObject.GetComponent<PlayableCharacter>();
        if (player == null) { return; }

        CheckRepeatedCollision();

        // 播放音效
        PlaySound();

        LevelManager.Instance.LossLifes();
    }

    // 播放音效
    protected void PlaySound()
    {
        if (_AudioSource != null)
        {
            _AudioSource.Play();
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
}
