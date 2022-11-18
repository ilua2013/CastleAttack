using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class UnitCard : Card
{
    [SerializeField] private CardStage _stage;
    [SerializeField] private UnitCard _nextStage;
    [SerializeField] private UnitFriend _unitPrefab;
    [SerializeField] private GameObject _projectionPrefab;

    public event Action<UnitCard> StageUp;
    public event Action<UnitCard> CameBack;
    public event Action<int> AmountChanged;

    public UnitFriend UnitPrefab => _unitPrefab;
    public UnitCard NextStage => _nextStage;
    public CardStage Stage => _stage;

    public override GameObject ProjectionPrefab => _projectionPrefab;

    public void ComeBack()
    {
        if (Stage != CardStage.Three)
        {
            StageUp?.Invoke(this);
        }
        else
        {
            if (Amount == 0)
                CameBack?.Invoke(this);

            Merge();
        }
    }

    public void Merge()
    {
        Amount++;
        AmountChanged?.Invoke(Amount);
    }
}
