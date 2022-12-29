using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Card))]
public class CardAnonimusView : MonoBehaviour
{
    [Header("Description")]
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _AnonymousDescription;
    [Header("Name")]
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _anonymousName;
    [Header("Icon")]
    [SerializeField] private Image _icon;
    [SerializeField] private GameObject _anonymousIcon;

    private Card _card;

    private void Awake()
    {
        _card = GetComponent<Card>();
    }

    private void Start()
    {
        if (_card.CardSave.Amount < 1)
            ReskinToAnonymous();
        else
            ReskinToInitial();
    }

    private void ReskinToAnonymous()
    {
        _description.enabled = false;
        _AnonymousDescription.enabled = true;

        _name.enabled = false;
        _anonymousName.enabled = true;

        _icon.enabled = false;
        _anonymousIcon.SetActive(true);
    }

    private void ReskinToInitial()
    {
        _description.enabled = true;
        _AnonymousDescription.enabled = false;

        _name.enabled = true;
        _anonymousName.enabled = false;

        _icon.enabled = true;
        _anonymousIcon.SetActive(false);

    }
}
