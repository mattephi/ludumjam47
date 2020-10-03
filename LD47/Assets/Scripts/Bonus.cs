using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField] private const float defaultMiningModifier = 1.5f;

    [SerializeField] private const float defaultMiningTimingModifier = 5.0f;
    
    [SerializeField] private const float defaultImmortalityTimingModifier = 5.0f;

    [SerializeField] private const bool  defaultImmortalityModifier = true;
    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}

    void EnableDamageBonus(
        Character character, 
        float miningModifier = defaultMiningModifier, 
        float timingModifier = defaultMiningTimingModifier)
    {
        character.CurMiningDamage *= miningModifier;
        StartCoroutine(DisableDamageBonus(character, miningModifier, timingModifier));
    }

    IEnumerator DisableDamageBonus(
        Character character,
        float miningModifier = defaultMiningModifier,
        float timingModifier = defaultMiningTimingModifier)
    {
        yield return new WaitForSeconds(timingModifier);
        character.CurMiningDamage /= miningModifier;
    }
    
    void EnableImmortality(
        Character character,
        bool immortalityModifier = defaultImmortalityModifier,
        float timingModifier = defaultImmortalityTimingModifier)
    {
        character.Immortal = defaultImmortalityModifier;
        StartCoroutine(DisableImmortality(character, immortalityModifier));
    }

    IEnumerator DisableImmortality(
        Character character, 
        bool immortalityModifier = defaultImmortalityModifier,
        float timingModifier = defaultImmortalityTimingModifier)
    {
        yield return new WaitForSeconds(timingModifier);
        character.Immortal = !immortalityModifier;
    }

    void Swap()
    {
        // TODO: Write controlling class
        // Which will perform these changes.
    }

    
}
