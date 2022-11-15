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
    [SerializeField] private UnitEnemy[] _targets;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private LevelSystem _levelSystem;

    private bool _isActivStepOne = true;
    private bool _isActivStepTwo = false;

    private void OnEnable()
    {
        _cardsSelection.CardSelected += StepOneTwo;
        _cardsHand.Spawned += StepTwoThree;
        _battleSystem.StepStarted += StepThreeFour;
        _cardReplenisher.CardUp += StepFourFive;
        _viewTutorial.EndStep += StepFive;
        _battleSystem.DiedAllEnemy += EndStep;
        _levelSystem.Wave1Finished += EnabledWaveTwo;
        _levelSystem.Wave2Finished += EnabledWaveThree;
    }

    private void OnDisable()
    {
        _cardsSelection.CardSelected -= StepOneTwo;
        _cardsHand.Spawned -= StepTwoThree;
        _battleSystem.StepStarted -= StepThreeFour;
        _cardReplenisher.CardUp -= StepFourFive;
        _viewTutorial.EndStep -= StepFive;
        _battleSystem.DiedAllEnemy -= EndStep;
        _levelSystem.Wave1Finished -= EnabledWaveTwo;
        _levelSystem.Wave2Finished -= EnabledWaveThree;
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
        if (_isActivStepOne == true)
        {
            _tutorialEffects.EffectOneTwo();
            _canvasTutorialFingerDraw.SetActive(true);
        }

        if(_isActivStepTwo == true)
        {
            _tutorialEffects.EffectOneTwo();
            _canvasTutorialFingerDraw.SetActive(true);
        }
    }

    private void StepTwoThree(UnitFriend unitFriend)
    {
        if (_isActivStepOne == true)
        {
            _tutorialEffects.EffectTwoThree();
            _canvasTutorialFingerDraw.SetActive(false);
            _canvasTutorialFingerTap.SetActive(true);
            _startFightButton.gameObject.SetActive(true);
        }

        if(_isActivStepTwo == true)
        {
            _tutorialEffects.EffectTwoThree();
            _canvasTutorialFingerDraw.SetActive(false);
            _canvasTutorialFingerTap.SetActive(true);          
        }
    }

    private void StepThreeFour()
    {
        if (_isActivStepOne == true)
        {
            _canvasTutorialFingerTap.SetActive(false);
            _tutorialEffects.EffectThreeFour();
        }
        if(_isActivStepTwo == true)
        {
            _canvasTutorialFingerTap.SetActive(false);
            _tutorialEffects.EffectThreeFour();
            _isActivStepTwo = false;
        }
    }

    private void StepFourFive(UnitCard cardLod, UnitCard cardNew)
    {
        if (_isActivStepOne == true)
        {
            _panelTutorial.gameObject.SetActive(true);
            _cardsSelection.TutorialTimeSwitch(_delaySelectionPanelTime);
            _cardSelectionView.TutorialTimeSwitch(_delaySelectionPanelTime);
            _viewTutorial.OnDrawOut(cardLod, cardNew, _delaySelectionPanelTime);
        }
    }
    private void StepFive()
    {        
            _isActivStepOne = false;
            _delaySelectionPanelTime = 0;
            _cardsSelection.TutorialTimeSwitch(_delaySelectionPanelTime);
            _cardSelectionView.TutorialTimeSwitch(_delaySelectionPanelTime);
            foreach (var target in _targets)
            {
                target.gameObject.SetActive(true);
            }
            _enemySpawner.TutorialEnemy(_targets);       
    }

    private void EndStep()
    {
        _tutorialEffects.EffectFourFive();       
    }

    private void EnabledWaveTwo()
    {
        _isActivStepTwo = true;
    }

    private void EnabledWaveThree()
    {
        _tutorialEffects.EffectFourFive();        
    }
}