using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{   
    [SerializeField] private CardsSelection _cardsSelection;    
    [SerializeField] private Button _startFightButton;
    [SerializeField] private GameObject _canvasTutorialFingerDraw;
    [SerializeField] private GameObject _canvasTutorialFingerTap;
    [SerializeField] private CardsHand _cardsHand;
    [SerializeField] private TutorialEffects _tutorialEffects;
    [SerializeField] private BattleSystem _battleSystem;

    private void OnEnable()
    {
        _cardsSelection.CardSelected += StepOneTwo;
        _cardsHand.Spawned += StepTwoThree;
        _battleSystem.StepStarted += StepThreeFour;

    }

    private void OnDisable()
    {
        _cardsSelection.CardSelected -= StepOneTwo;
        _cardsHand.Spawned -= StepTwoThree;
        _battleSystem.StepStarted -= StepThreeFour;
    }

    private void Start()
    {      
        _startFightButton.gameObject.SetActive(false);
        _canvasTutorialFingerDraw.SetActive(false);
        _canvasTutorialFingerTap.SetActive(false);
    }

    private void StepOneTwo(Card card)
    {       
        _tutorialEffects.EffectOneTwo();
        _canvasTutorialFingerDraw.SetActive(true);
    }

    private void StepTwoThree(UnitFriend unitFriend)
    {
        _tutorialEffects.EffectTwoThree();        
        _canvasTutorialFingerDraw.SetActive(false);
        _canvasTutorialFingerTap.SetActive(true);
        _startFightButton.gameObject.SetActive(true);      
    }

    private void StepThreeFour()
    {
        _canvasTutorialFingerTap.SetActive(false);
        _tutorialEffects.EffectThreeFour();
       
    }


}
