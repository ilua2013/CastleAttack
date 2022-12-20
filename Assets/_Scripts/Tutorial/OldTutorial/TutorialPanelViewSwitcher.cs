using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPanelViewSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject _panelViewMonstr;
    [SerializeField] private GameObject _panelViewBox;
    [SerializeField] private GameObject _canvasTutorialFingerDraw;
    [SerializeField] private GameObject _canvasTutorialFingerTap;
    [SerializeField] private GameObject _panelUpgradeCardTutorial;
    [SerializeField] private GameObject _panelInstructionSpellAndMonster;
    [SerializeField] private GameObject _panelTwoTutorial;
    [SerializeField] private GameObject _panelThreeTutorial;
    [SerializeField] private CardLevelUp _cardLevelUpOld;
    [SerializeField] private CardLevelUp _cardLevelUpNew;
    [SerializeField] private GameObject _alarmCardCount;

    private void Start()
    {
        _panelViewMonstr.SetActive(false);
        _panelViewBox.SetActive(false);
        _canvasTutorialFingerDraw.SetActive(false);
        _canvasTutorialFingerTap.SetActive(false);
        _panelUpgradeCardTutorial.SetActive(false);
        _alarmCardCount.SetActive(false);
    }

    public void PanelInstructinSpellAndMonster(bool actived)
    {
        _panelInstructionSpellAndMonster.SetActive(actived);
    }

    public void PanelMonstr(bool actived)
    {
        _panelViewMonstr.SetActive(actived);
    }

    public void AlarmCardCount(bool actived)
    {
        _alarmCardCount.SetActive(actived);
    }

    public void PanelViewBox(bool actived)
    {
        _panelViewBox.SetActive(actived);
    }

    public void TutorialFingerDraw(bool actived)
    {
        _canvasTutorialFingerDraw.SetActive(actived);
    }

    public void TutorialFingerTap(bool actived)
    {
        _canvasTutorialFingerTap.SetActive(actived);
    }

    public void UpgradeCardTutorial(bool actived)
    {
        _panelUpgradeCardTutorial.SetActive(actived);
    }

    public void PanelTwoTutorial(bool actived)
    {
        _panelTwoTutorial.SetActive(actived);
    }

    public void PanelThree(bool actived)
    {
        _panelThreeTutorial.SetActive(actived);
    }

    public void OnDrawOut(UnitCard cardOld, UnitCard cardNew)
    {
        _cardLevelUpOld.ChangeOldCard(cardOld);
        _cardLevelUpNew.ChangeDrawCard(cardNew);
    }
}
