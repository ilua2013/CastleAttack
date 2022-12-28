using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Linq;

public class CommonDecks : Deck
{
    public override DeckType DeckType => DeckType.Combat;
}
