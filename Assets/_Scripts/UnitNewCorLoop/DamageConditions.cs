using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageConditions
{
    private const float ArcherToWarrior = 1f;
    private const float ArcherToCatapult = 1f;
    private const float ArcherToTower = 1f;

    private const float WarriorToArcher = 1f;
    private const float WarriorToCatapult = 1f;
    private const float WarriorToTower = 1f;

    private const float CatapultToArcher = 1f;
    private const float CatapultToWarrior = 1f;
    private const float CatapultToTower = 1f;

    private const float TowerToArcher = 1f;
    private const float TowerToWarrior = 1f;
    private const float TowerToCatapult = 1f;

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
                }
                break;
        }

        Debug.LogError("No Type");
        return 0;
    }
}
