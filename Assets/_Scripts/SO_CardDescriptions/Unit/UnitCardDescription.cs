using UnityEngine;

public class UnitCardDescription : CardDescription
{
    [SerializeField] private Unit _unitTemplate;

    public Unit UnitTemplate => _unitTemplate;
}
