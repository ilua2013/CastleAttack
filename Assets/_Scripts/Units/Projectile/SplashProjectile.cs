using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Arrow))]
public class SplashProjectile : MonoBehaviour
{
    private const string DemolishAnimationState = "ZombieDemolish";

    [SerializeField] private Arrow _arrow;
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _vfx;
    [SerializeField] private ParticleSystem _firstPiece;
    [SerializeField] private ParticleSystem _secondPiece;

    private void OnValidate()
    {
        if (_arrow == null)
            _arrow = GetComponent<Arrow>();

        if (_animator == null)
            _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _arrow.Reached += OnReached;
    }

    private void OnDisable()
    {
        _arrow.Reached -= OnReached;
    }

    public void OnLandingAnimationEvent()
    {
        _vfx.Play();
    }

    public void OnPiecesLandingAnimationEvent()
    {
        _firstPiece.Play();
        _secondPiece.Play();
    }

    private void OnReached(Cell cell)
    {
        Cell left = cell.Left;
        Cell right = cell.Right;

        if (IsUnitExists(left))
            left.CurrentUnit.Fighter.TakeDamage(_arrow.Fighter);

        if (IsUnitExists(right))
            right.CurrentUnit.Fighter.TakeDamage(_arrow.Fighter);

        _animator.Play(DemolishAnimationState);
    }

    private bool IsUnitExists(Cell cell)
    {
        return cell != null && cell.CurrentUnit != null && cell.CurrentUnit is UnitFriend;
    }
}
