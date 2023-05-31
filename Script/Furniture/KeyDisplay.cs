using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyDisplay : MonoBehaviour
{
    // The Text component that will display the key being pressed
    public Text keyText;

    void Update()
    {
        // Check for any key being pressed
        if (Input.anyKey)
        {
            // Get the name of the key being pressed
            string keyName = GetKeyName();

            // Update the Text component with the key name
            keyText.text = keyName;
        }
        else
        {
            // If no key is being pressed, clear the Text component
            keyText.text = "";
        }
    }

    string GetKeyName()
    {
        // Check for all possible keys that can be pressed
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(keyCode))
            {
                return keyCode.ToString();
            }
        }

        // Return an empty string if no key is being pressed
        return "";
    }
}
