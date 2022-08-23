using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Alchemystical
{
    public class IngredientPage : MonoBehaviour
    {
        [SerializeField]
        private Image ingredientImage;
        [SerializeField]
        private TextMeshProUGUI ingredientName;
        [SerializeField]
        private TextMeshProUGUI[] ingredientEffects;

        public void Show(Potion potion)
        {
            ingredientName.text = potion.potionName;

            ingredientImage.gameObject.SetActive(true);
            ingredientImage.sprite = potion.potionPicture;

            for (int i = 0; i < ingredientEffects.Length; i++)
            {
                ingredientEffects[i].text = potion.effects[i].ToString();
            }

        }

        public void ShowBlank()
        {
            ingredientImage.gameObject.SetActive(false);
            ingredientName.text = "";
            

            for (int i = 0; i < ingredientEffects.Length; i++)
            {
                ingredientEffects[i].text = "";
            }
        }
    }
}

