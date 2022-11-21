using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private CardsSelection _cardsSelection;
    [SerializeField] private Button _startFightButton;
    [SerializeField] private CardsHand _cardsHand;
    [SerializeField] private TutorialEffects _tutorialEffects;
    [SerializeField] private BattleSystem _battleSystem;
    [SerializeField] private CardReplenisher _cardReplenisher;
    [SerializeField] private float _delaySelectionPanelTime;
    [SerializeField] private UnitEnemy[] _targets;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private LevelSystem _levelSystem;
    [SerializeField] private Button _buttonStartGame;
    [SerializeField] private TutorialPanelViewSwitcher _panelViewSwitcher;
   
    private bool _isActivStepTwo = false;    
    private int _stepTutorial = 0;

    private void OnEnable()
    {
        _cardsSelection.CardSelected += StepOneTwo;
        _cardsHand.Spawned += StepTwoThree;
        _battleSystem.StepStarted += StepThreeFour;
        _cardReplenisher.CardUp += StepFourFive;
        _battleSystem.DiedAllEnemy += EndStep;
        _battleSystem.TutorialStopedUnit += StopGame;
        _levelSystem.Wave1Finished += EnabledWaveTwo;
        _levelSystem.Wave2Finished += EnabledWaveThree;
        _buttonStartGame.onClick.AddListener(StartGamePause);
    }

    private void OnDisable()
    {
        _cardsSelection.CardSelected -= StepOneTwo;
        _cardsHand.Spawned -= StepTwoThree;
        _battleSystem.StepStarted -= StepThreeFour;
        _cardReplenisher.CardUp -= StepFourFive;
        _battleSystem.DiedAllEnemy -= EndStep;
        _levelSystem.Wave1Finished -= EnabledWaveTwo;
        _levelSystem.Wave2Finished -= EnabledWaveThree;
        _battleSystem.TutorialStopedUnit -= StopGame;
        _buttonStartGame.onClick.RemoveListener(StartGamePause);
    }

    private void Start()
    {
        _buttonStartGame.gameObject.SetActive(false);
        _startFightButton.gameObject.SetActive(false);
    }

    private void StepOneTwo(Card card)
    {
        if ( _stepTutorial == 0 )
        {        
            _panelViewSwitcher.PanelInstructinSpellAndMonster(true);
            ++_stepTutorial;
            Debug.Log(_stepTutorial);           
            StartCoroutine(DelayGameStop());
        }
    }

    private IEnumerator DelayGameStop()
    {
        Debug.Log(Time.timeScale);
        yield return new WaitForSeconds(0.1f);      
        GameSwitch(0, true);
    }

    private void StepTwoThree(UnitFriend unitFriend)
    {
        if (_stepTutorial == 1 )
        {
            _tutorialEffects.EffectTwoThree();
            _panelViewSwitcher.TutorialFingerDraw(false);
            _panelViewSwitcher.TutorialFingerTap(true);
            _startFightButton.gameObject.SetActive(true);
            ++_stepTutorial;
            Debug.Log(_stepTutorial);
        }
    }

    private void StepThreeFour()
    {
        if (_stepTutorial == 2 )
        {
            _panelViewSwitcher.TutorialFingerTap(false);
            _tutorialEffects.EffectThreeFour();
            _isActivStepTwo = false;
            ++_stepTutorial;
            Debug.Log(_stepTutorial);
        }
    }

    private void StopGame()
    {
        if (_stepTutorial == 3)
        {
            _panelViewSwitcher.PanelMonstr(true);
            ++_stepTutorial;
            GameSwitch(0, true);
            Debug.Log(_stepTutorial);
        }
    }

    private void StepFourFive(UnitCard cardLod, UnitCard cardNew)
    {
        if (_stepTutorial == 4)
        {
            _panelViewSwitcher.UpgradeCardTutorial(true);
            _panelViewSwitcher.OnDrawOut(cardLod, cardNew);
            ++_stepTutorial;
            Debug.Log(_stepTutorial);
            GameSwitch(0, true);

        }
    }
    private void StepFive()
    {
        foreach (var target in _targets)
        {
            target.gameObject.SetActive(true);
        }
        _tutorialEffects.ParticleBox();
        _enemySpawner.TutorialEnemy(_targets);
        _panelViewSwitcher.PanelViewBox(true);
        ++_stepTutorial;
        Debug.Log(_stepTutorial);
        GameSwitch(0, true);       
    }

    private void EndStep()
    {
        _tutorialEffects.EffectFourFive();
    }

    private void EnabledWaveTwo()
    {
        _isActivStepTwo = true;
    }

    private void StartGamePause()
    {
        GameSwitch(1, false);      

        switch (_stepTutorial)
        {
            case 1:

                StopCoroutine(DelayGameStop());
                _tutorialEffects.EffectOneTwo();
                _panelViewSwitcher.TutorialFingerDraw(true);
                _panelViewSwitcher.PanelInstructinSpellAndMonster(false);
                break;

            case 4:
                _panelViewSwitcher.PanelMonstr(false);
                break;

            case 5:
                _panelViewSwitcher.UpgradeCardTutorial(false);
                StepFive();
                break;

            case 6:
                _panelViewSwitcher.PanelViewBox(false);
                break;

            default:
                break;
        }

    }

    private void EnabledWaveThree()
    {
        _tutorialEffects.EffectFourFive();
    }

    private void GameSwitch(float time, bool isActived)
    {
        Time.timeScale = time;
        Debug.Log(isActived);
        _buttonStartGame.gameObject.SetActive(isActived);
    }
}