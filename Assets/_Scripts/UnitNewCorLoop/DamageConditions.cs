using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageConditions
{
    private const float ArcherToWarrior = 2f;
    private const float ArcherToCatapult = 1f;
    private const float ArcherToShield = 1f;
    private const float ArcherToBuild = 1f;
    private const float ArcherToWizzard = 1f;
    private const float ArcherToMainTarget = 1f;

    private const float WarriorToArcher = 1f;
    private const float WarriorToCatapult = 1f;
    private const float WarriorToShield = 2f;
    private const float WarriorToBuild = 1f;
    private const float WarriorToWizzard = 1f;
    private const float WarriorToMainTarget = 1f;

    private const float CatapultToArcher = 1f;
    private const float CatapultToWarrior = 1f;
    private const float CatapultToShield = 1f;
    private const float CatapultToBuild = 2f;
    private const float CatapultToWizzard = 1f;
    private const float CatapultToMainTarget = 1f;

    private const float ShieldToArcher = 2f;
    private const float ShieldToWarrior = 1f;
    private const float ShieldToCatapult = 1f;
    private const float ShieldToBuild = 1f;
    private const float ShieldToWizzard = 1f;
    private const float ShieldToMainTarget = 1f;

    private const float AttackSpellToArcher = 1f;
    private const float AttackSpellToShield = 1f;
    private const float AttackSpellToBuild = 1f;
    private const float AttackSpellToWizzard = 1f;
    private const float AttackSpellToMainTarget = 0.2f;

    public static float CalculateDamage(FighterType attacking, FighterType defensive, int damage)
    {
        if (attacking == defensive)
            return damage;

        switch (attacking)
        {
            case FighterType.Archer:
                switch (defensive)
                {
                    case FighterType.Shield:
                        return damage * ArcherToShield;

                    case FighterType.Attacker:
                        return damage * ArcherToWarrior;

                    case FighterType.Catapult:
                        return damage * ArcherToCatapult;

                    case FighterType.Build:
                        return damage * ArcherToBuild;

                    case FighterType.MainWizzard:
                        return damage * ArcherToWizzard;

                    case FighterType.MainTarget:
                        return damage * ArcherToMainTarget;
                }
                break;

            case FighterType.Shield:
                switch (defensive)
                {
                    case FighterType.Archer:
                        return damage * ShieldToArcher;

                    case FighterType.Attacker:
                        return damage * ShieldToWarrior;

                    case FighterType.Catapult:
                        return damage * ShieldToCatapult;

                    case FighterType.Build:
                        return damage * ShieldToBuild;

                    case FighterType.MainWizzard:
                        return damage * ShieldToWizzard;

                    case FighterType.MainTarget:
                        return damage * ShieldToMainTarget;
                }
                break;

            case FighterType.Attacker:
                switch (defensive)
                {
                    case FighterType.Archer:
                        return damage * WarriorToArcher;

                    case FighterType.Shield:
                        return damage * WarriorToShield;

                    case FighterType.Catapult:
                        return damage * WarriorToCatapult;

                    case FighterType.Build:
                        return damage * WarriorToBuild;

                    case FighterType.MainWizzard:
                        return damage * WarriorToWizzard;

                    case FighterType.MainTarget:
                        return damage * WarriorToMainTarget;
                }
                break;

            case FighterType.Catapult:
                switch (defensive)
                {
                    case FighterType.Archer:
                        return damage * CatapultToArcher;

                    case FighterType.Shield:
                        return damage * CatapultToShield;

                    case FighterType.Attacker:
                        return damage * CatapultToWarrior;

                    case FighterType.Build:
                        return damage * CatapultToBuild;

                    case FighterType.MainWizzard:
                        return damage * CatapultToWizzard;

                    case FighterType.MainTarget:
                        return damage * CatapultToMainTarget;
                }
                break;

            case FighterType.AttackSpell:
                switch (defensive)
                {
                    case FighterType.Archer:
                        return damage * AttackSpellToArcher;

                    case FighterType.Shield:
                        return damage * AttackSpellToShield;

                    case FighterType.Build:
                        return damage * AttackSpellToBuild;

                    case FighterType.MainWizzard:
                        return damage * AttackSpellToWizzard;

                    case FighterType.MainTarget:
                        return damage * AttackSpellToMainTarget;

                    default:
                        return damage;
                }
        }

        return damage;
    }
}
