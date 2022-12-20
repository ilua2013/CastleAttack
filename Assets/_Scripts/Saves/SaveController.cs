using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveController
{
    public class Params
    {
        public const string CommonDeck = nameof(CommonDeck);
        public const string CombatDeck = nameof(CombatDeck);
        public const string Level = nameof(Level);
        public const string Coins = nameof(Coins);
        public const string IsSoundMuted = nameof(IsSoundMuted);
        public const string HandCapacity = nameof(HandCapacity);
        public const string IsTutorialCompleted = nameof(IsTutorialCompleted);
        public const string IsOpeningViewed = nameof(IsOpeningViewed);
        public const string WizardStats = nameof(WizardStats);
    }
}
