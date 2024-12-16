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
    public Transform DisplayAudioTransform;

    private Coroutine[] DisplayButtonAnimCoroutines;
    private Coroutine[] KeypadKeyAnimCoroutines;
    private float DisplayButtonInitPos;
    private float KeypadKeyInitPos;

    void Awake()
    {
        _moduleID = _moduleIdCounter++;

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
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
}
