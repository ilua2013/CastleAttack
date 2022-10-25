using UnityEngine;

public class UnitCardDescription : CardDescription
{
    [SerializeField] private ProbaMonstr _unitTemplate;

    public ProbaMonstr UnitTemplate => _unitTemplate;
}
