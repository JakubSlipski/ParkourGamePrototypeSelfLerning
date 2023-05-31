using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsControl : MonoBehaviour
{
    [Header("UI CHANGE")]
    public Dropdown dropDown;

    [Header("References")]
    public KeyBinds kb;

    public void SetFlashLight()
    {
        string codeKey = dropDown.options[dropDown.value].text;
        Debug.Log(codeKey);
        if (codeKey == "Q")
        {
            kb.flashLightKey = KeyCode.Q;
        }
        else if (codeKey == "E")
        {
            kb.flashLightKey = KeyCode.E;
        }
        else if (codeKey == "R")
        {
            kb.flashLightKey = KeyCode.R;
        }
        else if (codeKey == "T")
        {
            kb.flashLightKey = KeyCode.T;
        }
        else if (codeKey == "Y")
        {
            kb.flashLightKey = KeyCode.Y;
        }
        else if (codeKey == "U")
        {
            kb.flashLightKey = KeyCode.U;
        }
        else if (codeKey == "I")
        {
            kb.flashLightKey = KeyCode.I;
        }
        else if (codeKey == "O")
        {
            kb.flashLightKey = KeyCode.O;
        }
        else if (codeKey == "F")
        {
            kb.flashLightKey = KeyCode.F;
        }
        else if (codeKey == "G")
        {
            kb.flashLightKey = KeyCode.G;
        }
        else if (codeKey == "H")
        {
            kb.flashLightKey = KeyCode.H;
        }
        else if (codeKey == "J")
        {
            kb.flashLightKey = KeyCode.J;
        }
        else if (codeKey == "K")
        {
            kb.flashLightKey = KeyCode.K;
        }
        else if (codeKey == "L")
        {
            kb.flashLightKey = KeyCode.L;
        }
        else if (codeKey == "Z")
        {
            kb.flashLightKey = KeyCode.Z;
        }
        else if (codeKey == "X")
        {
            kb.flashLightKey = KeyCode.X;
        }
        else if (codeKey == "C")
        {
            kb.flashLightKey = KeyCode.C;
        }
        else if (codeKey == "V")
        {
            kb.flashLightKey = KeyCode.V;
        }
        else if (codeKey == "B")
        {
            kb.flashLightKey = KeyCode.B;
        }
        else if (codeKey == "N")
        {
            kb.flashLightKey = KeyCode.N;
        }
        else if (codeKey == "M")
        {
            kb.flashLightKey = KeyCode.M;
        }
        else if (codeKey == "Caps Lock")
        {
            kb.flashLightKey = KeyCode.CapsLock;
        }
        else if (codeKey == "LeftCtrl")
        {
            kb.flashLightKey = KeyCode.LeftControl;
        }
        else if (codeKey == "RightCtrl")
        {
            kb.flashLightKey = KeyCode.RightControl;
        }
        else if (codeKey == "LeftShift")
        {
            kb.flashLightKey = KeyCode.LeftShift;
        }
        else if (codeKey == "RightShift")
        {
            kb.flashLightKey = KeyCode.RightShift;
        }
        else if (codeKey == "LeftAlt")
        {
            kb.flashLightKey = KeyCode.LeftAlt;
        }
        else if (codeKey == "RightAlt")
        {
            kb.flashLightKey = KeyCode.RightAlt;
        }
        else if (codeKey == "Space")
        {
            kb.flashLightKey = KeyCode.Space;
        }
    }
}

/* LIST OF BINDS
      Q,
      E,
      R,
      T,
      Y,
      U,
      I,
      O,
      F,
      G,
      H,
      J,
      K,------
      L,
      Z,
      X,
      C,
      V,
      B,
      N,
      M,
      CapsLock,
      LeftCtrl,
      RightCtrl,
      LeftShift,
      RightShift,
      LeftAlt,
      RightAlt,
      Space
*/

/* KEY TAMPLATE
 * if (codeKey == "Q")
        {
            Debug.Log(codeKey);
            Key = KeyCode.Q;
        }
        else if (codeKey == "E")
        {
            Key = KeyCode.E;
        }
        else if (codeKey == "R")
        {
            Key = KeyCode.R;
        }
        else if (codeKey == "T")
        {
            Key = KeyCode.T;
        }
        else if (codeKey == "Y")
        {
            Key = KeyCode.Y;
        }
        else if (codeKey == "U")
        {
            Key = KeyCode.U;
        }
        else if (codeKey == "I")
        {
            Key = KeyCode.I;
        }
        else if (codeKey == "O")
        {
            Key = KeyCode.O;
        }
        else if (codeKey == "F")
        {
            Key = KeyCode.F;
        }
        else if (codeKey == "G")
        {
            Key = KeyCode.G;
        }
        else if (codeKey == "H")
        {
            Key = KeyCode.H;
        }
        else if (codeKey == "J")
        {
            Key = KeyCode.J;
        }
        else if (codeKey == "K")
        {
            Key = KeyCode.K;
        }
        else if (codeKey == "L")
        {
            Key = KeyCode.L;
        }
        else if (codeKey == "Z")
        {
            Key = KeyCode.Z;
        }
        else if (codeKey == "X")
        {
            Key = KeyCode.X;
        }
        else if (codeKey == "C")
        {
            Key = KeyCode.C;
        }
        else if (codeKey == "V")
        {
            Key = KeyCode.V;
        }
        else if (codeKey == "B")
        {
            Key = KeyCode.B;
        }
        else if (codeKey == "N")
        {
            Key = KeyCode.N;
        }
        else if (codeKey == "M")
        {
            Key = KeyCode.M;
        }
        else if (codeKey == "Caps Lock")
        {
            Key = KeyCode.CapsLock;
        }
        else if (codeKey == "LeftCtrl")
        {
            Key = KeyCode.LeftControl;
        }
        else if (codeKey == "RightCtrl")
        {
            Key = KeyCode.RightControl;
        }
        else if (codeKey == "LeftShift")
        {
            Key = KeyCode.LeftShift;
        }
        else if (codeKey == "RightShift")
        {
            Key = KeyCode.RightShift;
        }
        else if (codeKey == "LeftAlt")
        {
            Key = KeyCode.LeftAlt;
        }
        else if (codeKey == "RightAlt")
        {
            Key = KeyCode.RightAlt;
        }
        else if (codeKey == "Space")
        {
            Key = KeyCode.Space;
        }
*/