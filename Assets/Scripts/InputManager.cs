using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This persistent singleton handles the inputs and sends commands to the player
/// </summary>
public class InputManager : Singleton<InputManager>
{
    [Header("手機觸控螢幕")]
    public ETCTouchPad TouchPad = null;

    // ETCJoystick 在 Unity 上的 Inspector 上的 Visible 設定不能關閉，不然會偵測不到訊號。
    [Header("搖桿")]
    public ETCJoystick Joystick = null;

    protected virtual void Start()
    {

#if UNITY_ANDROID

        if (Joystick != null)
        {
            Joystick.gameObject.SetActive(false);
        }

        if (TouchPad != null)
        {
            TouchPad.OnDownLeft.AddListener(() => {
                LeftButtonDown();
            });
            TouchPad.OnPressLeft.AddListener(() => {
                LeftButtonPressed();
            });


            TouchPad.onTouchUp.AddListener(() => {
                RightButtonUp();
                LeftButtonUp();
            });


            TouchPad.OnDownRight.AddListener(() => {
                RightButtonDown();
            });
            TouchPad.OnPressRight.AddListener(() => {
                RightButtonPressed();
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
                LeftButtonDown();
            });
            Joystick.OnPressLeft.AddListener(() => {
                LeftButtonPressed();
            });


            Joystick.onMoveEnd.AddListener(() => {
                RightButtonUp();
                LeftButtonUp();
            });


            Joystick.OnDownRight.AddListener(() => {
                RightButtonDown();
            });
            Joystick.OnPressRight.AddListener(() => {
                RightButtonPressed();
            });
        }

#endif // UNITY_STANDALONE_WIN
    }

    /// <summary>
    /// Every frame, we get the various inputs and process them
    /// </summary>
    protected virtual void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

        HandleKeyboard();
    }

    /// <summary>
    /// Called at each Update(), it checks for various key presses
    /// </summary>
    protected virtual void HandleKeyboard()
    {
        // 請注意！！！ Input.GetButton() 裡面所帶的字串名稱，必須到 Edit->Project Settings->Input 設定才能正常執行。

        if (Input.GetButtonDown("Left")) { LeftButtonDown(); }
        if (Input.GetButtonUp("Left")) { LeftButtonUp(); }
        if (Input.GetButton("Left")) { LeftButtonPressed(); }

        if (Input.GetButtonDown("Right")) { RightButtonDown(); }
        if (Input.GetButtonUp("Right")) { RightButtonUp(); }
        if (Input.GetButton("Right")) { RightButtonPressed(); }

        if (Input.GetButtonDown("Up")) { UpButtonDown(); }
        if (Input.GetButtonUp("Up")) { UpButtonUp(); }
        if (Input.GetButton("Up")) { UpButtonPressed(); }

        if (Input.GetButtonDown("Down")) { DownButtonDown(); }
        if (Input.GetButtonUp("Down")) { DownButtonUp(); }
        if (Input.GetButton("Down")) { DownButtonPressed(); }

    }

    /// LEFT BUTTON ----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Triggered once when the left button is pressed down
    /// </summary>
    public virtual void LeftButtonDown()
    {
        LevelManager.Instance.CurrentPlayableCharacters.LeftStart();
    }

    /// <summary>
    /// Triggered once when the left button is released
    /// </summary>
    public virtual void LeftButtonUp()
    {
        LevelManager.Instance.CurrentPlayableCharacters.LeftEnd();
    }

    /// <summary>
    /// Triggered while the left button is being pressed
    /// </summary>
    public virtual void LeftButtonPressed()
    {
        LevelManager.Instance.CurrentPlayableCharacters.LeftOngoing();
    }


    /// RIGHT BUTTON ----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Triggered once when the right button is pressed down
    /// </summary>
    public virtual void RightButtonDown()
    {
        LevelManager.Instance.CurrentPlayableCharacters.RightStart();
    }

    /// <summary>
    /// Triggered once when the right button is released
    /// </summary>
    public virtual void RightButtonUp()
    {
        LevelManager.Instance.CurrentPlayableCharacters.RightEnd();
    }

    /// <summary>
    /// Triggered while the right button is being pressed
    /// </summary>
    public virtual void RightButtonPressed()
    {
        LevelManager.Instance.CurrentPlayableCharacters.RightOngoing();
    }



    /// DOWN BUTTON ----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Triggered once when the down button is pressed down
    /// </summary>
    public virtual void DownButtonDown()
    {
        LevelManager.Instance.CurrentPlayableCharacters.DownStart();
    }

    /// <summary>
    /// Triggered once when the down button is released
    /// </summary>
    public virtual void DownButtonUp()
    {
        LevelManager.Instance.CurrentPlayableCharacters.DownEnd();
    }

    /// <summary>
    /// Triggered while the down button is being pressed
    /// </summary>
    public virtual void DownButtonPressed()
    {
        LevelManager.Instance.CurrentPlayableCharacters.DownOngoing();
    }



    /// UP BUTTON ----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Triggered once when the up button is pressed down
    /// </summary>
    public virtual void UpButtonDown()
    {
        LevelManager.Instance.CurrentPlayableCharacters.UpStart();
    }

    /// <summary>
    /// Triggered once when the up button is released
    /// </summary>
    public virtual void UpButtonUp()
    {
        LevelManager.Instance.CurrentPlayableCharacters.UpEnd();
    }

    /// <summary>
    /// Triggered while the up button is being pressed
    /// </summary>
    public virtual void UpButtonPressed()
    {
        LevelManager.Instance.CurrentPlayableCharacters.UpOngoing();
    }
}

