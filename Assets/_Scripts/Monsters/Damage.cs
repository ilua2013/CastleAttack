using System;
using System.Collections;
using System.Collections.Generic;
using TypesMobs;
using UnityEngine;

[Serializable]
public class Damage
{
    [SerializeField] private TypeMob typeMob;
    [SerializeField] private int _taken;
    [SerializeField] private int _dealt;

    public TypeMob TypeMob => typeMob;
    public int Taken => _taken;
    public int Dealt => _dealt;
}
