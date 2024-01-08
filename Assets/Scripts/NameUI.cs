using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _cursorText;
    [SerializeField] private GameObject _win;
    [SerializeField] private GameObject _lose;
    [SerializeField] private GameObject _transcription;
    [SerializeField] private GameObject _spiritBox;
    [SerializeField] private GameObject _audio;

    private string currentText = "";

    private float timer = 0.5f;
    private string spiritName = "";

    private List<KeyCode> validKeys = new List<KeyCode>()
    {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J,
        KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T,
        KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z
    };

    public void Init(string name)
    {
        spiritName = name;
        _transcription.SetActive(false);
        _spiritBox.SetActive(false);
        _audio.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 0.5f && !_cursorText.text.Contains('_'))
        {
            _cursorText.text += "_";
        }
        if (timer >= 1)
        {
            _cursorText.text = _cursorText.text.Replace("_", "");
            timer = 0f;
        }
    }

    private void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey && e.type == EventType.KeyUp)
        {
            if (e.keyCode == KeyCode.None) { return; }

            if (e.keyCode == KeyCode.Backspace)
            {
                if (currentText.Length > 0)
                {
                    currentText = currentText.Remove(_nameText.text.Length - 1);
                    _nameText.text = currentText;
                }
            }
            else if (e.keyCode == KeyCode.Return)
            {
                CheckName();
            }
            else if (CheckValidKeyCode(e.keyCode)) 
            {
                currentText += e.keyCode.ToString();
                _nameText.text = currentText;
            }
        }
    }

    private bool CheckValidKeyCode(KeyCode code)
    {
        foreach (KeyCode key in validKeys)
        {
            if (key == code)
            {
                return true;
            }
        }

        return false;
    }

    public void CheckName()
    {
        if (spiritName.ToLower() == currentText.ToLower())
        {
            _win.SetActive(true);
        }
        else
        {
            _lose.SetActive(true);
        }

        gameObject.SetActive(false);
    }
}
