using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private CardsSelection _cardsSelection;
    [SerializeField] private CardSelectionView _cardSelectionView;
    [SerializeField] private Button _startFightButton;
    [SerializeField] private GameObject _canvasTutorialFingerDraw;
    [SerializeField] private GameObject _canvasTutorialFingerTap;
    [SerializeField] private GameObject _panelTutorial;
    [SerializeField] private CardsHand _cardsHand;
    [SerializeField] private TutorialEffects _tutorialEffects;
    [SerializeField] private BattleSystem _battleSystem;
    [SerializeField] private CardReplenisher _cardReplenisher;
    [SerializeField] private CardViewTutorial _viewTutorial;
    [SerializeField] private float _delaySelectionPanelTime;

    private bool _isActivStep = true;


    private void OnEnable()
    {
        _cardsSelection.CardSelected += StepOneTwo;
        _cardsHand.Spawned += StepTwoThree;
        _battleSystem.StepStarted += StepThreeFour;
        _cardReplenisher.CardUp += StepFourFive;
        _viewTutorial.EndStep += StepFive;

    }

    private void OnDisable()
    {
        _cardsSelection.CardSelected -= StepOneTwo;
        _cardsHand.Spawned -= StepTwoThree;
        _battleSystem.StepStarted -= StepThreeFour;
        _cardReplenisher.CardUp -= StepFourFive;
        _viewTutorial.EndStep -= StepFive;
    }

    private void Start()
    {
        _startFightButton.gameObject.SetActive(false);
        _canvasTutorialFingerDraw.SetActive(false);
        _canvasTutorialFingerTap.SetActive(false);
        _panelTutorial.gameObject.SetActive(false);
    }

    private void StepOneTwo(Card card)
    {
        if (_isActivStep ==true)
        {
            _tutorialEffects.EffectOneTwo();
            _canvasTutorialFingerDraw.SetActive(true);
        }

    }

    private void StepTwoThree(UnitFriend unitFriend)
    {
        if (_isActivStep == true)
        {
            _tutorialEffects.EffectTwoThree();
            _canvasTutorialFingerDraw.SetActive(false);
            _canvasTutorialFingerTap.SetActive(true);
            _startFightButton.gameObject.SetActive(true);
        }
    }

    private void StepThreeFour()
    {
        if (_isActivStep == true)
        {
            _canvasTutorialFingerTap.SetActive(false);
            _tutorialEffects.EffectThreeFour();
        }

    }

    private void StepFourFive(UnitCard cardLod, UnitCard cardNew)
    {
        if (_isActivStep == true)
        {
            _panelTutorial.gameObject.SetActive(true);
            _cardsSelection.TutorialTimeSwitch(_delaySelectionPanelTime);
            _cardSelectionView.TutorialTimeSwitch(_delaySelectionPanelTime);
            _viewTutorial.OnDrawOut(cardLod, cardNew, _delaySelectionPanelTime);
        }
    }
    private void StepFive()
    {
        _isActivStep = false;
        _delaySelectionPanelTime = 0;
        _cardsSelection.TutorialTimeSwitch(_delaySelectionPanelTime);
        _cardSelectionView.TutorialTimeSwitch(_delaySelectionPanelTime);
    }
}


