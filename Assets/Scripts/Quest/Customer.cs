using UnityEngine;

namespace Alchemystical
{
    public enum CustomerType
    {
        Invalid = -1,
        QuestGiver,
        Trader,
    }

    [System.Serializable]
    public class Customer
    {
        public Potion potion;
        public string conversationText;
        public int gold;
        public Sprite customerSprite;
        public CustomerType customerType;
        public bool questDone;

        public Customer(CustomerType type, Sprite sprite, string text)
        {
            customerType = type;
            customerSprite = sprite;
            conversationText = text;
        }
    }
}

