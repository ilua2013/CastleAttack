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
        _battleSystem.Lose += OnFailed;
    }

    private void OnDisable()
    {
        _battleSystem.Lose -= OnFailed;
    }

    public void Init()
    {
        RewardData reward = _levelRewardData.GetAward(Saves.SelectedLevel);

        ReceivedCoins = reward.Coins;
        _coinsWallet.Add(ReceivedCoins, 0);

        if (Saves.HasKey(SaveController.Params.CompletedLevel))
            if (Saves.GetInt(SaveController.Params.CompletedLevel) >= Saves.SelectedLevel)
                return;

        AwardCards(reward);
    }

    private void AwardCards(RewardData reward)
    {
        if (reward.Card != null)
        {
            List<Card> cards = new List<Card>();

            Card card = _deckBuilder.GetCard(reward.Card.Name);
            SaveAward(reward, cards, card);

            _cardRewardPanel.ShowCards(cards.ToArray());
        }
    }

    private void OnFailed()
    {
        RewardData reward = _levelRewardData.GetAward(Saves.SelectedLevel);

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
}
