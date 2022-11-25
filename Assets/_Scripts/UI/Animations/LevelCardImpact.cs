using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCardImpact : MonoBehaviour
{
    private const string AnimationName = "Impact";

    [SerializeField] private Animator _animator;
    [SerializeField] private CardLevelUpButton _lvlButton;

    private void OnEnable()
    {
        _lvlButton.LevelUp += OnLevelUp;
    }

    private void OnDisable()
    {
        _lvlButton.LevelUp -= OnLevelUp;
    }

    private void OnLevelUp()
    {
        _animator.Play(AnimationName);
    }
}
