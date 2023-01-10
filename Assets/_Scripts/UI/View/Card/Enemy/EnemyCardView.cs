using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyCardView : MonoBehaviour
{
    [SerializeField] private UnitEnemyType _typeId;
    [SerializeField] private TMP_Text _amount;

    public UnitEnemyType TypeId => _typeId;

    public void Init(int amount)
    {
        _amount.text = $"x{amount}";
    }
}
