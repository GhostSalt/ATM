﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Rnd = UnityEngine.Random;

public class TransToMainMenuMenu : TransMenu
{
    public override void Activate(bool isInitialMenu)
    {
        base.Activate(isInitialMenu);
        StartCoroutine(RunAnimation());
    }

    private IEnumerator RunAnimation()
    {
        var duration1 = Rnd.Range(1.35f, 1.65f);
        var duration2 = Rnd.Range(0.45f, 0.75f);

        float timer = 0;
        while (timer < duration1)
        {
            yield return null;
            timer += Time.deltaTime;
        }

        SetText("CONNECTING TO\nSERVER…");

        timer = 0;
        while (timer < duration2)
        {
            yield return null;
            timer += Time.deltaTime;
        }

        ChangeMenu(typeof(MainMenu));
    }
}
