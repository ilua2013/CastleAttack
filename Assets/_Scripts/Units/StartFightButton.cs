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

    public Button Button => _button;
    public Button ButtonSkip => _buttonSkip;
    public Phase[] Phases => _phases;

    private void OnValidate()
    {
        _battleSystem = FindObjectOfType<BattleSystem>();
        _carsHand = FindObjectOfType<CardsHand>();
    }

    private void OnEnable()
    {
        _battleSystem.StepFinished += EnableButtonSkip;
        _carsHand.CardDrop += DisableButtonSkip;
    }

    private void OnDisable()
    {
        _battleSystem.StepFinished -= EnableButtonSkip;
        _carsHand.CardDrop += DisableButtonSkip;
    }

    public void Activate(bool isActive)
    {
        _button.interactable = isActive;
    }

    public IEnumerator SwitchPhase(PhaseType phaseType)
    {
        Phase phase = _phases.FirstOrDefault((phase) => phase.PhaseType == phaseType);

        yield return new WaitForSeconds(phase.Delay);

        _button.interactable = phase.IsActive;
        _buttonSkip.interactable = phase.IsActive;
    }

    private void EnableButtonSkip()
    {
        _buttonSkip.interactable = true;
    }

    private void DisableButtonSkip()
    {
        _buttonSkip.interactable = false;
    }
}
