using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleChanger : MonoBehaviour
{
    public bool test;
    [Header("Link")]
    [SerializeField] private CoinsWallet _coinsWallet;
    [SerializeField] private Transform _parentCastle;
    [SerializeField] private Transform _parentNewCastle;
    [SerializeField] private Castle[] _castles;
    [Header("Params")]
    [SerializeField] private int _damage;
    [SerializeField] private float _delayDamage = 1.5f;
    [SerializeField] private float _delayChangeCastle = 5f;

    private Castle _castle;
    private Animator _animator;

    private void OnValidate()
    {
        _coinsWallet = FindObjectOfType<CoinsWallet>();
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        if (SaveCastle.TypeCastle == -1)
        {
            SaveCastle.TypeCastle = (int)CastleType.Var1;
            SpawnNewCastle(_parentCastle);
        }
        else
        {
            LoadCastle();
        }
    }

    private void Start()
    {
        if (SaveCastle.AttackCastle)
        {
            StartCoroutine(_castle.TakeDamage(_damage, _delayDamage, () =>
            {
                Invoke(nameof(ChangeCastle), _delayChangeCastle);
                _coinsWallet.Add(_castle.Reward, _delayChangeCastle);
            }));

            SaveCastle.AttackCastle = false;
        }
    }

    private void Update()
    {
        if (test)
        {
            test = false;

            StartCoroutine(_castle.TakeDamage(_damage, _delayDamage, () =>
            {
                Invoke(nameof(ChangeCastle), _delayChangeCastle);
                _coinsWallet.Add(_castle.Reward, _delayChangeCastle);
            }));
        }
    }

    private void ChangeCastle()
    {
        SaveCastle.TypeCastle = (CastleType)SaveCastle.TypeCastle == CastleType.Var5 ? 0 : SaveCastle.TypeCastle + 1;

        SpawnNewCastle(_parentNewCastle);

        _animator.SetTrigger("ChangeCastle");
    }

    private void LoadCastle()
    {
        foreach (var item in _castles)
        {
            if((int)item.CastleType == SaveCastle.TypeCastle)
            {
                var castle = Instantiate(item, _parentCastle.position, Quaternion.identity, _parentCastle);
                castle.SetHealth(SaveCastle.Health);

                _castle = castle;
            }
        }
    }

    private void SpawnNewCastle(Transform parent)
    {
        foreach (var item in _castles)
        {
            if ((int)item.CastleType == SaveCastle.TypeCastle)
            {
                var castle = Instantiate(item, parent.position, Quaternion.identity, parent);
                castle.transform.localRotation =Quaternion.Euler( Vector3.zero);

                SaveCastle.Health = castle.MaxHealth;

                _castle = castle;
            }
        }
    }
}
public enum CastleType
{
    Var1, Var2, Var3, Var4, Var5
}