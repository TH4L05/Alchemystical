using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Playables;

namespace Alchemystical
{
    public class BrewInfoUI : MonoBehaviour
    {
        [SerializeField] private GameObject infoContainer;
        [SerializeField] private TextMeshProUGUI[] effectTexts;
        [SerializeField] private Image ingredientImage;
        [SerializeField] private PlayableDirector playableDirector;


        public void ChangeStatus(bool active)
        {
            if (infoContainer != null) infoContainer.SetActive(active);
        }

        public void ShowEffectText(int index)
        {
            if (effectTexts.Length < 1) return;
            if (index == -1) return;
            if (index-1 > effectTexts.Length) return;

            effectTexts[index-1].gameObject.SetActive(true);
        }

        public void SetEffectText(int index, string text)
        {
            if (effectTexts.Length < 1) return;
            if (index == -1) return;
            if (index-1 > effectTexts.Length) return;

            effectTexts[index-1].text = text;
        }

        public void ResetAllEffectText()
        {
            if (effectTexts.Length < 1) return;

            foreach (var text in effectTexts)
            {
                text.text = "";
                text.gameObject.SetActive(false);
            }
        }

        public void PlayDirector(Ingredient ingredient)
        {
            if (ingredientImage != null) ingredientImage.sprite = ingredient.icon;
            if (playableDirector != null) playableDirector.Play();
        }
    }
}

