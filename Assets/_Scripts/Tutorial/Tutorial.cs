using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Agava.YandexMetrica;

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
    [SerializeField] private UnitEnemy _unitEnemy;
   
    private bool _isActivStepTwo = false;
    private bool _isActivStepThree = false;
    private bool _isCountCardAlarm = true;
    private int _stepTutorial = 0;

    private void OnEnable()
    {
        //_cardsSelection.CardSelected += StepOneTwo;
        _cardsHand.Spawned += StepTwoThree;
        _battleSystem.StepStarted += StepThreeFour;
        //_cardReplenisher.CardUp += StepFourFive;
        _battleSystem.DiedAllEnemy += EndStep;
        _battleSystem.TutorialStopedUnit += StopGame;
        _levelSystem.Wave1Finished += EnabledWaveTwo;
        _levelSystem.Wave2Finished += EnabledWaveThree;
        _buttonStartGame.onClick.AddListener(StartGamePause);
        _unitEnemy.LevelUppedTutorial += StepFourFive;
    }

    private void OnDisable()
    {
       
        //_cardsSelection.CardSelected -= StepOneTwo;
        _cardsHand.Spawned -= StepTwoThree;
        _battleSystem.StepStarted -= StepThreeFour;
        //_cardReplenisher.CardUp -= StepFourFive;
        _battleSystem.DiedAllEnemy -= EndStep;
        _levelSystem.Wave1Finished -= EnabledWaveTwo;
        _levelSystem.Wave2Finished -= EnabledWaveThree;
        _battleSystem.TutorialStopedUnit -= StopGame;
        _buttonStartGame.onClick.RemoveListener(StartGamePause);
        _unitEnemy.LevelUppedTutorial -= StepFourFive;
    }

    private void Start()
    {
        _cardsSelection.gameObject.SetActive(false);
        _buttonStartGame.gameObject.SetActive(false);
        _startFightButton.gameObject.SetActive(false);
        StepOneTwo();
    }

    private void StepOneTwo(/*Card card*/)
    {
       
        if ( _stepTutorial == 0 )
        {        
            _panelViewSwitcher.PanelInstructinSpellAndMonster(true);
            ++_stepTutorial;
            Debug.Log(_stepTutorial);
#if !UNITY_EDITOR

            YandexMetrica.Send("StepTutorial 1");
#endif
            Debug.Log("Step 1");
            StartCoroutine(DelayGameStop());
        }
        
    }

    private IEnumerator DelayGameStop()
    {
        Debug.Log(Time.timeScale);
        yield return new WaitForSeconds(0.7f);      
        GameSwitch(0, true);
    }

    private void StepTwoThree(UnitFriend unitFriend)
    {
        if (_stepTutorial == 1 )
        {
            _tutorialEffects.EffectTwoThree();
            _panelViewSwitcher.TutorialFingerDraw(false);
            _panelViewSwitcher.AlarmCardCount(true);
            ++_stepTutorial;
#if !UNITY_EDITOR
            YandexMetrica.Send("StepTutorial 2");
#endif
            Debug.Log("Step 2");
            Debug.Log(_stepTutorial);
           
            StartCoroutine(DelayGameStop());           
           

        }
        if (_isActivStepTwo)
        {
            _panelViewSwitcher.PanelTwoTutorial(true);
            _isActivStepTwo = false;
            ++_stepTutorial;
#if !UNITY_EDITOR
            YandexMetrica.Send("StepTutorial 6");
#endif
            Debug.Log("Step 6");
            Debug.Log(_stepTutorial);

            StartCoroutine(DelayGameStop());
        }
        if (_isActivStepThree)
        {
            _panelViewSwitcher.PanelThree(true);
            _isActivStepThree = false;
#if !UNITY_EDITOR
            YandexMetrica.Send("StepTutorial 7");
#endif
            Debug.Log("Step 7");
            Debug.Log(_stepTutorial);
            ++_stepTutorial;
            StartCoroutine(DelayGameStop());
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
#if !UNITY_EDITOR
            YandexMetrica.Send("Step 3");
#endif
            Debug.Log("Step 3");
            Debug.Log(_stepTutorial);
        }
    }

    private void StopGame()
    {
        if (_stepTutorial == 3)
        {
            _panelViewSwitcher.PanelMonstr(true);
            ++_stepTutorial;
#if !UNITY_EDITOR
            YandexMetrica.Send("Step 4");
#endif
            Debug.Log("Step 4");
            Debug.Log(_stepTutorial);
            StartCoroutine(DelayGameStop());
        }
    }

    private void StepFourFive()
    {
        if (_stepTutorial == 5)
        {
            _panelViewSwitcher.UpgradeCardTutorial(true);            
            ++_stepTutorial;
            StartCoroutine(DelayGameStop());
            Debug.Log(_stepTutorial);
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
#if !UNITY_EDITOR
            YandexMetrica.Send("Step 5");
#endif
        Debug.Log("Step 5");
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

                //StopCoroutine(DelayGameStop());
                //_cardsSelection.Phases.
                _tutorialEffects.EffectOneTwo();
                _panelViewSwitcher.TutorialFingerDraw(true);
                _panelViewSwitcher.PanelInstructinSpellAndMonster(false);
                _cardsSelection.TutorialPhaseEnable();
                break;

            case 2:
                _tutorialEffects.EffectStartButton();
                _panelViewSwitcher.AlarmCardCount(false);
                _panelViewSwitcher.TutorialFingerTap(true);
                _startFightButton.gameObject.SetActive(true);
                break;

            case 4:
                _panelViewSwitcher.PanelMonstr(false);
                StepFive();
                break;

            case 5:
                StartCoroutine(PanelVievBox());
                //_panelViewSwitcher.PanelViewBox(false);
                //_panelViewSwitcher.UpgradeCardTutorial(false);
                //StepFive();
                break;

            case 6:
                _panelViewSwitcher.UpgradeCardTutorial(false);
                //_panelViewSwitcher.PanelViewBox(false);
                break;

            case 7:
                _panelViewSwitcher.PanelTwoTutorial(false);
                //_panelViewSwitcher.PanelTwoTutorial(false);
                break;
            case 8:
                _panelViewSwitcher.PanelThree(false);
                //_panelViewSwitcher.PanelThree(false);
                break;


            default:
                break;
        }

    }

    private IEnumerator PanelVievBox()
    {
        yield return new WaitForSeconds(2.3f);
        _panelViewSwitcher.PanelViewBox(false);
    }

    private void EnabledWaveThree()
    {
        _tutorialEffects.EffectFourFive();
        _isActivStepThree = true;
    }

    private void GameSwitch(float time, bool isActived)
    {
        Time.timeScale = time;
        Debug.Log(isActived);
        _buttonStartGame.gameObject.SetActive(isActived);
    }
}