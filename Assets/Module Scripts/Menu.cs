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
    private Sprite[] AllSprites;

    public virtual void Construct(Image imageTemplate, Text textTemplate, Sprite[] allSprites)
    {
        ImageTemplate = imageTemplate;
        TextTemplate = textTemplate;
        AllSprites = allSprites;
    }

    public void RegisterInput(string input)
    {
        if (Regex.IsMatch(input, "^card insert$"))
            RegisterCardInsert();
        else if (Regex.IsMatch(input, "^module focus"))
            RegisterModuleFocus();
        else if (Regex.IsMatch(input, "^module defocus$"))
            RegisterModuleDefocus();
        else if (Regex.IsMatch(input, "^arrow [0-7]$"))
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
    }

    protected virtual void RegisterModuleFocus() { }

    protected virtual void RegisterModuleDefocus() { }

    protected virtual void RegisterCardInsert() { }

    protected virtual void RegisterArrowPress(int pos) { }

    protected virtual void RegisterNumKeyPress(int pos) { }

    protected virtual void RegisterSideKeyPress(int pos) { }

    public abstract void Activate(bool isInitialMenu);

    public abstract void Destroy();

    public event Action<Type> OnChangeMenus;

    protected void ChangeMenu(Type type)
    {
        OnChangeMenus?.Invoke(type);
    }

    protected Sprite FindSprite(string name)
    {
        foreach (var sprite in AllSprites)
            if (sprite.name == name)
                return sprite;
        return null;
    }
}
