using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class InfoUITexT : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private TextMeshProUGUI textField;
    private string currentText;

    public void SetText(string text)
    {
        currentText = text;
        textField.text = currentText;
    }

    public void PlayDirector()
    {
        playableDirector.Play();
    }
}
