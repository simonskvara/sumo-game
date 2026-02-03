using System;
using System.Collections;
using skv_toolkit;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Sorry about all those texts, had only one day to do this
/// </summary>
public class SumoManager : MonoBehaviour
{
    public static SumoManager Instance;

    public GameObject player1;
    public GameObject player2;

    private Vector3 _player1StartPosition;
    private Vector3 _player2StartPosition;
    
    private Quaternion _player1StartRotation;
    private Quaternion _player2StartRotation;

    public int player1RoundsWon;
    public int player2RoundsWon;
    private int _currentRound;
    
    [Header("Texts")] 
    public TextMeshProUGUI player1RoundsWonText;
    public TextMeshProUGUI player2RoundsWonText;
    [SerializeField] private GameObject roundWonObject;
    [SerializeField] private TextMeshProUGUI roundWonText;

    [SerializeField] private string gameWonDescription;
    [SerializeField] private GameObject gameWonObject;
    [SerializeField] private TextMeshProUGUI gameWonText;

    private PlayerController[] _controllers;
    private PlayerChargeAttack[] _chargeAttacks;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        UpdateText();
        _player1StartPosition = player1.transform.position;
        _player2StartPosition = player2.transform.position;

        _player1StartRotation = player1.transform.rotation;
        _player2StartRotation = player2.transform.rotation;

        _controllers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        _chargeAttacks = FindObjectsByType<PlayerChargeAttack>(FindObjectsSortMode.None);
    }

    public void ResetPlayers()
    {
        /*player1RoundsWon = 0;
        player2RoundsWon = 0;*/
        
        player1.transform.position = _player1StartPosition;
        player1.transform.rotation = _player1StartRotation;
        
        player2.transform.position = _player2StartPosition;
        player2.transform.rotation = _player2StartRotation;
    }

    public void Player1WinsRound()
    {
        player1RoundsWon++;
        UpdateText();
        
        if (player1RoundsWon >= 3)
        {
            GameWin(true, false);
            return;
        }
        
        StartCoroutine(RoundWon());
        
        roundWonObject.SetActive(true);
        roundWonText.text = "Player 1 won the round";
        
        Debug.Log("Player 1 wins the round");
    }

    public void Player2WinsRound()
    {
        player2RoundsWon++;
        UpdateText();
        
        if (player2RoundsWon >= 3)
        {
            GameWin(false, true);
            return;
        }

        StartCoroutine(RoundWon());
        
        roundWonObject.SetActive(true);
        roundWonText.text = "Player 2 won the round";
        
        Debug.Log("Player 2 wins the round");
    }

    IEnumerator RoundWon()
    {
        DisablePlayers();
        
        yield return new WaitForSeconds(2f);
        ArenaManager.Instance.ResetArena();
        ResetPlayers();
        
        roundWonObject.SetActive(false);
        
        EnablePlayers();
    }

    void UpdateText()
    {
        player1RoundsWonText.text = $"{player1RoundsWon}/3";
        player2RoundsWonText.text = $"{player2RoundsWon}/3";
    }

    void GameWin(bool isPlayer1, bool isPlayer2)
    {
        DisablePlayers();
        
        UpdateText();

        gameWonObject.SetActive(true);
        
        if (isPlayer1)
        {
            string tempString = gameWonDescription.Replace("<playerName>", "Player 1");
            gameWonText.text = tempString;
        }

        if (isPlayer2)
        {
            string tempString = gameWonDescription.Replace("<playerName>", "Player 2");
            gameWonText.text = tempString;
        }
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
    }

    private void DisablePlayers()
    {
        foreach (var controller in _controllers)
        {
            controller.DisableControls();
        }

        foreach (var chargeAttack in _chargeAttacks)
        {
            chargeAttack.CancelChargingAndDashing();
        }
    }

    private void EnablePlayers()
    {
        foreach (var controller in _controllers)
        {
            controller.EnableControls();
        }
    }
}
