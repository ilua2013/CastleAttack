using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Fighter
{
    public Transform transform { get; private set; }

    [SerializeField] private FighterType _type;
    [SerializeField] private int _damage;
    [SerializeField] private int _maxHealth;
    [SerializeField] private Arrow _arrow;

    private IUnit _unit;
    private Vector3 _startRotate;
    private float _timeToDefaultRotate = 1f;
    private float _timeToMakeDamage = 0.35f;
    private float _speedRotate = 3f;
    private int _health = 1;

    public int Damage => _damage;
    public int Health => _health;
    public bool IsDead => _health < 1;
    public int MaxHealth => _maxHealth;
    public float RemainingHealth => (float)Health / MaxHealth;
    public FighterType FighterType => _type;
    public IUnit Unit => _unit;

    public event Action Died;
    public event Action ReadyToDie;
    public event Action EffectDied;
    public event Action<Transform> Died_getKiller;
    public event Action<Transform> Attacked_get;
    public event Action Attacked;
    public event Action RotatedToAttack;
    public event Action<int> Damaged;
    public event Action<int> Healed;
    public event Action<Fighter> Died_get;

    public void Init(IUnit unit, Transform transformUnit)
    {
        transform = transformUnit;
        _unit = unit;
        _health = _maxHealth;
        _startRotate = transform.forward;
    }

    public void Init(IUnit unit, Transform transformUnit, int damage, int health)
    {
        transform = transformUnit;
        _unit = unit;

        _maxHealth = health;
        _health = _maxHealth;
        _damage = damage;

        _startRotate = transform.forward;
    }

    public bool Attack(Fighter fighter, Action onEnd = null)
    {
        bool isFatal = false;

        if (_type == FighterType.Archer)
        {
            Arrow arrow = _unit.SpawnArrow(_arrow, transform.position);
            arrow.FlyTo(fighter.transform.position, () =>
            {
                isFatal = fighter.TakeDamage(this);
                onEnd?.Invoke();
            });
        }
        else
        {
            _unit.RotateTo(fighter.transform, () => isFatal = fighter.TakeDamage(this), onEnd);
        }

        if (fighter.FighterType == FighterType.MainWizzard) // получаем обратный урон если бьем по боссу
            fighter.Attack(this);

        Attacked?.Invoke();
        Attacked_get?.Invoke(fighter.transform);

        return isFatal;
    }

    public void RecoveryHealth(int value)
    {
        if (IsDead)
            return;

        _health += value;
        Healed?.Invoke(_health);
    }

    public void Die()
    {
        Died_get?.Invoke(this);
        Died?.Invoke();
    }

    public bool TakeDamage(Fighter fighter)
    {
        if (TakeDamage(fighter.Damage))
        {
            Died_getKiller?.Invoke(fighter.transform);
            return true;
        }

        return false;
    }

    public bool TakeDamage(int damage)
    {
        if (FighterType == FighterType.MainWizzard)
            if (_health <= damage)
            {
                ReadyToDie?.Invoke();
                return true;
            }

        _health = Math.Clamp(_health - damage, 0, _maxHealth);

        Damaged?.Invoke(damage);

        if (_health <= 0)
        {
            Die();
            EffectDied?.Invoke();

            return true;
        }

        return false;
    }

    public IEnumerator RotateTo(Transform lookAt, Action onFinish = null, Action onRotatedAttack = null)
    {
        Debug.Log("ROTATE TO " + lookAt.name.ToString());
        Vector3 target = lookAt.position - transform.position;
        Vector3 defaultRotate;
        target.y = 0;

        if (_unit is UnitFriend unitFriend)
            defaultRotate = Vector3.zero;
        else
            defaultRotate = new Vector3(0, 180, 0);

        while (Vector3.Distance(transform.forward, target.normalized) > 0.1f)
        {
            transform.forward = Vector3.MoveTowards(transform.forward, target, _speedRotate * Time.deltaTime);
            Debug.Log("xxx");
            yield return null;
        }

        RotatedToAttack?.Invoke();
        Debug.Log("a1");
        yield return new WaitForSeconds(_timeToMakeDamage);
        Debug.Log("a2");

        onRotatedAttack?.Invoke();
        
        yield return new WaitForSeconds(_timeToDefaultRotate - _timeToMakeDamage);
        Debug.Log("a3");

        while (transform.eulerAngles != defaultRotate)
        {
            transform.rotation = Quaternion.RotateTowards(Quaternion.Euler(transform.eulerAngles), Quaternion.Euler(defaultRotate), 600 * Time.deltaTime);
            Debug.Log("zzz ");
            yield return null;
        }

        Debug.Log("a4");
        onFinish?.Invoke();
    }
}

public enum FighterType
{
    Catapult, Archer, Attacker, Build, Shield, MainTarget, MainWizzard, AttackSpell, Cavalery
}
