using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public abstract class Menu : MonoBehaviour
{
    protected Image ImageTemplate;
    protected Text TextTemplate;

    public Menu(Image imageTemplate, Text textTemplate)
    {
        ImageTemplate = imageTemplate;
        TextTemplate = textTemplate;
    }

    public void RegisterInput(string input)
    {
        if (Regex.IsMatch(input, "^arrow [0-7]$"))
        {
            int value = int.Parse(Regex.Matches(input, "[0-7]")[0].Value);
            RegisterArrowPress(value);
        }
        else if (Regex.IsMatch(input, "^keypad num [0-9]$"))
        {
            int value = int.Parse(Regex.Matches(input, "[0-9]")[0].Value);
            RegisterNumKeyPress(value);
        }
        else if (Regex.IsMatch(input, "^keypad side [0-2]$"))
        {
            int value = int.Parse(Regex.Matches(input, "[0-2]")[0].Value);
            RegisterSideKeyPress(value);
        }
        else if (Regex.IsMatch(input, "^card insert$"))
            RegisterCardInsert();
    }

    protected virtual void RegisterArrowPress(int pos) { }

    protected virtual void RegisterNumKeyPress(int pos) { }

    protected virtual void RegisterSideKeyPress(int pos) { }

    protected virtual void RegisterCardInsert() { }

    public abstract void Activate();

    public abstract void Destroy();

    public event Action<Type> OnChangeMenus;

    protected void ChangeMenu(Type type)
    {
        OnChangeMenus?.Invoke(type);
    }
}
