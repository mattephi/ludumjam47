using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    private const float defaultMiningModifier = 1.5f;

    private const float defaultMiningTimingModifier = 5.0f;
    
    private const float defaultImmortalityTimingModifier = 5.0f;

    private const bool defaultImmortalityModifier = true;
    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}

    void EnableSpeedBonus(
        Character character, 
        float miningModifier = defaultMiningModifier, 
        float timingModifier = defaultMiningTimingModifier)
    {
        character.CurMiningSpeed *= miningModifier;
        StartCoroutine(DisableSpeedBonus(character, miningModifier, timingModifier));
    }

    IEnumerator DisableSpeedBonus(
        Character character,
        float miningModifier = defaultMiningModifier,
        float timingModifier = defaultMiningTimingModifier)
    {
        yield return new WaitForSeconds(timingModifier);
        character.CurMiningSpeed /= miningModifier;
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
