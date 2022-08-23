using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Alchemystical
{
    public class Offer : MonoBehaviour
    {
        public static Action<Ingredient, int> BuyOffer;


        #region Fields

        [SerializeField] private Image shopIcon;
        [SerializeField] private TextMeshProUGUI nameWidget;
        [SerializeField] private TextMeshProUGUI priceWidget;
        [SerializeField] private TextMeshProUGUI priceSingleWidget;
        [SerializeField] private TextMeshProUGUI stackSizeWidget;
        [SerializeField] private Sprite defaultSlotSprite;

        private Ingredient ingredientType;
        private int offerSize;
        private bool canBuy;

        public Ingredient IngredientType => ingredientType;
        public int OfferSize => offerSize;

        #endregion


        public void ResetSlot()
        {
            offerSize = 0;
            priceSingleWidget.text = "";
            priceWidget.text = "";
            stackSizeWidget.text = "";
            nameWidget.text = "";
            shopIcon.sprite = defaultSlotSprite;
        }

        public void SetIngriedient(Ingredient type)
        {
            canBuy = true;
            ingredientType = type;
            UpdateUI();
        }

        public void UpdateUI()
        {
            offerSize = UnityEngine.Random.Range(1, 50);
            //priceWidget.text = ingredientType.shopPrice.ToString() + " Gold";
            priceSingleWidget.text = ingredientType.shopPrice.ToString() + " Gold";
            priceWidget.text = (ingredientType.shopPrice * offerSize) + " Gold";
            stackSizeWidget.text = offerSize.ToString();
            nameWidget.text = ingredientType.ingredientName;
            shopIcon.sprite = ingredientType.icon;
        }

        public void Buy()
        {
            if (!canBuy) return;
            canBuy = false;
            BuyOffer(ingredientType, offerSize);
        }

        public void SetOfferSize(int size)
        {
            offerSize = size;
        }
    }
}

