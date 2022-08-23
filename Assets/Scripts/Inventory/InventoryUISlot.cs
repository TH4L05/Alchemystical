using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace Alchemystical
{
    public class InventoryUISlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public static Action<Ingredient> AddIngredientFromSlot;

        [SerializeField] private Image inventoryIcon;
        [SerializeField] private TextMeshProUGUI nameWidget;
        [SerializeField] private TextMeshProUGUI stackSizeWidget;
        [SerializeField] private Sprite slotDefaultSprite;
        [SerializeField] private Button slotButton;
        [SerializeField] private InventoryInfo inventoryInfo;
        
        private int inventorySlotIndex;
        private Ingredient ingredient;

        public void SetInventoryIndex(int index)
        {
            inventorySlotIndex = index;
        }

        public void UpdateSlot()
        {
            ingredient = Game.Instance.inventory.GetInventorySlotIngredient(inventorySlotIndex);
            int amount = Game.Instance.inventory.GetInventorySlotIngredientAmount(inventorySlotIndex);

            nameWidget.text =  ingredient.ingredientName;
            inventoryIcon.sprite = ingredient.icon;
            stackSizeWidget.text = amount.ToString();
        }

        public void UpdateSlotDefault()
        {
            nameWidget.text = "";
            inventoryIcon.sprite = slotDefaultSprite;
            stackSizeWidget.text = "";
            ingredient = null;
        }

        public void OnSlotClicked()
        {
            var ingredient = Game.Instance.inventory.GetInventorySlotIngredient(inventorySlotIndex);
            AddIngredientFromSlot?.Invoke(ingredient);
        }

        public void SetButtonInteractable(bool interactable)
        {
            slotButton.interactable = interactable;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            inventoryInfo.UpdateInfo(ingredient);
            inventoryInfo.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            inventoryInfo.gameObject.SetActive(false);
        }
    }
}

