using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    public void GoToMainMenu() => SceneManager.LoadScene("MainMenu");
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public int TotalTargets = 0;
    public int DestroyedTargets = 0;
    public bool IsGameOver = false;

    [Header("Timer Settings")]
    [SerializeField] private float _roundTime = 30f; // Время на раунд в секундах
    public float TimeRemaining { get; private set; }

    private List<TargetHealth> _allTargets = new List<TargetHealth>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Time.timeScale = 1f;
        TimeRemaining = _roundTime;
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        TotalTargets = targets.Length;

        foreach (var t in targets)
        {
            if (t.TryGetComponent(out TargetHealth health))
                _allTargets.Add(health);
        }


        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateScoreUI(0);
            UIManager.Instance.UpdateTimerUI(TimeRemaining);
        }
    }

    private void Update()
    {
        if (IsGameOver) return;
        TimeRemaining -= Time.deltaTime;

        if (UIManager.Instance != null)
            UIManager.Instance.UpdateTimerUI(TimeRemaining);

        if (TimeRemaining <= 0f)
        {
            TimeRemaining = 0f;
            if (UIManager.Instance != null) UIManager.Instance.UpdateTimerUI(0f);

            EndRound(false);
        }
    }

    public void OnTargetKilled(bool destroyedProperly)
    {
        if (IsGameOver) return;

        if (destroyedProperly)
        {
            DestroyedTargets++;
            if (UIManager.Instance != null)
                UIManager.Instance.UpdateScoreUI(DestroyedTargets);

            if (DestroyedTargets >= TotalTargets)
            {
                EndRound(true); // Win
            }
        }
        else
        {
            EndRound(false); // Lose
        }
    }

    public void EndRound(bool isWin)
    {
        if (IsGameOver) return;
        IsGameOver = true;

        Time.timeScale = 0f; // stop game

        if (UIManager.Instance != null)
            UIManager.Instance.ShowEndScreen(isWin);
    }

}