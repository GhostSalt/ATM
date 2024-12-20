using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Rnd = UnityEngine.Random;

public class TransToMainMenuMenu : Menu
{
    private Text Text;

    public override void Construct(Image imageTemplate, Text textTemplate, Sprite[] allSprites)
    {
        base.Construct(imageTemplate, textTemplate, allSprites);

        Text = Instantiate(textTemplate, textTemplate.transform.parent);
        Text.text = "PLEASE WAIT";
        Text.fontSize = 30;
        Text.transform.localScale = Vector3.zero;
    }

    public override void Activate(bool isInitialMenu)
    {
        Text.transform.localScale = Vector3.one;

        StartCoroutine(RunAnimation());
    }

    public override void Destroy()
    {
        Destroy(Text.gameObject);
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

        Text.text = "CONNECTING TO\nSERVER…";

        timer = 0;
        while (timer < duration2)
        {
            yield return null;
            timer += Time.deltaTime;
        }

        ChangeMenu(typeof(InitialMenu));
    }
}
