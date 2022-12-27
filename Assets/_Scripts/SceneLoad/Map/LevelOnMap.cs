using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelOnMap : MonoBehaviour
{
    public int IndexScene;
    [Header("Sprite")]
    [SerializeField] private Sprite _spriteOpenLevel;
    [SerializeField] private Sprite _spriteCloseLevel;
    [SerializeField] private Sprite _spriteCurrentLevel;
    [Header("Stars")]
    [SerializeField] private Sprite _starsYellow;
    [SerializeField] private Sprite _starsGray;
    [SerializeField] private Image[] _stars;

    private Button _button;

    public event Action<LevelOnMap> Clicked;

    private void OnValidate()
    {
        GetComponentInChildren<Text>(true).text = IndexScene.ToString();

        //if (_stars.Length != 3)
        //    _stars = new Image[3];

        //foreach (var item in GetComponentsInChildren<RectTransform>())
        //{
        //    if (item.name == "Stars")
        //        _stars = item.GetComponentsInChildren<Image>();
        //}
    }

    private void Awake()
    {
        _button = GetComponentInChildren<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(InvokeClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(InvokeClicked);
    }

    public void CloseLevel()
    {

    }

    public void OpenLevel(int starsCount)
    {

    }

    public void ShowCurrentLevel()
    {

    }

    private void InvokeClicked()
    {
        Clicked?.Invoke(this);
    }

    private void EnableStars(int count)
    {
        foreach (var item in _stars)
            item.sprite = _starsGray;

        for (int i = 0; i < count; i++)
            _stars[i].sprite = _starsYellow;
    }

    private void DisableStars()
    {
        foreach (var item in _stars)
        {
            item.gameObject.SetActive(false);
        }
    }
}
