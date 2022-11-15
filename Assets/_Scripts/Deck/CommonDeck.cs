using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommonDeck : Deck
{
    public override string SaveKey => SaveController.Params.CommonDeck;
}
