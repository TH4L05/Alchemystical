
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Alchemystical
{
    public class OrderUIInfoSlot : MonoBehaviour
    {
        [SerializeField] private Image slotImage;
        [SerializeField] private TextMeshProUGUI slotTextField;
        [SerializeField] private Button slotButton;
        private Customer customer;

        public void UpdateSlot(Customer customer)
        {
            this.customer = customer;
            slotImage.sprite = customer.potion.potionPicture;
            slotTextField.text = customer.potion.potionName;
        }

        public void ClearSlot()
        {
            customer = null;
            slotImage.sprite = null;
            slotTextField.text = "";
        }
    }
}

