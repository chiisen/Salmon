using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// You should extend this class for all your playable characters
/// The asset includes a bunch of examples of how you can do that : Jumper, Flappy, Dragon, etc...
/// </summary>
public class PlayableCharacter : MonoBehaviour
{
    /// <summary>
    /// What happens when the down button is pressed
    /// </summary>
    public virtual void DownStart() { }
    /// <summary>
    /// What happens when the down button is released
    /// </summary>
    public virtual void DownEnd() { }
    /// <summary>
    /// What happens when the down button is being pressed
    /// </summary>
    public virtual void DownOngoing() { }

    /// <summary>
    /// What happens when the up button is pressed
    /// </summary>
    public virtual void UpStart() { }
    /// <summary>
    /// What happens when the up button is released
    /// </summary>
    public virtual void UpEnd() { }
    /// <summary>
    /// What happens when the up button is being pressed
    /// </summary>
    public virtual void UpOngoing() { }

    /// <summary>
    /// What happens when the left button is pressed
    /// </summary>
    public virtual void LeftStart() { }
    /// <summary>
    /// What happens when the left button is released
    /// </summary>
    public virtual void LeftEnd() { }
    /// <summary>
    /// What happens when the left button is being pressed
    /// </summary>
    public virtual void LeftOngoing() { }

    /// <summary>
    /// What happens when the right button is pressed
    /// </summary>
    public virtual void RightStart() { }
    /// <summary>
    /// What happens when the right button is released
    /// </summary>
    public virtual void RightEnd() { }
    /// <summary>
    /// What happens when the right button is being pressed
    /// </summary>
    public virtual void RightOngoing() { }
}

