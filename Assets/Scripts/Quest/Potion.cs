using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPotion",menuName = "Alchemystical/Data/Potion")]
public class Potion : ScriptableObject
{
    public string potionName;
    public Sprite potionPicture;
    public List<EffectType> effects = new List<EffectType>();
    public bool lockedEffects = true;

    public void AddEffect(EffectType type)
    {
        if (lockedEffects) return;
        effects.Add(type);
    }

    public void RemoveAllEffects()
    {
        effects.Clear();
    }

    public List<EffectType> GetEffects()
    {
        return effects;
    }
}
