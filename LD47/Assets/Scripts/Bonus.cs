using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField] private const float DefaultMiningModifier = 1.5f;

    [SerializeField] private const float DefaultMiningTimingModifier = 5.0f;

    [SerializeField] private const float DefaultImmortalityTimingModifier = 5.0f;

    [SerializeField] private const bool DefaultImmortalityModifier = true;
    
    [SerializeField] private Cell cell;

    [SerializeField] private GlobalController globalController;

    public enum BonusType
    {
        Immortality,
        DamageBonus,
        Swap,
        CrossBomb,
        SplashBomb
    }
    public BonusType myBonusType;

    public void EnableBonus(
        Character character,
        bool immortalityModifier = DefaultImmortalityModifier,
        float miningModifier = DefaultMiningModifier, 
        float timingModifier = DefaultImmortalityTimingModifier)
    {
        cell = character.curCell;
        switch (myBonusType)
        {
            case BonusType.Immortality:
                EnableImmortality(character);
                break;
            case BonusType.DamageBonus:
                EnableDamageBonus(character);
                break;
            case BonusType.Swap:
                EnableImmortality(character);
                break;
            case BonusType.CrossBomb:
                EnableCrossBomb(character);
                break;
            case BonusType.SplashBomb:
                EnableSplashBomb(character, 3f);
                break;
            default:
                break;
        }
        cell.HideResBonSurf();
    }

    
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
        character.curCell.RemoveResBonSurf();
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
        character.curCell.RemoveResBonSurf();
        character.immortal = !immortalityModifier;
    }

    
    
    
    void EnableSplashBomb(
        Character character,
        float timingModifier)
    {
        StartCoroutine(DisableSplashBomb(character,timingModifier));
    }

    IEnumerator DisableSplashBomb(
        Character character,
        float timingModifier)
    {
        yield return new WaitForSeconds(timingModifier);

        foreach (var item in cell.NeighborCells)
        {
            if(cell.myState != Cell.State.StartingPoint)
                item.Value.Explode(character);
        }
        character.curCell.RemoveResBonSurf();
        cell.Explode(character);
    }
    
    
    
    
    void EnableCrossBomb(
        Character character, 
        float miningModifier = DefaultMiningModifier, 
        float timingModifier = DefaultMiningTimingModifier)
    {
        StartCoroutine(DisableCrossBomb(character, miningModifier, 3f));
    }
    
    IEnumerator DisableCrossBomb(
        Character character,
        float miningModifier = DefaultMiningModifier,
        float timingModifier = DefaultMiningTimingModifier)
    {
        yield return new WaitForSeconds(3f);
        
        if (cell.IsExist(Cell.Direction.Up))
        {
            cell.NeighborCells[Cell.Direction.Up].Explode(character);
        }
        if (cell.IsExist(Cell.Direction.Down))
        {
            cell.NeighborCells[Cell.Direction.Down].Explode(character);
        }
        if (cell.IsExist(Cell.Direction.Right))
        {
            cell.NeighborCells[Cell.Direction.Right].Explode(character);
        }
        if (cell.IsExist(Cell.Direction.Left))
        {
            cell.NeighborCells[Cell.Direction.Left].Explode(character);
        }
        character.curCell.RemoveResBonSurf();
        cell.Explode(character);
    }

    
    
    
    void EnableSwap(
        Character character, 
        float miningModifier = DefaultMiningModifier, 
        float timingModifier = DefaultMiningTimingModifier)
    {
        StartCoroutine(DisableCrossBomb(character, miningModifier, 0.5f));
    }

    IEnumerator DisableSwap(
        Character character,
        float miningModifier = DefaultMiningModifier,
        float timingModifier = DefaultMiningTimingModifier)
    {
        yield return new WaitForSeconds(0.5f);
        character.curCell.RemoveResBonSurf();
        character.GlobalController.SwapCharacters();
    }

}
