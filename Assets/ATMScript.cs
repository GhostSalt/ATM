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
    public Transform DisplayAudioTransform;

    private Coroutine[] DisplayButtonAnimCoroutines;
    private float DisplayButtonInitPos;

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
        DisplayButtonAnimCoroutines[pos] = StartCoroutine(DisplayButtonAnim(pos));
    }

    private IEnumerator DisplayButtonAnim(int pos, float duration = 0.075f, float depression = 0.1f)
    {
        var target = DisplayButtons[pos].transform;
        target.localPosition = new Vector3(target.localPosition.x, DisplayButtonInitPos, target.localPosition.z);

        float timer = 0;
        while (timer < duration)
        {
            target.localPosition = new Vector3(target.localPosition.x, DisplayButtonInitPos - Mathf.Lerp(0, depression, timer / duration), target.localPosition.z);
            yield return null;
            timer += Time.deltaTime;
        }
        target.localPosition = new Vector3(target.localPosition.x, DisplayButtonInitPos - depression, target.localPosition.z);

        timer = 0;
        while (timer < duration)
        {
            target.localPosition = new Vector3(target.localPosition.x, DisplayButtonInitPos - Mathf.Lerp(depression, 0, timer / duration), target.localPosition.z);
            yield return null;
            timer += Time.deltaTime;
        }
        target.localPosition = new Vector3(target.localPosition.x, DisplayButtonInitPos, target.localPosition.z);
    }
}
