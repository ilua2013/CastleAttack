using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    public const int TutorialIndex = 1;
    public const int MenuIndex = 2;
    public const int FirstLevelIndex = 3;
    public List<LevelCellSize> IndexSceneCellSize = new List<LevelCellSize>()
    {
        new LevelCellSize(0, 3, new CellSize(5,3)),
        new LevelCellSize(3, 4, new CellSize(6,4)),
        new LevelCellSize(8, 5, new CellSize(6,5)),
        new LevelCellSize(11, 6, new CellSize(7,5)),
        new LevelCellSize(15, 7, new CellSize(8,6)),
    };

    [SerializeField] private LevelEnemiesData _levelEnemiesData;
    [SerializeField] private Image _background;
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private TextMeshProUGUI _loadingPercent;

    private AsyncOperation _load;

    private void OnValidate()
    {
        if (_levelEnemiesData == null)
            _levelEnemiesData = Resources.Load("Configs/LevelEnemies") as LevelEnemiesData;

        _background.enabled = false;
        _progressSlider.gameObject.SetActive(false);
    }

    public void RestartLevel()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenu()
    {
        LoadScene(MenuIndex);
    }

    public void LoadTutorial()
    {
        LoadScene(TutorialIndex);
    }

    public void LoadNextLevel()
    {
        int levelIndex = TutorialIndex;

        if (Saves.HasKey(SaveController.Params.IsTutorialCompleted))
            if (Saves.GetBool(SaveController.Params.IsTutorialCompleted))
                levelIndex = MenuIndex;

        //if (Saves.HasKey(SaveController.Params.Level))
        //    if (Saves.GetInt(SaveController.Params.Level) + 1 < SceneManager.sceneCountInBuildSettings)
        //        levelIndex = Saves.GetInt(SaveController.Params.Level) + 1;

        LoadScene(levelIndex);
    }

    public void LoadLevel(int level)
    {
        print("LoadLevel - " + level);
        for (int i = IndexSceneCellSize.Count - 1; i >= 0; i--)
        {
            if(level >= IndexSceneCellSize[i].Level)
            {
                print("Load Scene with index - " + IndexSceneCellSize[i].Index);
                LoadScene(IndexSceneCellSize[i].Index);
                break;
            }
        }
    }

    private void LoadScene(int index)
    {
        _background.enabled = true;
        _progressSlider.gameObject.SetActive(true);

        StartCoroutine(AsyncLoad(index));
    }

    private IEnumerator AsyncLoad(int sceneIndex)
    {
        if (_load != null)
            yield break; 

        _load = SceneManager.LoadSceneAsync(sceneIndex);

        _load.allowSceneActivation = false;

        while (_progressSlider.value < 1)
        {
            _progressSlider.value += Time.timeScale * 0.05f;
            _loadingPercent.text = Mathf.RoundToInt(_progressSlider.value * 100) + "%";
            yield return null;
        }

        _load.allowSceneActivation = true;

        _load = null;
    }
}

public class LevelCellSize
{
    public int Level;
    public int Index;
    public CellSize CellSize;

    public LevelCellSize(int levelNumber, int index, CellSize cellSize)
    {
        Level = levelNumber;
        Index = index;
        CellSize = cellSize;
    }
}
