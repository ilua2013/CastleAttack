using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private float _delayFirstStep = 1.5f;
    [SerializeField] private UnitFriend _wizzard;
    [SerializeField] private Button _buttonStartFight;
    [SerializeField] private Button _buttonSkipStep;
    [SerializeField] private CardsHand _cardsHand;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private SpellsRecorder _spellsRecorder;

    private List<UnitFriend> _unitFriend = new List<UnitFriend>();
    private List<UnitEnemy> _unitEnemy = new List<UnitEnemy>();
    private List<Spell> _spells = new List<Spell>();
    private IUnit _nextStepUnit;
    private bool _doStep = true;
    private bool _friendFinishStep;
    private bool _enemyFinishStep;
    private bool _spellFinishStep;
    private bool _doStepFriend;
    private bool _doStepSpell;
    private bool _doStepEnemy;

    public int CountEnemy => _unitEnemy.Count;
    public UnitFriend Wizzard => _wizzard;
    public IUnit NextStepUnit => _nextStepUnit;
    public EnemySpawner EnemySpawner => _enemySpawner;
    public List<UnitEnemy> UnitsEnemy => _unitEnemy;
    public List<UnitFriend> UnitsFriend => _unitFriend;

    public event Action StepFinished;
    public event Action SkipedStep;
    public event Action BattleStarted;
    public event Action RemovedDie;
    public event Action TutorialStopedUnit;
    public event Action Lose;
    public event Action Win;
    public event Action Revived;

    private void OnValidate()
    {
        if (_cardsHand == null)
            _cardsHand = FindObjectOfType<CardsHand>();

        if (_spellsRecorder == null)
            _spellsRecorder = FindObjectOfType<SpellsRecorder>();

        if (_buttonStartFight == null)
            _buttonStartFight = FindObjectOfType<StartFightButton>().Button;

        if(_buttonSkipStep == null)
            _buttonSkipStep = FindObjectOfType<StartFightButton>().ButtonSkip;

        foreach (var item in FindObjectsOfType<UnitFriend>())
        {
            if (item.Fighter.FighterType == FighterType.MainWizzard)
            {
                _wizzard = item;
                break;
            }
        }
    }

    private void OnEnable()
    {
        _buttonStartFight.onClick.AddListener(StartBattle);
        _buttonSkipStep.onClick.AddListener(StartSkipStep);
        _cardsHand.Spawned += AddUnit;
        _spellsRecorder.WasSpellCast += AddSpell;
        _enemySpawner.Spawned_get += AddUnit;
        _wizzard.Fighter.Died += InvokeLose;
        _wizzard.Fighter.ReadyToDie += InvokeLose;
        _wizzard.Fighter.Revived += InvokeRevived;
    }

    private void OnDisable()
    {
        _buttonStartFight.onClick.RemoveListener(StartBattle);
        _buttonSkipStep.onClick.RemoveListener(StartSkipStep);
        _cardsHand.Spawned -= AddUnit;
        _spellsRecorder.WasSpellCast -= AddSpell;
        _enemySpawner.Spawned_get -= AddUnit;
        _wizzard.Fighter.Died -= InvokeLose;
        _wizzard.Fighter.ReadyToDie -= InvokeLose;
        _wizzard.Fighter.Revived -= InvokeRevived;

        foreach (var unit in _unitFriend)
            unit.Mover.ReachedHigherCell -= TutorialStopUnit;
    }

    public void ReturnToHandFriend()
    {        
        for (int i = _unitFriend.Count - 1; i > -1; i--)
            _unitFriend[i].ReturnToHand();
    }

    private void TutorialStopUnit()
    {       
        TutorialStopedUnit?.Invoke();          
    }

    public void StopDoStep()
    {
        _doStep = false;
    }

    public void ContinueDoStep()
    {
        _doStep = true;
    }

    private void AddSpell(Spell spell)
    {
        if (spell != null && _spells.Contains(spell) == false)
        {
            _spells.Add(spell);
            spell.Dispelled += RemoveSpell;
        }
    }

    private void AddUnit(UnitFriend unitFriend)
    {
        if (unitFriend != null && _unitFriend.Contains(unitFriend) == false)
        {
            _unitFriend.Add(unitFriend);            
            unitFriend.Fighter.Died_get += RemoveUnit;
            unitFriend.FinishedStep += CheckFinishStepFriend;
            unitFriend.LevelUpped += AddUnit;
        }
    }

    private void AddUnit(UnitEnemy unitEnemy)
    {
        if (_unitEnemy.Contains(unitEnemy) == false)
        {
            _unitEnemy.Add(unitEnemy);
            unitEnemy.Fighter.Died_get += RemoveUnit;
            unitEnemy.FinishedStep += CheckFinishStepEnemy;
        }
    }

    private void StartBattle()
    {
        CalculateFirstStep();
        _doStep = true;

        BattleStarted?.Invoke();
        StartCoroutine(Battle());
    }

    private void StartSkipStep() => SkipStep();

    private void SkipStep(bool invokeFinishStep = true)
    {
        CalculateFirstStep();
        SkipedStep?.Invoke();
        BattleStarted?.Invoke();
        _doStep = true;
        StartCoroutine(BattleEnemyOnly(invokeFinishStep));
    }

    private IEnumerator Battle()
    {
        while (CheckHaveStep() && _doStep == true)
        {
            _friendFinishStep = false;
            _enemyFinishStep = false;
            _spellFinishStep = false;

            yield return new WaitForSeconds(0.2f);

            StartCoroutine(DoStepSpell());

            while (_doStepSpell)
                yield return null;

            while (_spellFinishStep == false)
            {
                yield return null;
                CheckFinishStepSpells();
            }

            yield return new WaitForSeconds(0.2f);

            StartCoroutine(DoStepFriend());

            while (_doStepFriend)
                yield return null;

            while (_friendFinishStep == false)
            {
                yield return null;
                CheckFinishStepFriend();
            }

            yield return new WaitForSeconds(0.1f);

            StartCoroutine(DoStepEnemy());

            while (_doStepEnemy)
                yield return null;

            while (_enemyFinishStep == false)
            {
                yield return null;
                CheckFinishStepEnemy();
            }
        }

        StepFinished?.Invoke();

        foreach (var item in _unitFriend)
            item.UpdateStep();

        foreach (var item in _unitEnemy)
            item.UpdateStep();
    }

    private IEnumerator BattleEnemyOnly(bool invokeFinishStep = true)
    {
        while (CheckHaveStep(true,false) && _doStep == true)
        {
            _enemyFinishStep = false;

            StartCoroutine(DoStepEnemy());

            while (_doStepEnemy)
                yield return null;

            while (_enemyFinishStep == false)
            {
                yield return null;
                CheckFinishStepEnemy();
            }
        }

        if (invokeFinishStep)
            StepFinished?.Invoke();

        foreach (var item in _unitEnemy)
            item.UpdateStep();
    }

    private IEnumerator DoStepSpell()
    {
        _doStepSpell = true;

        for (int i = _spells.Count - 1; i > -1; i--) // определяет верный порядок действий
        {
            if (i > _spells.Count - 1)
                continue;

            _spells[i].DoStep();

            yield return new WaitForSeconds(0.6f);

            while (_spells.Count > i && _spells[i].DoingStep == true)
                yield return null;
        }

        _doStepSpell = false;
    }

    private IEnumerator DoStepFriend()
    {
        _doStepFriend = true;

        foreach (var unitFriend in _unitFriend)        
            unitFriend.UnitsStartedWalking();
        
        foreach (var unitEnemy in _unitEnemy)        
            unitEnemy.UnitsStartedWalking();
        

        for (int i = _unitFriend.Count - 1; i > -1; i--) // определяет верный порядок действий
        {
            while (_doStep == false)
                yield return null;

            _unitFriend[i].DoStep();

            if (i > 0)
            {
                _nextStepUnit = _unitFriend[i - 1];
            }

            yield return new WaitForSeconds(0.6f);

            while (_unitFriend.Count > i && _unitFriend[i].DoingStep == true)
                yield return null;
        }

        _doStepFriend = false;
    }

    private IEnumerator DoStepEnemy()
    {
        _doStepEnemy = true;
        for (int i = _unitEnemy.Count - 1; i > -1; i--)
        {
            if (i > _unitEnemy.Count - 1)
                continue;

            while (_doStep == false)
                yield return null;

            _unitEnemy[i].DoStep();

            if(i > 0)
                _nextStepUnit = _unitEnemy[i - 1];

            yield return new WaitForSeconds(0.6f);

            while (_unitEnemy.Count > i && _unitEnemy[i].DoingStep == true)
                yield return null;

            if (_wizzard.Fighter.IsDamaged && _doStep)
            {
                _wizzard.Fighter.IsDamaged = false;
                _wizzard.DoStep(_unitEnemy[i]);

                while (_wizzard.DoingStep == true)
                    yield return null;
            }
        }     

        _doStepEnemy = false;
    }

    private bool CheckHaveStep(bool enemy = true, bool friend = true)
    {
        if (friend)
        {
            foreach (var item in _unitFriend)
                if (item.CurrentStep > 0)
                    return true;
        }

        if (enemy)
        {
            foreach (var item in _unitEnemy)
                if (item.CurrentStep > 0)
                    return true;
        }

        return false;
    }

    private void CheckFinishStepSpells()
    {
        foreach (var item in _spells)
        {
            if (item.DoingStep == true)
                return;
        }

        _spellFinishStep = true;
    }

    private void CheckFinishStepFriend()
    {
        foreach (var item in _unitFriend)
        {
            if (item.DoingStep == true)
                return;
        }

        _friendFinishStep = true;
    }

    private void CheckFinishStepEnemy()
    {
        foreach (var item in _unitEnemy)
            if (item.DoingStep == true)
                return;

        _enemyFinishStep = true;
    }

    private void CalculateFirstStep()
    {
        List<UnitFriend> units = new List<UnitFriend>();
        int index = 0;
        
        while (units.Count != _unitFriend.Count)
        {
            foreach (var item in _unitFriend)
                if (item.Mover.CurrentCell.Number == index)
                    units.Add(item);

            index++;
        }

        _unitFriend = units;

        foreach (var unit in _unitFriend)
        {
            unit.Mover.ReachedTutorialHigherCell += TutorialStopUnit;
        }

        index = 20;

        List<UnitEnemy> unitsEnemy = new List<UnitEnemy>();

        while (unitsEnemy.Count != _unitEnemy.Count)
        {
            foreach (var item in _unitEnemy)
                if (item.Mover.CurrentCell.Number == index)
                    unitsEnemy.Add(item);

            index--;
        }

        _unitEnemy = unitsEnemy;
    }

    private void RemoveSpell(Spell spell)
    {
        spell.Dispelled -= RemoveSpell;
        _spells.Remove(spell);
    }

    private void RemoveUnit(Fighter fighter)
    {
        fighter.Died_get -= RemoveUnit;

        if (fighter.Unit is UnitFriend unitFriend)
            _unitFriend.Remove(unitFriend);
        else if (fighter.Unit is UnitEnemy unitEnemy)
        {
            _unitEnemy.Remove(unitEnemy);

            if (_unitEnemy.Count == 0 && _enemySpawner.HaveWave == false)
            {
                Win?.Invoke();
                SaveCastle.AttackCastle = true;

                StopDoStep();
            }
        }
    }

    private void InvokeLose()
    {
        Lose?.Invoke();
    }

    private void InvokeRevived()
    {
        Revived?.Invoke();
    }
}
