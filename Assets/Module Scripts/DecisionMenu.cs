using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class DecisionMenu : Menu
{
    private Text Question;
    private List<Text> Answers = new List<Text>();
    private List<Type> Links = new List<Type>();
    private List<int> IncorrectAnswers = new List<int>();

    public override void Construct(int bank, Image imageTemplate, Text textTemplate, Sprite[] allSprites, Font[] allFonts)
    {
        base.Construct(bank, imageTemplate, textTemplate, allSprites, allFonts);

        Question = CreateText();
        Question.fontSize = 20;
        Question.transform.localScale = Vector3.zero;

        Question.rectTransform.sizeDelta -= Vector2.right * 20;
        Question.transform.localPosition = Vector3.up * 65;

        for (int i = 0; i < 8; i++)
        {
            Answers.Add(CreateText());
            Answers[i].fontSize = 20;
            Answers[i].transform.localScale = Vector3.zero;

            Answers[i].rectTransform.sizeDelta -= Vector2.right * 20;
            Answers[i].transform.localPosition = Vector3.up * (18 - (32 * (i % 4)));
            Answers[i].alignment = (i > 3 ? TextAnchor.MiddleRight : TextAnchor.MiddleLeft);
        }
    }

    public override void Activate(bool isInitialMenu)
    {
        Question.transform.localScale = Vector3.one;
        for (int i = 0; i < 8; i++)
            Answers[i].transform.localScale = Vector3.one;
    }

    public override void Destroy()
    {
        Destroy(Question.gameObject);
        for (int i = 0; i < 8; i++)
            Destroy(Answers[i].gameObject);
    }

    protected void Assign(string question, string[] answers, Type[] links, List<int> incorrectAnswers = null)
    {
        Question.text = question;

        for (int i = 0; i < Mathf.Min(answers.Length, Answers.Count()); i++)
            Answers[i].text = answers[i];
        for (int i = Mathf.Min(answers.Length, 8); i < Answers.Count(); i++)
            Answers[i].text = "";

        for (int i = 0; i < Mathf.Min(links.Length, Answers.Count()); i++)
            Links.Add(links[i]);
        for (int i = Mathf.Min(links.Length, 8); i < Answers.Count(); i++)
            Links.Add(null);

        if (incorrectAnswers != null)
            IncorrectAnswers = incorrectAnswers.ToList();
    }

    protected override bool RegisterArrowPress(int pos)
    {
        if (Links[pos] != null)
        {
            if (!IncorrectAnswers.Contains(pos))
                ChangeMenu(Links[pos]);
            else
                RequestStrike();
            return true;
        }
        return false;
    }
}
