using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Alchemystical
{
    public class InventoryInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI infoClockwise;
        [SerializeField] private TextMeshProUGUI infoCounterClockwise;

        public void UpdateInfo(Ingredient ingredient)
        {
            if (ingredient == null)
            {
                infoClockwise.text = "---";
                infoCounterClockwise.text = "---";
            }
            else
            {
                (bool, bool) effectsUnlocked = ingredient.GetEffectStatus(); 

                if (effectsUnlocked.Item1)
                {
                    infoClockwise.text = ingredient.clockwiseEffect.ToString();
                }
                else
                {
                    infoClockwise.text = "???";
                }

                if (effectsUnlocked.Item2)
                {
                    infoCounterClockwise.text = ingredient.counterClockwiseEffect.ToString();
                }
                else
                {
                    infoCounterClockwise.text = "???";
                }               
            }
        }
    }
}

