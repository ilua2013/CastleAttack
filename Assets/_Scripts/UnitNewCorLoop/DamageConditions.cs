using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageConditions
{
    private const float ArcherToWarrior = 1f;
    private const float ArcherToCatapult = 1f;
    private const float ArcherToTower = 1f;
    private const float ArcherToBuild = 1f;
    private const float ArcherToWizzard = 1f;
    private const float ArcherToMainTarget = 1f;

    private const float WarriorToArcher = 1f;
    private const float WarriorToCatapult = 1f;
    private const float WarriorToTower = 1f;
    private const float WarriorToBuild = 1f;
    private const float WarriorToWizzard = 1f;
    private const float WarriorToMainTarget = 1f;

    private const float CatapultToArcher = 1f;
    private const float CatapultToWarrior = 1f;
    private const float CatapultToTower = 1f;
    private const float CatapultToBuild = 1f;
    private const float CatapultToWizzard = 1f;
    private const float CatapultToMainTarget = 1f;

    private const float TowerToArcher = 1f;
    private const float TowerToWarrior = 1f;
    private const float TowerToCatapult = 1f;
    private const float TowerToBuild = 1f;
    private const float TowerToWizzard = 1f;
    private const float TowerToMainTarget = 1f;

    private const float AttackSpellToArcher = 1f;
    private const float AttackSpellToTower = 1f;
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
                    case FighterType.Tower:
                        return damage * ArcherToTower;

                    case FighterType.Warrior:
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

            case FighterType.Tower:
                switch (defensive)
                {
                    case FighterType.Archer:
                        return damage * TowerToArcher;

                    case FighterType.Warrior:
                        return damage * TowerToWarrior;

                    case FighterType.Catapult:
                        return damage * TowerToCatapult;

                    case FighterType.Build:
                        return damage * TowerToBuild;

                    case FighterType.MainWizzard:
                        return damage * TowerToWizzard;

                    case FighterType.MainTarget:
                        return damage * TowerToMainTarget;
                }
                break;

            case FighterType.Warrior:
                switch (defensive)
                {
                    case FighterType.Archer:
                        return damage * WarriorToArcher;

                    case FighterType.Tower:
                        return damage * WarriorToTower;

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

                    case FighterType.Tower:
                        return damage * CatapultToTower;

                    case FighterType.Warrior:
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

                    case FighterType.Tower:
                        return damage * AttackSpellToTower;

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

        Debug.LogError("No Type");
        return 0;
    }
}
