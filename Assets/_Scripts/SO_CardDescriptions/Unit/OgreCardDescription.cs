using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit Card", menuName = "Cards/Unit Cards/New Ogre Card")]
public class OgreCardDescription : UnitCardDescription
{
    [SerializeField] private float _damagePerSecond;

    public float DamagePerSecond => _damagePerSecond;
}
