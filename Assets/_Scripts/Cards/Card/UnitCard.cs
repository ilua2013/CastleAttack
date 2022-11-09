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

    public event Action<UnitCard> StageUp;
    public event Action<UnitCard> CameBack;

    public UnitFriend UnitPrefab => _unitPrefab;
    public Card NextStage => _nextStage;
    public CardStage Stage => _stage;

    public void ComeBack()
    {
        if (Stage != CardStage.Three)
        {
            StageUp?.Invoke(this);
        }
        else
        {
            Amount++;
            CameBack?.Invoke(this);
        }
    }
}
