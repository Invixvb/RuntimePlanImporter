using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class ConfigField
{
    private TMP_InputField _inputField;
        
    private string ValuePrefsName { get; }
    private string InputFieldGoName { get; }
    private Action<float> ChangeAction { get; }

    /// <summary>
    /// Gets the values when a new one is created and sets it for this script to use.
    /// Also Creates a new PlayerPref and sets the value it got.
    /// </summary>
    /// <param name="valuePrefsName"></param>
    /// <param name="inputFieldGoName"></param>
    /// <param name="initialValue"></param>
    /// <param name="changeAction"></param>
    public ConfigField(string valuePrefsName, string inputFieldGoName, float initialValue, Action<float> changeAction)
    {
        ValuePrefsName = valuePrefsName;
        InputFieldGoName = inputFieldGoName;
        ChangeAction = changeAction;
        
        PlayerPrefs.SetFloat(valuePrefsName, initialValue);
    }

    /// <summary>
    /// Get executed on creation of a value.
    /// Gets the value and sets the text of the input field to that value.
    /// Also adds a listener to that specific input field so it executes that method when the values changes.
    /// </summary>
    public void Initialize()
    {
        var value = PlayerPrefs.GetFloat(ValuePrefsName);
        
        _inputField = GameObject.Find(InputFieldGoName).gameObject.transform.Find("InputField").GetComponent<TMP_InputField>();
        _inputField.text = value.ToString(CultureInfo.InvariantCulture);

        _inputField.onValueChanged.AddListener(delegate { OnChangeFieldValue(); });
    }
    
    /// <summary>
    /// Gets executed when the values of the input field is changed.
    /// Changes the text of the input field to the new value and sets the PlayerPrefs to it as well.
    /// </summary>
    private void OnChangeFieldValue()
    {
        var value = float.Parse(_inputField.text);
        PlayerPrefs.SetFloat(ValuePrefsName, value);
        ChangeAction?.Invoke(value);
    }
}