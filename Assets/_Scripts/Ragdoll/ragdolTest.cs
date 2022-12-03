using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ragdolTest : MonoBehaviour
{
    [Header("test")]
    public Transform from;
    public Vector3 _spawn;
    public bool _default;
    public bool startAttack;
    public float attack;
    public GameObject _prefab;

    private RagdollFriend _rag;

    private void Update()
    {
        if (_default)
        {
            _default = false;
            _rag = Instantiate(_prefab, _spawn, Quaternion.identity).GetComponentInChildren<Animator>().GetComponentInChildren<RagdollFriend>();
        }

        if (startAttack)
        {
            startAttack = false;
            //_rag.RagDollEnable(attack, from.position);
        }
    }
}
