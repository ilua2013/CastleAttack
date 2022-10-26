using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreSpawner : UnitSpawner
{
    private List<ProbaMonstr> _units = new List<ProbaMonstr>();

    protected override bool TryApplySpell(Card card, Vector3 place)
    {
        if (card.Description is OgreCardDescription)
        {
            OgreCardDescription description = card.Description as OgreCardDescription;
            ProbaMonstr ogre = Instantiate(description.UnitTemplate, SpawnPoint.position, Quaternion.identity);

            ogre.Init(TargetPoint, Button);
            ogre.Deaded += OnUnitDead;

            _units.Add(ogre);

            return true;
        }

        return false;
    }

    private void OnUnitDead(IMonstr unit)
    {

    }
}
