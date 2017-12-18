using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundLoop : MonoBehaviour
{
    // 2D圖需要將 Wrap Mode 調整成 Repeat 才有辦法讓圖 Loop
    [Header("流度速度")]
    public float speed = 0.2f;
    [Header("是否流動中")]
    public bool play = true;

    private Vector2 _mainTextureOffset;

    private Material _material;

    private float _OffsetTime = 0f;
    private float _time = 0f;

    public void Stop()
    {
        play = false;
    }

    public void Play()
    {
        play = true;
    }

    private void Start()
    {
        Renderer renderer_ = GetComponent<Renderer>();
        if (renderer_ != null)
        {
            _material = renderer_.material;
        }
        else
        {
            Debug.LogError("Null renderer_");
        }

        _time = Time.time;
    }

    private void Update()
    {
        if (play == true)
        {
            if (_OffsetTime != Time.time)
            {
                _time += (Time.time - _OffsetTime);
                _OffsetTime = Time.time;
            }

            _mainTextureOffset = new Vector2(0f, _time * speed);
            _material.mainTextureOffset = _mainTextureOffset;
        }
        else
        {
            _OffsetTime = Time.time;
        }
    }
}
