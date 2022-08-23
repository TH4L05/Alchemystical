using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alchemystical
{
    [CreateAssetMenu(fileName = "newGameData", menuName = "Alchemystical/Data/GameData")]
    public class GameData : ScriptableObject
    {
        public Potion[] potions;
        public Ingredient[] ingredients;
    }
}

