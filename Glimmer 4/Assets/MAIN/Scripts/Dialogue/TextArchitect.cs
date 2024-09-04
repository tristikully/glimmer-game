using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextArchitect
{
    private TextMeshProUGUI tmpro_ui;

    public TMP_Text tmpro => tmpro_ui;
    public string currentText => tmpro.text;
    public string targetText { get; private set; } = "";
    public string preText { get; private set; } = "";

    public string fullTargetText => preText + targetText;

    public float speed { get { return baseSpeed * speedMultiplier; } set { speedMultiplier = value ;} }
    private const float baseSpeed = 0.6f;
    private float speedMultiplier = 1;

    public int charactersPerCycle { get { return speed <= 2f ? characterMultiplier : speed <= 2.5f ? characterMultiplier * 2 : characterMultiplier * 3; } }
    private int characterMultiplier = 1;

    public TextArchitect(TextMeshProUGUI tmpro_ui)
    {
        this.tmpro_ui = tmpro_ui as TextMeshProUGUI;
    }

    public Coroutine Build(string text)
    {
        preText = "";
        targetText = text;

        Stop();

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }

    public Coroutine Append(string text)
    {
        preText = tmpro.text;
        targetText = text;

        Stop();

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }

    private Coroutine buildProcess = null;
    public bool isBuilding => buildProcess != null;

    public void Stop()
    {
        if (!isBuilding)
            return;

        tmpro.StopCoroutine(buildProcess);
        buildProcess = null;
    }


    public void ForceComplete()
    {
        tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;

        Stop();
        buildProcess = null;
    }

    IEnumerator Building()
    {
        Typewriter();
        yield return Build_Typewriter();
        buildProcess = null;
    }


    private void Typewriter()
    {
        tmpro.maxVisibleCharacters = 0;
        tmpro.text = preText;

        if (preText != "")
        {
            tmpro.ForceMeshUpdate();
            tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
        }

        tmpro.text += targetText;
        tmpro.ForceMeshUpdate();
    }

    private IEnumerator Build_Typewriter()
    {
        while(tmpro.maxVisibleCharacters < tmpro.textInfo.characterCount)
        {
            tmpro.maxVisibleCharacters += charactersPerCycle;

            yield return new WaitForSeconds(0.015f / speed);
        }
    }
}
