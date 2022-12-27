using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDimmer : MonoBehaviour
{
    private bool _isInited;

    private List<Image> _images = new List<Image>();
    private List<TMP_Text> _texts = new List<TMP_Text>();

    public void Init(Card card)
    {
        _isInited = true;

        foreach (Image image in card.gameObject.GetComponentsInChildren<Image>(true))
            _images.Add(image);

        foreach (TMP_Text text in card.gameObject.GetComponentsInChildren<TMP_Text>(true))
            _texts.Add(text);
    }

    public void Activate()
    {
        if (!_isInited)
            return;

        foreach (Image image in _images)
            image.color = new Color32(138, 138, 138, 255);

        foreach (TMP_Text text in _texts)
            text.color = new Color32(138, 138, 138, 255);
    }

    public void Inactivate()
    {
        if (!_isInited)
            return;

        foreach (Image image in _images)
            image.color = new Color32(255, 255, 255, 255);

        foreach (TMP_Text text in _texts)
            text.color = new Color32(255, 255, 255, 255);
    }
}
