using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsComparer : IComparer<CardHoverView>
{
    public int Compare(CardHoverView x, CardHoverView y)
    {
        if (x.StartIndex < y.StartIndex)
            return -1;

        if (x.StartIndex > y.StartIndex)
            return 1;

        return 0;
    }
}
