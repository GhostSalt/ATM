using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;

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

    private Coroutine[] DisplayButtonAnimCoroutines;
    private Coroutine[] KeypadKeyAnimCoroutines;
    private Vector3 CardInitScale;
    private float DisplayButtonInitPos;
    private float KeypadKeyInitPos;

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
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void DisplayButtonPress(int pos)
    {
        Audio.PlaySoundAtTransform("button press", DisplayButtons[pos].transform);
        Audio.PlaySoundAtTransform("beep", DisplayAudioTransform);

        if (DisplayButtonAnimCoroutines[pos] != null)
            StopCoroutine(DisplayButtonAnimCoroutines[pos]);
        DisplayButtonAnimCoroutines[pos] = StartCoroutine(ButtonPressAnim(DisplayButtons[pos].transform, DisplayButtonInitPos, 0.1f));
    }

    private void KeypadKeysPress(int pos)
    {
        Audio.PlaySoundAtTransform("button press", KeypadKeys[pos].transform);
        Audio.PlaySoundAtTransform("beep", DisplayAudioTransform);

        if (KeypadKeyAnimCoroutines[pos] != null)
            StopCoroutine(KeypadKeyAnimCoroutines[pos]);
        KeypadKeyAnimCoroutines[pos] = StartCoroutine(ButtonPressAnim(KeypadKeys[pos].transform, KeypadKeyInitPos, 0.002f));
    }

    private void InsertCard()
    {
        CardTransform.localScale = CardInitScale;

        StartCoroutine(InsertCardAnim());
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
        Vector3 introEnd = new Vector3(0.0425f, 0.06f, -0.0095f);
        Vector3 introStart = introEnd + new Vector3(0, 2f, -0.2f);

        Vector3 insertEnd = introEnd - Vector3.up * 0.03f;
        Vector3 animEnd = insertEnd - Vector3.up * 0.05f;

        CardTransform.localPosition = introStart;

        float timer = 0;
        while (timer < introDuration)
        {
            CardTransform.localPosition = new Vector3(introStart.x, Easing.OutCubic(timer, introStart.y, introEnd.y, introDuration), Easing.OutExpo(timer, introStart.z, introEnd.z, introDuration));
            yield return null;
            timer += Time.deltaTime;
        }
        CardTransform.localPosition = introEnd;

        timer = 0;
        while (timer < insertDuration)
        {
            CardTransform.localPosition = new Vector3(introEnd.x, Easing.InSine(timer, introEnd.y, insertEnd.y, insertDuration), introEnd.z);
            yield return null;
            timer += Time.deltaTime;
        }
        CardTransform.localPosition = insertEnd;

        Audio.PlaySoundAtTransform("insert card", CardReader.transform);
        yield return new WaitForSeconds(0.2f);

        timer = 0;
        while (timer < takeDuration)
        {
            CardTransform.localPosition = new Vector3(insertEnd.x, Easing.InSine(timer, insertEnd.y, animEnd.y, takeDuration), insertEnd.z);
            yield return null;
            timer += Time.deltaTime;
        }
        CardTransform.localPosition = animEnd;
    }
}
