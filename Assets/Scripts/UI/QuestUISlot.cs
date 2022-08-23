

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Alchemystical
{
    public class QuestUISlot : MonoBehaviour
    {
        public static Action<Customer> QuestbuttonClicked;

        [SerializeField] private Image slotImage;
        [SerializeField] private TextMeshProUGUI slotTextField;
        [SerializeField] private Button slotButton;
        private Customer customer;
        private bool questDone;

        public void UpdateSlot(Customer customer)
        {
            this.customer = customer;
            slotImage.sprite = customer.potion.potionPicture;
            slotTextField.text = customer.potion.potionName;
        }

        public bool QuestDone()
        {
            return questDone;
        }


        public Customer GetCustomer()
        {
            return customer;
        }

        public Potion GetPotion()
        {
            return customer.potion;
        }

        public void Destroy()
        {
            Destroy(gameObject, 1f);
        }

        public void ButtonClicked()
        {
            if (!questDone) return;
            QuestbuttonClicked?.Invoke(customer);
            Destroy();
        }

        public void SetQuestDone(Color color)
        {
            questDone = true;
            customer.questDone = true;
            slotTextField.color = color;
        }
    }
}

