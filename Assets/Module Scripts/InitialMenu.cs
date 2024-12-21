using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InitialMenu : Menu
{
    private Image Logo;
    private Text Text;
    private Image Advert;

    private List<Sprite> PossibleAds = new List<Sprite>();

    private Coroutine AdCycleCoroutine;
    private List<Sprite> ChosenAds;
    private bool IsPlayingAds = false;

    public override void Construct(int bank, Image imageTemplate, Text textTemplate, Sprite[] allSprites, Font[] allFonts)
    {
        base.Construct(bank, imageTemplate, textTemplate, allSprites, allFonts);

        Logo = CreateImage();
        Logo.sprite = Utility.FindSprite("logo " + Bank, allSprites);
        Logo.transform.localScale = Vector3.zero;

        Text = CreateText();
        Text.text = "\n\n\n\n\n\nPlease insert your card";
        Text.fontSize = 20;
        Text.transform.localScale = Vector3.zero;

        Advert = CreateImage();
        Advert.transform.localScale = Vector3.zero;

        int i = 0;
        Sprite tempSprite = Utility.FindSprite("ad 0", allSprites);
        while (tempSprite != null && i < 1000) // Wanna make sure this isn't an infinite loop.
        {
            i++;
            PossibleAds.Add(tempSprite);
            tempSprite = Utility.FindSprite("ad " + i, allSprites);
        }

        ChosenAds = PossibleAds.ToList().Shuffle().Take(3).ToList();
    }

    public override void Activate(bool isInitialMenu)
    {
        Logo.transform.localScale = Vector3.one;
        Text.transform.localScale = Vector3.one;
        
        if (AdCycleCoroutine != null)
            StopCoroutine(AdCycleCoroutine);
        AdCycleCoroutine = StartCoroutine(AdCycle(isInitialMenu));
        IsPlayingAds = true;
    }

    public override void Destroy()
    {
        if (AdCycleCoroutine != null)
            StopCoroutine(AdCycleCoroutine);
        IsPlayingAds = false;

        Destroy(Logo.gameObject);
        Destroy(Text.gameObject);
        Destroy(Advert.gameObject);
    }

    protected override void RegisterModuleFocus()
    {
        if (IsPlayingAds)
        {
            if (AdCycleCoroutine != null)
                StopCoroutine(AdCycleCoroutine);
            IsPlayingAds = false;
            Advert.transform.localScale = Vector3.zero;
        }
    }

    protected override void RegisterModuleDefocus()
    {
        if (!IsPlayingAds)
        {
            if (AdCycleCoroutine != null)
                StopCoroutine(AdCycleCoroutine);
            AdCycleCoroutine = StartCoroutine(AdCycle());
            IsPlayingAds = true;
        }
    }

    protected override void RegisterCardInsert()
    {
        ChangeMenu(typeof(PINMenu));
    }

    private IEnumerator AdCycle(bool happenImmediately = false, float interval = 10f)
    {
        float timer = 0;
        if (!happenImmediately)
        {
            while (timer < interval)
            {
                yield return null;
                timer += Time.deltaTime;
            }
        }
        Advert.transform.localScale = Vector3.one;
        while (true)
            foreach (var ad in ChosenAds)
            {
                Advert.sprite = ad;
                timer = 0;
                while (timer < interval)
                {
                    yield return null;
                    timer += Time.deltaTime;
                }
            }
    }

}
