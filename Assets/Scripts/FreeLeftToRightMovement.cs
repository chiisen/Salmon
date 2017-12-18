using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Add this class to a character and it'll allow it to move from left to right (or down/up, or back/forward) on the axis of your choice.
/// 提供能左右操作玩家物件的基礎類別給開發者繼承使用
/// </summary>
public class FreeLeftToRightMovement : PlayableCharacter
{
    /// the movement inertia (the higher it is, the longer it takes for it to stop / change direction
    protected int _movement;
    protected Vector3 _newPosition;

    /// the character's movement speed
    public float MoveSpeed = 1f;

    /// X 的左右邊界
    protected float MinBound = -3f;
    protected float MaxBound = 3f;

    /// <summary>
    /// On start we set bounds and movement axis based on what's been set in the inspector
    /// </summary>
    protected virtual void Start()
    {
        MinBound = LevelManager.Instance.MinBound;
        MaxBound = LevelManager.Instance.MaxBound;
    }

    /// <summary>
    /// On update we just handle our character's movement
    /// </summary>
    protected virtual void Update()
    {
        MoveCharacter();
    }

    /// <summary>
    /// Every frame, we move our character
    /// </summary>
    protected virtual void MoveCharacter()
    {
        switch (_movement)
        {
            case -1:// 按下左
                {
                    _newPosition = transform.position;
                    float move_ = MoveSpeed * Time.deltaTime;
                    if (_newPosition.x - move_ < MinBound)
                    {
                        _newPosition.x = MinBound;
                    }
                    else
                    {
                        _newPosition.x -= move_;
                    }
                }
                break;
            case 0: // 放開按鈕
                _newPosition = transform.position;
                break;
            case 1:// 按下右
                {
                    _newPosition = transform.position;
                    float move_ = MoveSpeed * Time.deltaTime;
                    if (_newPosition.x + move_ > MaxBound)
                    {
                        _newPosition.x = MaxBound;
                    }
                    else
                    {
                        _newPosition.x += move_;
                    }
                }
                break;
            default:
                Debug.Log("[ERROR] _movement=" + _movement.ToString());
                break;
        }

        // we actually move our transform to the new position
        transform.position = _newPosition;
    }

    /// <summary>
    /// When the player presses left, we apply a negative movement
    /// </summary>
    public override void LeftStart()
    {
        _movement = -1;
    }

    /// <summary>
    /// When the player stops pressing left, we reset our movement
    /// </summary>
    public override void LeftEnd()
    {
        _movement = 0;
    }

    /// <summary>
    /// When the player presses right, we apply a positive movement
    /// </summary>
    public override void RightStart()
    {
        _movement = 1;
    }

    /// <summary>
    /// When the player stops pressing right, we reset our movement
    /// </summary>
    public override void RightEnd()
    {
        _movement = 0;
    }
}
