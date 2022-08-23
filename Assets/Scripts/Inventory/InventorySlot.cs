using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Alchemystical
{
    [System.Serializable]
    public class InventorySlot
    {
        public Ingredient IngredientType;
        public int currentAmount = 0;
        public int maxAmount = 999;

        public void IncreaseAmount(int amount)
        {
            currentAmount += amount;    
            
            if (currentAmount > maxAmount)
            {
                currentAmount = maxAmount;
            }

        }
        public void DecreaseAmount(int amount)
        {
            currentAmount -= amount;

            if (currentAmount < 0)
            {
                currentAmount = 0;
            }

        }
    }
}

