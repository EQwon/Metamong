using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginInputField : MonoBehaviour
{
    public Text passwordText;

    public void MakeToAsteroid()
    {
        int length = passwordText.text.Length;

        string asteroidPassword = "";
        for (int i = 0; i <= length; i++)
        {
            asteroidPassword += "*";
        }
        
        GetComponent<InputField>().text = asteroidPassword;
    }
}
