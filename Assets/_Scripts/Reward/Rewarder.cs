using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Rewarder : MonoBehaviour
{
    [SerializeField] private LevelRewardData _levelRewardData;
    [SerializeField] private BattleSystem _battleSystem;
    [SerializeField] private DeckBuilder _deckBuilder;
    [SerializeField] private CoinsWallet _coinsWallet;
    [SerializeField] private CardRewardPanel _cardRewardPanel;

    private Card[] _rewardCards;

    public int ReceivedCoins { get; private set; }
    public Card[] RewardCards => _rewardCards;

    private void OnValidate()
    {
        if (_battleSystem == null)
            _battleSystem = FindObjectOfType<BattleSystem>();

        if (_deckBuilder == null)
            _deckBuilder = FindObjectOfType<DeckBuilder>();

        if (_cardRewardPanel == null)
            _cardRewardPanel = FindObjectOfType<CardRewardPanel>(true);

        if (_coinsWallet == null)
            _coinsWallet = FindObjectOfType<CoinsWallet>();
    }

    private void OnEnable()
    {
        _battleSystem.Win += OnFinished;
        _battleSystem.Lose += OnFailed;
    }

    private void OnDisable()
    {
        _battleSystem.Win -= OnFinished;
        _battleSystem.Lose -= OnFailed;
    }

    private void OnFinished()
    {
        RewardData reward = _levelRewardData.GetAward(GetCurretLevel());
        List<Card> cards = new List<Card>();

        AwardCards(reward, cards);

        ReceivedCoins = reward.Coins;

        _cardRewardPanel.ShowCards(cards.ToArray());
        _coinsWallet.Add(ReceivedCoins, 0);
    }

    private void AwardCards(RewardData reward, List<Card> cards)
    {
        Debug.Log(reward);
        if (reward.Card != null)
        {
            Card card = _deckBuilder.GetCard(reward.Card.Name);
            SaveAward(reward, cards, card);
        }
    }

    private void OnFailed()
    {
        RewardData reward = _levelRewardData.GetAward(GetCurretLevel());

        ReceivedCoins = reward.Coins / 2;
        _coinsWallet.Add(ReceivedCoins, 0);
    }

    private void SaveAward(RewardData reward, List<Card> cards, Card card)
    {
        card.CardSave.SetAvailable(true);
        card.CardSave.SetDeck(DeckType.Combat);
        card.CardSave.Add(reward.Amount);

        card.Save();
        cards.Add(card);
    }

    private int GetCurretLevel() => SceneManager.GetActiveScene().buildIndex - 2;
}
