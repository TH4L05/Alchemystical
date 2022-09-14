

using System.Collections.Generic;
using UnityEngine;

namespace Alchemystical
{
    public class OrderInfoUI : MonoBehaviour
    {
        [SerializeField] private List<OrderUIInfoSlot> orderUIInfoSlots = new List<OrderUIInfoSlot>();


        public void UpdateSlot(int index, Customer customer)
        {
            orderUIInfoSlots[index].UpdateSlot(customer);
        }

        public void ActivateSlot(int index)
        {
            orderUIInfoSlots[index].gameObject.SetActive(true);
        }

        public void DeactivateSlot(int index)
        {
            orderUIInfoSlots[index].gameObject.SetActive(false);
        }

    }
}

