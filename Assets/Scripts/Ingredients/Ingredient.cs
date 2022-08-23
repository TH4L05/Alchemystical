using System.Collections.Generic;
using UnityEngine;

namespace Alchemystical
{
    [CreateAssetMenu(fileName = "newIngredient", menuName = "ScriptableObject/Ingredient", order = 2)]
    public class Ingredient : ScriptableObject
    {
        public string ingredientName;
        public Sprite icon;
        public int shopPrice;
        public Color brewColor;

        public EffectType clockwiseEffect;
        public EffectType counterClockwiseEffect;

        public bool cwEffectUnlocked = false;
        public bool ccwEffectUnlocked = false;

        public (bool,bool) GetEffectStatus()
        {
            return (cwEffectUnlocked,ccwEffectUnlocked);
        }

        public void UnlockEffect(MotionType motion)
        {
            switch (motion)
            {
                case MotionType.Invalid:
                    break;

                case MotionType.Clockwise:
                    cwEffectUnlocked = true;
                    break;

                case MotionType.Counterclockwise:
                    ccwEffectUnlocked = true;
                    break;

                case MotionType.Count:
                    break;

                default:
                    break;
            }
        }

    }
}
