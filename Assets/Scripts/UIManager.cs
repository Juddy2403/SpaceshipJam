
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> _scoreText;
    [SerializeField] private GameObject _endMenu;
    [SerializeField] private TextMeshProUGUI _endMenuText;
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
        if(ScoreManager.Instance) ScoreManager.Instance.OnScoreChanged -= OnScoreUpdate;
        if(_satteliteManager) _satteliteManager.OnGameFinish -= OnGameEnd;
    }
    private void OnGameEnd()
    {
        var scores = ScoreManager.Instance.PlayerScores;
        int winner;
        if (scores[0] > scores[1]) winner = 0; else winner = 1;
        _endMenuText.text = $"Player {ScoreManager.Instance.PlayerColorNames[winner]} Wins!";
        _endMenu.SetActive(true);
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
