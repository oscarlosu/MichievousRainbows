using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    // Actions
    [SerializeField]
    InputGroup left;
    [SerializeField]
    InputGroup right;
    [SerializeField]
    InputGroup jump;
    [SerializeField]
    InputGroup shoot;


    /// <summary>
    /// Positive input = Pressed or Held
    /// NegativeInput = Released
    /// 
    /// Pressed = 1st frame down
    /// Held = > 1st frame down
    /// Released = 1st frame up
    /// 
    /// </summary>
    /// <param name="input">Mainly useful when corresponding InputGroup uses Axis</param>
    public delegate void HandlePositiveInput(float input);
    public delegate void HandleNegativeInput();
    // Event Listeners
    public static HandlePositiveInput LeftPressed;
    public static HandlePositiveInput LeftHeld;
    public static HandleNegativeInput LeftReleased;

    public static HandlePositiveInput RightPressed;
    public static HandlePositiveInput RightHeld;
    public static HandleNegativeInput RightReleased;

    public static HandlePositiveInput JumpPressed;
    public static HandlePositiveInput JumpHeld;
    public static HandleNegativeInput JumpReleased;

    public static HandlePositiveInput ShootPressed;
    public static HandlePositiveInput ShootHeld;
    public static HandleNegativeInput ShootReleased;

    void Update () {
        // Left
        left.Update();
        if (left.State == InputGroup.InputGroupState.Pressed) {
            if (LeftPressed != null) {
                LeftPressed(left.Value);
            }
        } else if (left.State == InputGroup.InputGroupState.Held) {
            if (LeftHeld != null) {
                LeftHeld(left.Value);
            }
        } else if (left.State == InputGroup.InputGroupState.Released) {
            if (LeftHeld != null) {
                LeftReleased();
            }
        }
        // Right
        right.Update();
        if (right.State == InputGroup.InputGroupState.Pressed) {
            if (RightPressed != null) {
                RightPressed(right.Value);
            }
        } else if (right.State == InputGroup.InputGroupState.Held) {
            if (RightHeld != null) {
                RightHeld(right.Value);
            }
        } else if (right.State == InputGroup.InputGroupState.Released) {
            if (RightHeld != null) {
                RightReleased();
            }
        }
        // Jump
        jump.Update();
        if (jump.State == InputGroup.InputGroupState.Pressed) {
            if (JumpPressed != null) {
                JumpPressed(jump.Value);
            }
        } else if (jump.State == InputGroup.InputGroupState.Held) {
            if (JumpHeld != null) {
                JumpHeld(jump.Value);
            }
        } else if (jump.State == InputGroup.InputGroupState.Released) {
            if (JumpHeld != null) {
                JumpReleased();
            }
        }
        // Shoot
        shoot.Update();
        if (shoot.State == InputGroup.InputGroupState.Pressed) {
            if (ShootPressed != null) {
                ShootPressed(shoot.Value);
            }
        } else if (shoot.State == InputGroup.InputGroupState.Held) {
            if (ShootHeld != null) {
                ShootHeld(shoot.Value);
            }
        } else if (shoot.State == InputGroup.InputGroupState.Released) {
            if (ShootHeld != null) {
                ShootReleased();
            }
        }
    }
}
