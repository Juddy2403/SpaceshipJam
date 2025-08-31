
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> _scoreText;
    [SerializeField] private GameObject _endMenu;
    [SerializeField] private TextMeshProUGUI _endMenuText;
    [SerializeField] private Button _restartButton;
    private SatteliteManager _satteliteManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _satteliteManager = FindFirstObjectByType<SatteliteManager>();
        ScoreManager.Instance.OnScoreChanged += OnScoreUpdate;
        _satteliteManager.OnGameFinish += OnGameEnd;
    }
    private void OnDisable()
    {
        if (ScoreManager.Instance) ScoreManager.Instance.OnScoreChanged -= OnScoreUpdate;
        if (_satteliteManager) _satteliteManager.OnGameFinish -= OnGameEnd;
    }
    private void OnGameEnd()
    {
        _restartButton.Select();
        var scores = ScoreManager.Instance.PlayerScores;
        int winner;
        if (scores[0] == scores[1]) _endMenuText.text = $"It´s a tie!";
        else
        {
            if (scores[0] > scores[1]) winner = 0; else winner = 1;
            _endMenuText.text = $"Player {ScoreManager.Instance.PlayerColorNames[winner]} Wins!";
        }
        _endMenu.SetActive(true);
        ScoreManager.Instance.ResetScores();

    }
    void OnScoreUpdate(int playerNumber, int newScore)
    {
        _scoreText[playerNumber].text = newScore.ToString();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
