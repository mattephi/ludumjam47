using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField] private const float DefaultMiningModifier = 1.5f;

    [SerializeField] private const float DefaultMiningTimingModifier = 5.0f;
    
    [SerializeField] private const float DefaultImmortalityTimingModifier = 5.0f;

    [SerializeField] private const bool  DefaultImmortalityModifier = true;

    [SerializeField] private GlobalController globalController;

    void EnableDamageBonus(
        Character character, 
        float miningModifier = DefaultMiningModifier, 
        float timingModifier = DefaultMiningTimingModifier)
    {
        character.curDamage *= miningModifier;
        StartCoroutine(DisableDamageBonus(character, miningModifier, timingModifier));
    }

    IEnumerator DisableDamageBonus(
        Character character,
        float miningModifier = DefaultMiningModifier,
        float timingModifier = DefaultMiningTimingModifier)
    {
        yield return new WaitForSeconds(timingModifier);
        character.curDamage /= miningModifier;
    }
    
    void EnableImmortality(
        Character character,
        bool immortalityModifier = DefaultImmortalityModifier,
        float timingModifier = DefaultImmortalityTimingModifier)
    {
        character.immortal = DefaultImmortalityModifier;
        StartCoroutine(DisableImmortality(character, immortalityModifier));
    }

    IEnumerator DisableImmortality(
        Character character, 
        bool immortalityModifier = DefaultImmortalityModifier,
        float timingModifier = DefaultImmortalityTimingModifier)
    {
        yield return new WaitForSeconds(timingModifier);
        character.immortal = !immortalityModifier;
    }

    void Swap()
    {
        globalController.SwapCharacters();
        // TODO: Write controlling class
        // Which will perform these changes.
    }

    
}
