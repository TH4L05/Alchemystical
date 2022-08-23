using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Alchemystical
{
    public class Inventory : MonoBehaviour
    {

        [SerializeField] private List<InventorySlot> inventorySlots = new List<InventorySlot>();
        [SerializeField] private List<InventoryUISlot> inventoryUISlots = new List<InventoryUISlot>();
        [SerializeField] private GameObject inventoryUI;
        [SerializeField] private TextMeshProUGUI currencyTextField;
        private int gold;
        private int currentPage = 1;
        private int pageAmount;
        private Ingredient[] ingredients;

        [SerializeField] private int startGold = 500;
        [SerializeField] private int ingredientStartValue = 10;

        public int Gold => gold;

        private void Start()
        {
            ingredients = Game.Instance.gameData.ingredients;
            pageAmount = Mathf.RoundToInt(inventorySlots.Count / ingredients.Length) + 1;

            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (i < ingredients.Length)
                {
                    inventorySlots[i].IngredientType = ingredients[i];
                    continue;
                }
                inventorySlots[i].IngredientType = null;
                
            }

            AddGold(startGold);
            for (int i = 0; i < ingredients.Length; i++)
            {
                AddItemAmount(ingredients[i], ingredientStartValue);
            }
        }

        public void AddGold(int goldAmount)
        {
            Debug.Log("AddGold");
            gold += goldAmount;
            UpdateUI();
        }

        public void RemoveGold(int goldAmount)
        {
            Debug.Log("RemoveGold");
            gold -= goldAmount;
            UpdateUI();
        }

        public void AddItemAmount(Ingredient ingredient, int amount)
        {
            
            foreach (var slot in inventorySlots)
            {
                if (slot.IngredientType.ingredientName == ingredient.ingredientName)
                {
                    slot.IncreaseAmount(amount);
                    Debug.Log($"Add {amount}x of {slot.IngredientType.ingredientName} to Inventory");
                    break;
                }
            }
            UpdateUISlots();
        }

        public void RemoveItemAmount(Ingredient ingredient, int amount)
        {         
            foreach (var slot in inventorySlots)
            {
                if (slot.IngredientType.ingredientName == ingredient.ingredientName)
                {
                    Debug.Log($"Removed {amount}x of {slot.IngredientType.ingredientName} from Inventory");
                    slot.DecreaseAmount(amount);
                    break;
                }
            }
            UpdateUISlots();
        }

        private void UpdateUI()
        {
            currencyTextField.text = gold.ToString();
        }

        public void ChangeInvewntoryUIObjectStatus(bool active)
        {

            if (active)
            {
                UpdateUISlots();
                UpdateUI();
            }
            
            inventoryUI.SetActive(active);
        }

        public void NextPage()
        {
            currentPage++;

            if (currentPage > pageAmount)
            {
                currentPage = pageAmount;
                
            }
            UpdateUISlots();
        }

        public void PrevoiusPage()
        {
            currentPage--;

            if (currentPage < 1)
            {
                currentPage = 1;
                
            }
            UpdateUISlots();
        }

        private void UpdateUISlots()
        {
            for (int i = 0; i < inventoryUISlots.Count; i++)
            {
                int index = i;

                if (currentPage > 1)
                {
                    index =  i + 5 + (currentPage - 1);
                }

                if (index > ingredients.Length - 1)
                {
                    inventoryUISlots[i].SetInventoryIndex(index);
                    inventoryUISlots[i].UpdateSlotDefault();
                    continue;
                }
                inventoryUISlots[i].SetInventoryIndex(index);
                inventoryUISlots[i].UpdateSlot();
            }
        }

        public Ingredient GetInventorySlotIngredient(int index)
        {
            return inventorySlots[index].IngredientType;
        }

        public int GetInventorySlotIngredientAmount(int index)
        {
            return inventorySlots[index].currentAmount;
        }

        public void ChangeInventoryUISlotButtonInteractability(bool interactable)
        {
            foreach (var uiSlot in inventoryUISlots)
            {
                uiSlot.SetButtonInteractable(interactable);
            }
        }
    }
}
