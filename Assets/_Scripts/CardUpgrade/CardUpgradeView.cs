using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CardUpgradeView
{
    [SerializeField] private CardStage _stage;
    [SerializeField] private Sprite _background;
    [SerializeField] private Sprite _icon;

    public CardStage Stage => _stage;
    public Sprite Background => _background;
    public Sprite Icon => _icon;
}
