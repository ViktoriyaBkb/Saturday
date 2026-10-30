using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public void OnRestartButtonClicked() => GameManager.Instance.RestartGame();
    public void OnMainMenuButtonClicked() => GameManager.Instance.GoToMainMenu();
    public void OnResumeButtonClicked() => TogglePause();
    public static UIManager Instance { get; private set; }

    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI _killsText;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private TextMeshProUGUI _timerText;

    [Header("End Screens")]
    [SerializeField] private GameObject _endScreenPanel;
    [SerializeField] private TextMeshProUGUI _resultText;

    private bool _isPaused = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.Instance.IsGameOver)
        {
            TogglePause();
        }
    }

    public void UpdateScoreUI(int currentKills)
    {
        if (_killsText != null)
        {
            int total = GameManager.Instance.TotalTargets;
            _killsText.text = $"KILLS: {currentKills} / {total}";
        }
    }

    public void UpdateTimerUI(float timeRemaining)
    {
        if (_timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            _timerText.text = $"{minutes:00}:{seconds:00}";

            _timerText.color = timeRemaining <= 5f ? Color.red : Color.white;
        }
    }

    public void ShowEndScreen(bool isWin)
    {
        if (_endScreenPanel != null) _endScreenPanel.SetActive(true);

        if (_resultText != null)
        {
            if (isWin)
            {
                _resultText.text = "MISSION COMPLETE";
                _resultText.color = Color.green;
            }
            else
            {
                if (GameManager.Instance.TimeRemaining <= 0f)
                {
                    _resultText.text = "TIME OUT";
                    _resultText.color = Color.yellow;
                }
                else
                {
                    _resultText.text = "YOU WERE SPOTTED";
                    _resultText.color = Color.red;
                }
            }
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void TogglePause()
    {
        _isPaused = !_isPaused;
        _pauseMenu.SetActive(_isPaused);

        Time.timeScale = _isPaused ? 0f : 1f;
        Cursor.lockState = _isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = _isPaused;

        if (AudioManager.Instance != null)
        {
            if (_isPaused) AudioManager.Instance.PlayMenuMusic();
            else AudioManager.Instance.PlayGameMusic();           
        }
    }

}