using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreSpawner : UnitSpawner
{
    protected override bool TryApplySpell(CardDescription card, Vector3 place)
    {
        if (card is OgreCardDescription)
        {
            OgreCardDescription description = card as OgreCardDescription;
            ProbaMonstr ogre = Instantiate(description.UnitTemplate, SpawnPoint.position, Quaternion.identity);

            ogre.Init(TargetPoint, Button);

            return true;
        }

        return false;
    }
}
