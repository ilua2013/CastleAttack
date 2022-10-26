using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreSpawner : UnitSpawner
{
    private TestFinisher _finisher;
    private List<IUnit> _units = new List<IUnit>();

    private void Awake()
    {
        _finisher = FindObjectOfType<TestFinisher>();
    }

    private void OnEnable()
    {
        _finisher.Finished += OnFinished;
    }

    private void OnDisable()
    {
        _finisher.Finished -= OnFinished;
    }

    protected override bool TryApplyUnit(Card card, Vector3 place)
    {
        if (card.Description is OgreCardDescription)
        {
            OgreCardDescription description = card.Description as OgreCardDescription;
            ProbaMonstr ogre = Instantiate(description.UnitTemplate, SpawnPoint.position, Quaternion.identity);

            ogre.Init(card, TargetPoint, Button);
            ogre.Deaded += OnUnitDead;

            _units.Add(ogre);

            return true;
        }

        return false;
    }

    private void OnFinished()
    {
        foreach (var unit in _units)
        {
            unit.Card.ComeBack();
            unit.ReurnToHand();
        }

        _units.Clear();
    }

    private void OnUnitDead(IMonstr monstr, IUnit unit)
    {
        monstr.Deaded -= OnUnitDead;
        _units.Remove(unit);
    }
}
