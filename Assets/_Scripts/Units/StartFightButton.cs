using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StartFightButton : MonoBehaviour, IPhaseHandler
{
    [SerializeField] private Button _button;
    [SerializeField] private Button _buttonSkip;
    [SerializeField] private Phase[] _phases;
    [SerializeField] private BattleSystem _battleSystem;
    [SerializeField] private CardsHand _carsHand;

    private List<KeyCode> _fightKeys = new List<KeyCode>() { KeyCode.Space, KeyCode.Return };

    public Button Button => _button;
    public Button ButtonSkip => _buttonSkip;
    public Phase[] Phases => _phases;

    public event Action FightClicked;
    public event Action SkipClicked;

    private void OnValidate()
    {
        if (_battleSystem == null)
            _battleSystem = FindObjectOfType<BattleSystem>();

        if (_carsHand == null)
            _carsHand = FindObjectOfType<CardsHand>();
    }

    private void OnEnable()
    {
        _battleSystem.StepFinished += EnableButtonSkip;
        _carsHand.CardDrop += DisableButtonSkip;
        _button.onClick.AddListener(OnFightClick);
        _buttonSkip.onClick.AddListener(OnSkipClick);
    }

    private void OnDisable()
    {
        _battleSystem.StepFinished -= EnableButtonSkip;
        _carsHand.CardDrop += DisableButtonSkip;
        _button.onClick.RemoveListener(OnFightClick);
        _buttonSkip.onClick.RemoveListener(OnSkipClick);
    }

    private void Update()
    {
        foreach (KeyCode key in _fightKeys)
        {
            if (Input.GetKeyDown(key))
            {
                FightClicked?.Invoke();
                return;
            }
        }
    }

    public void Activate(bool isActive)
    {
        _button.interactable = isActive;
        _buttonSkip.interactable = isActive;
    }

    public IEnumerator SwitchPhase(PhaseType phaseType)
    {
        Phase phase = _phases.FirstOrDefault((phase) => phase.PhaseType == phaseType);

        yield return new WaitForSeconds(phase.Delay);

        Activate(phase.IsActive);
    }

    private void EnableButtonSkip()
    {
        _buttonSkip.interactable = true;
    }

    private void DisableButtonSkip()
    {
        _buttonSkip.interactable = false;
    }

    private void OnFightClick()
    {
        FightClicked?.Invoke();
    }

    private void OnSkipClick()
    {
        SkipClicked?.Invoke();
    }
}
