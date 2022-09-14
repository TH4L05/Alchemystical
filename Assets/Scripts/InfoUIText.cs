

using UnityEngine;
using UnityEngine.Playables;
using TMPro;
using System;
using Alchemystical;

public class InfoUIText : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private TextMeshProUGUI textField;
    private string currentText;

    private void Start()
    {
        Brew.ShowInfoText += SetAndShow;
    }

    private void OnDestroy()
    {
        Brew.ShowInfoText -= SetAndShow;
    }

    public void SetText(string text)
    {
        currentText = text;
        textField.text = currentText;
    }

    public void PlayDirector()
    {
        if (playableDirector == null)
        {
            throw new ArgumentNullException("PlayableDirectorRef is missing");
        }

        playableDirector.Play();
    }

    public void SetAndShow(string text)
    {
        SetText(text);
        PlayDirector();
    }
}
