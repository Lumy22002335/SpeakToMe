using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceWriteToScreen : MonoBehaviour
{
    [SerializeField] private Text _textDisplay;
    [SerializeField] private VoskSpeechToText _speechToText;

    bool isLissening = true;
    private void Awake()
    {
        _speechToText.OnTranscriptionResult += CheckTranscriptionResult;
    }

    private void CheckTranscriptionResult(string obj)
    {
        RecognitionResult result = new RecognitionResult(obj);
               
        if (result.Phrases[0].Text == "" || !isLissening) { return; }

        StartTyping(result.Phrases[0].Text);
    }

    public float typingSpeed = 0.01f;
    private string targetString;

    public void StartTyping(string text)
    {
        StopAllCoroutines();
        targetString = text;
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        _textDisplay.text = "";
        foreach (char c in targetString)
        {
            _textDisplay.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
