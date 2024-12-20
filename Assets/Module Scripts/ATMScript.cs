﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;
using UnityEngine.UI;

public class ATMScript : MonoBehaviour
{
    static int _moduleIdCounter = 1;
    int _moduleID = 0;

    public KMBombModule Module;
    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMSelectable[] DisplayButtons;
    public KMSelectable[] KeypadKeys;
    public KMSelectable CardReader;
    public Transform DisplayAudioTransform;
    public Transform CardTransform;
    public MeshRenderer[] CardFaceRends;
    public TextMesh PaperText;
    public Sprite[] AllSprites;
    public Font[] AllFonts;

    public Image BGImage;
    public Image ImageTemplate;
    public Text TextTemplate;

    private Coroutine[] DisplayButtonAnimCoroutines;
    private Coroutine[] KeypadKeyAnimCoroutines;
    private Vector3 CardInitScale;
    private float DisplayButtonInitPos;
    private float KeypadKeyInitPos;

    private Menu CurrentMenu;
    private GameObject CurrentMenuObject;

    private List<int> CardNumber = new List<int>();
    private List<int> AccountNumber = new List<int>();
    private List<int> Expiry = new List<int>();
    private List<int> CVV2 = new List<int>();

    private int Bank;
    private bool IsCardInserted;

    private string FormatCardNumber(List<int> num)
    {
        var output = new List<string>();
        for (int i = 0; i < 4; i++)
        {
            output.Add("");
            for (int j = 0; j < 4; j++)
                output[i] += num[(i * 4) + j];
        }
        return output.Join(" ");
    }

    private string FormatExpiry(List<int> expiry)
    {
        return expiry[0].ToString("00") + "/" + expiry[1].ToString("00");
    }

    void Awake()
    {
        _moduleID = _moduleIdCounter++;

        var hue = Rnd.Range(0, 1f);
        foreach (var rend in CardFaceRends)
            rend.material.color = Color.HSVToRGB(hue, 1, 0.5f);

        DisplayButtonAnimCoroutines = new Coroutine[DisplayButtons.Length];
        DisplayButtonInitPos = DisplayButtons[0].transform.localPosition.y;
        for (int i = 0; i < DisplayButtons.Length; i++)
        {
            int x = i;
            DisplayButtons[x].OnInteract += delegate { DisplayButtonPress(x); return false; };
        }

        KeypadKeyAnimCoroutines = new Coroutine[KeypadKeys.Length];
        KeypadKeyInitPos = KeypadKeys[0].transform.localPosition.y;
        for (int i = 0; i < KeypadKeys.Length; i++)
        {
            int x = i;
            KeypadKeys[x].OnInteract += delegate { KeypadKeysPress(x); return false; };
        }

        CardReader.OnInteract += delegate { InsertCard(); return false; };
        CardInitScale = CardTransform.localScale;
        CardTransform.localScale = Vector3.zero;

        Bank = Rnd.Range(0, AllFonts.Length);
        BGImage.sprite = Utility.FindSprite("background " + Bank, AllSprites);

        GenerateCardInfo();
        InitialiseMenu();

        PaperText.transform.parent.localEulerAngles = new Vector3(90, Rnd.Range(-5f, 5f), 0);

        Module.OnActivate += delegate { CurrentMenu.Activate(true); BGImage.transform.localScale = Vector3.one; };

        CurrentMenu.OnChangeMenus += HandleMenuChange;
        CurrentMenu.OnRequestRemoveCard += HandleRemoveCard;
        CurrentMenu.OnRequestDispenseCash += HandleDispenseCash;

        Module.GetComponent<KMSelectable>().OnFocus += delegate { CurrentMenu.RegisterInput("module focus"); };
        Module.GetComponent<KMSelectable>().OnDefocus += delegate { CurrentMenu.RegisterInput("module defocus"); };
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void HandleMenuChange(Type type)
    {
        CurrentMenu.Destroy();
        Destroy(CurrentMenuObject);

        CurrentMenuObject = new GameObject("Current Menu");
        CurrentMenuObject.transform.parent = Module.transform;

        CurrentMenu.OnChangeMenus -= HandleMenuChange;
        CurrentMenu.OnRequestRemoveCard -= HandleRemoveCard;
        CurrentMenu.OnRequestDispenseCash -= HandleDispenseCash;

        CurrentMenu = (Menu)CurrentMenuObject.AddComponent(type);
        CurrentMenu.Construct(Bank, ImageTemplate, TextTemplate, AllSprites, AllFonts);

        CurrentMenu.OnChangeMenus += HandleMenuChange;
        CurrentMenu.OnRequestRemoveCard += HandleRemoveCard;
        CurrentMenu.OnRequestDispenseCash += HandleDispenseCash;

        CurrentMenu.Activate(false);
    }

    private void HandleRemoveCard()
    {
        StartCoroutine(RemoveCardAnim());
    }

    private void HandleDispenseCash()
    {
        Audio.PlaySoundAtTransform("dispense cash", transform);
    }

    private void InitialiseMenu()
    {
        BGImage.transform.localScale = ImageTemplate.transform.localScale = TextTemplate.transform.localScale = Vector3.zero;

        CurrentMenuObject = new GameObject("Current Menu");
        CurrentMenuObject.transform.parent = Module.transform;
        CurrentMenu = CurrentMenuObject.AddComponent<InitialMenu>();
        CurrentMenu.Construct(Bank, ImageTemplate, TextTemplate, AllSprites, AllFonts);
    }

    private void GenerateCardInfo()
    {
        CardNumber = new List<int>();
        AccountNumber = new List<int>();
        Expiry = new List<int>();
        CVV2 = new List<int>();

        for (int i = 0; i < 16; i++)
            CardNumber.Add(Rnd.Range(0, 10));

        for (int i = 0; i < 8; i++)
            AccountNumber.Add(Rnd.Range(0, 10));

        Expiry.Add(Rnd.Range(0, 12));
        Expiry.Add(Rnd.Range(0, 100));

        for (int i = 0; i < 3; i++)
            CVV2.Add(Rnd.Range(0, 10));

        DisplayCardInfo();
    }

    private void DisplayCardInfo()
    {
        PaperText.text = "Card number:\n\n\n" + FormatCardNumber(CardNumber) + "\n\n\n\nAccount number:\n\n\n" + AccountNumber.Join("") + "\n\n\n\nExpiry: " + FormatExpiry(Expiry) + "\n\n\n\nCVV2: " + CVV2.Join("");
    }

    private void DisplayButtonPress(int pos)
    {
        Audio.PlaySoundAtTransform("button press", DisplayButtons[pos].transform);

        if (DisplayButtonAnimCoroutines[pos] != null)
            StopCoroutine(DisplayButtonAnimCoroutines[pos]);
        DisplayButtonAnimCoroutines[pos] = StartCoroutine(ButtonPressAnim(DisplayButtons[pos].transform, DisplayButtonInitPos, 0.1f));

        bool playBeep = CurrentMenu.RegisterInput("arrow " + pos);
        if (playBeep)
            Audio.PlaySoundAtTransform("beep", DisplayAudioTransform);
    }

    private void KeypadKeysPress(int pos)
    {
        Audio.PlaySoundAtTransform("button press", KeypadKeys[pos].transform);

        if (KeypadKeyAnimCoroutines[pos] != null)
            StopCoroutine(KeypadKeyAnimCoroutines[pos]);
        KeypadKeyAnimCoroutines[pos] = StartCoroutine(ButtonPressAnim(KeypadKeys[pos].transform, KeypadKeyInitPos, 0.002f));

        bool playBeep = false;
        if (pos < 10)
            playBeep = CurrentMenu.RegisterInput("keypad num " + (pos + 1) % 10);
        else
            playBeep = CurrentMenu.RegisterInput("keypad side " + (pos - 10));

        if (playBeep)
            Audio.PlaySoundAtTransform("beep", DisplayAudioTransform);
    }

    private void InsertCard()
    {
        if (!IsCardInserted)
        {
            IsCardInserted = true;
            StartCoroutine(InsertCardAnim());
        }
    }

    private IEnumerator ButtonPressAnim(Transform target, float start, float depression, float duration = 0.075f)
    {
        target.localPosition = new Vector3(target.localPosition.x, start, target.localPosition.z);

        float timer = 0;
        while (timer < duration)
        {
            target.localPosition = new Vector3(target.localPosition.x, Mathf.Lerp(start, start - depression, timer / duration), target.localPosition.z);
            yield return null;
            timer += Time.deltaTime;
        }
        target.localPosition = new Vector3(target.localPosition.x, start - depression, target.localPosition.z);

        timer = 0;
        while (timer < duration)
        {
            target.localPosition = new Vector3(target.localPosition.x, Mathf.Lerp(start - depression, start, timer / duration), target.localPosition.z);
            yield return null;
            timer += Time.deltaTime;
        }
        target.localPosition = new Vector3(target.localPosition.x, start, target.localPosition.z);
    }

    private IEnumerator InsertCardAnim(float introDuration = 0.5f, float insertDuration = 0.35f, float takeDuration = 0.5f)
    {
        CardTransform.localScale = CardInitScale;

        Vector3 introEnd = new Vector3(0.0425f, 0.06f, -0.0095f);
        Vector3 introStart = introEnd + new Vector3(0, 2f, -0.5f);

        Vector3 insertEnd = introEnd - Vector3.up * 0.03f;
        Vector3 animEnd = insertEnd - Vector3.up * 0.05f;

        CardTransform.localPosition = introStart;
        CardTransform.localEulerAngles = Vector3.up * 90;

        float timer = 0;
        while (timer < introDuration)
        {
            yield return null;
            timer += Time.deltaTime;
            CardTransform.localPosition = new Vector3(introStart.x, Easing.OutCubic(timer, introStart.y, introEnd.y, introDuration), Easing.OutExpo(timer, introStart.z, introEnd.z, introDuration));
            CardTransform.localEulerAngles = new Vector3(0, 90, Easing.OutQuad(timer, 0, 90, introDuration));
        }
        CardTransform.localPosition = introEnd;
        CardTransform.localEulerAngles = new Vector3(0, 90, 90);

        timer = 0;
        while (timer < insertDuration)
        {
            yield return null;
            timer += Time.deltaTime;
            CardTransform.localPosition = new Vector3(introEnd.x, Easing.InSine(timer, introEnd.y, insertEnd.y, insertDuration), introEnd.z);
        }
        CardTransform.localPosition = insertEnd;

        Audio.PlaySoundAtTransform("insert card", CardReader.transform);
        yield return new WaitForSeconds(0.2f);

        timer = 0;
        while (timer < takeDuration)
        {
            yield return null;
            timer += Time.deltaTime;
            CardTransform.localPosition = new Vector3(insertEnd.x, Mathf.Lerp(insertEnd.y, animEnd.y, timer / takeDuration), insertEnd.z);
        }
        CardTransform.localPosition = animEnd;

        yield return new WaitForSeconds(0.3f);
        CardTransform.localScale = Vector3.zero;
        CurrentMenu.RegisterInput("card insert");
    }

    private IEnumerator RemoveCardAnim(float spitDuration = 0.5f, float snatchDuration = 0.5f)
    {
        CardTransform.localScale = CardInitScale;

        Vector3 snatchMiddle = new Vector3(0.0425f, 0.06f, -0.0095f);

        Vector3 snatchEnd = snatchMiddle + new Vector3(0, 2f, -0.5f);
        Vector3 snatchStart = snatchMiddle - Vector3.up * 0.03f;

        Vector3 spitStart = snatchStart - Vector3.up * 0.05f;

        CardTransform.localPosition = spitStart;
        CardTransform.localEulerAngles = new Vector3(0, 90, 90);

        Audio.PlaySoundAtTransform("remove card", CardReader.transform);
        yield return new WaitForSeconds(0.4f);

        float timer = 0;
        while (timer < spitDuration)
        {
            yield return null;
            timer += Time.deltaTime;
            CardTransform.localPosition = new Vector3(spitStart.x, Mathf.Lerp(spitStart.y, snatchStart.y, timer / spitDuration), spitStart.z);
        }
        CardTransform.localPosition = snatchStart;

        yield return new WaitForSeconds(0.75f);

        timer = 0;
        while (timer < snatchDuration)
        {
            yield return null;
            timer += Time.deltaTime;
            CardTransform.localPosition = new Vector3(snatchStart.x, Easing.InCubic(timer, snatchStart.y, snatchEnd.y, snatchDuration),
                Easing.InExpo(Mathf.Max(0, timer - 0.1f), snatchStart.z, snatchEnd.z, snatchDuration - 0.1f));
            CardTransform.localEulerAngles = new Vector3(0, 90, Easing.InQuad(timer, 90, 0, snatchDuration));
        }
        CardTransform.localPosition = snatchEnd;
        CardTransform.localEulerAngles = Vector3.up * 90;

        yield return new WaitForSeconds(0.1f);
        IsCardInserted = false;
        CardTransform.localScale = Vector3.zero;
        CurrentMenu.RegisterInput("card remove");
    }
}
