using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputGroup {
    public string Name;
    public List<KeyCode> Keys;
    public List<string> Buttons;
    public List<string> Axis;

    public float Value = 0;
    public InputGroupState State = InputGroupState.Default;

    public float Tolerance = 0;

    public enum InputGroupState {
        Pressed,
        Held,
        Released,
        Default
    }
	
	public void Update () {        
        // Keys
        bool pressed = false;
        bool held = false;
        bool released = false;
		for(int i = 0; i < Keys.Count; ++i) {
            pressed |= Input.GetKeyDown(Keys[i]);
            held |= Input.GetKey(Keys[i]);
            released |= Input.GetKeyUp(Keys[i]);
        }
        
        // Buttons
        for (int i = 0; i < Buttons.Count; ++i) {
            pressed |= Input.GetButtonDown(Buttons[i]);
            held |= Input.GetButton(Buttons[i]);
            released |= Input.GetButtonUp(Buttons[i]);
        }

        // Axis
        float axis = 0;
        for (int i = 0; i < Axis.Count; ++i) {
            axis = Mathf.Max(axis, Input.GetAxis(Axis[i]));
            pressed |= (State == InputGroupState.Default && Mathf.Abs(axis) > Tolerance);
            held |= ((State == InputGroupState.Pressed || State == InputGroupState.Held) && Mathf.Abs(axis) > Tolerance);
            released |= ((State == InputGroupState.Pressed || State == InputGroupState.Held) && Mathf.Abs(axis) <= Tolerance);
        }

        // Summarize
        if (pressed) {
            State = InputGroupState.Pressed;
            Value = axis > Tolerance ? axis : 1;
        } else if (held) {
            State = InputGroupState.Held;
            Value = axis > Tolerance ? axis : 1;
        } else if (released) {
            State = InputGroupState.Released;
            Value = 0;
        } else {
            State = InputGroupState.Default;
            Value = 0;
        }    
    }
}
