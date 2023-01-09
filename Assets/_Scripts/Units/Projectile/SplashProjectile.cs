using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Arrow))]
public class SplashProjectile : MonoBehaviour
{
    [SerializeField] private Arrow _arrow;

    private void OnValidate()
    {
        _arrow = GetComponent<Arrow>();
    }

    private void OnEnable()
    {
        _arrow.Reached += OnReached;
    }

    private void OnDisable()
    {
        _arrow.Reached -= OnReached;
    }

    private void OnReached(Cell cell)
    {
        Cell left = cell.Left;
        Cell right = cell.Right;

        if (IsUnitExists(left))
            left.CurrentUnit.Fighter.TakeDamage(_arrow.Fighter);

        if (IsUnitExists(right))
            right.CurrentUnit.Fighter.TakeDamage(_arrow.Fighter);
    }

    private bool IsUnitExists(Cell cell)
    {
        return cell != null && cell.CurrentUnit != null && cell.CurrentUnit is UnitFriend;
    }
}
