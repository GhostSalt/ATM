using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public abstract class Menu : MonoBehaviour
{
    protected int Bank;
    protected Image ImageTemplate;
    protected Text TextTemplate;
    private Sprite[] AllSprites;
    private Font[] AllFonts;

    public virtual void Construct(int bank, Image imageTemplate, Text textTemplate, Sprite[] allSprites, Font[] allFonts)
    {
        Bank = bank;
        ImageTemplate = imageTemplate;
        TextTemplate = textTemplate;
        AllSprites = allSprites;
        AllFonts = allFonts;
    }

    public bool RegisterInput(string input)
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
            return RegisterArrowPress(value);
        }
        else if (Regex.IsMatch(input, "^keypad num [0-9]$"))
        {
            int value = int.Parse(Regex.Matches(input, "[0-9]")[0].Value);
            return RegisterNumKeyPress(value);
        }
        else if (Regex.IsMatch(input, "^keypad side [0-2]$"))
        {
            int value = int.Parse(Regex.Matches(input, "[0-2]")[0].Value);
            return RegisterSideKeyPress(value);
        }

        return false;
    }

    protected virtual void RegisterModuleFocus() { }

    protected virtual void RegisterModuleDefocus() { }

    protected virtual void RegisterCardInsert() { }

    protected virtual bool RegisterArrowPress(int pos) { return false; }

    protected virtual bool RegisterNumKeyPress(int pos) { return false; }

    protected virtual bool RegisterSideKeyPress(int pos) { return false; }

    public abstract void Activate(bool isInitialMenu);

    public abstract void Destroy();

    public event Action<Type> OnChangeMenus;

    protected void ChangeMenu(Type type)
    {
        OnChangeMenus?.Invoke(type);
    }

    public event Action OnRequestRemoveCard;

    protected void RemoveCard()
    {
        OnRequestRemoveCard?.Invoke();
    }

    public event Action OnRequestDispenseCash;

    protected void DispenseCash()
    {
        OnRequestDispenseCash?.Invoke();
    }

    public event Action OnRequestStrike;

    protected void RequestStrike()
    {
        OnRequestStrike?.Invoke();
    }

    public event Action OnRequestSolve;

    protected void RequestSolve()
    {
        OnRequestSolve?.Invoke();
    }

    protected Image CreateImage()
    {
        return Instantiate(ImageTemplate, ImageTemplate.transform.parent);
    }

    protected Text CreateText()
    {
        var text = Instantiate(TextTemplate, TextTemplate.transform.parent);
        text.font = Utility.FindFont(Bank, AllFonts);
        return text;
    }
}
