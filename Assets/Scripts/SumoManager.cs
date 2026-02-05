using System;
using System.Collections;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using UnityEngine;


/// <summary>
/// Sorry about all those texts, had only one day to do this
/// </summary>
/// TODO: Maybe make a UI manager to handle all the text objects
public class SumoManager : MonoBehaviour
{
    public static SumoManager Instance;

    private int _currentRound;

    [BoxGroup("Configuration")] 
    [SerializeField]
    private int roundsNeededToWin = 3;
    
    [BoxGroup("References")]
    [SerializeField]
    private TextMeshProUGUI player1RoundsWonText;
    [BoxGroup("References")]
    [SerializeField]
    private TextMeshProUGUI player2RoundsWonText;
    [BoxGroup("References")]
    [SerializeField] 
    private GameObject roundWonObject;
    [BoxGroup("References")]
    [SerializeField] 
    private TextMeshProUGUI roundWonText;
    [BoxGroup("References")]
    [SerializeField] 
    private GameObject gameWonObject;
    [BoxGroup("References")]
    [SerializeField] 
    private TextMeshProUGUI gameWonText;

    private PlayerController[] _controllers;
    private PlayerChargeAttack[] _chargeAttacks;
    
    private Player player1;
    private Player player2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        _controllers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        _chargeAttacks = FindObjectsByType<PlayerChargeAttack>(FindObjectsSortMode.None);

        player1 = FindObjectsByType<Player>(FindObjectsSortMode.None).FirstOrDefault(p => p.PlayerId == 1);
        player2 = FindObjectsByType<Player>(FindObjectsSortMode.None).FirstOrDefault(p => p.PlayerId == 2);
    }

    private void Start()
    {
        UpdateText();
    }

    public void ResetPlayers()
    {
        player1.ResetPlayerPositionAndRotation();
        player2.ResetPlayerPositionAndRotation();
        
        EnablePlayers();
    }

    public void RoundEnd(Player losingPlayer)
    {
        Player winningPlayer = losingPlayer == player1 ? player2 : player1;
        
        winningPlayer.RoundWon();
        UpdateText();

        if (winningPlayer.RoundsWon >= roundsNeededToWin)
        {
            GameWin(winningPlayer);
            return;
        }

        StartCoroutine(RoundWonCoroutine(winningPlayer));
    }

    private IEnumerator RoundWonCoroutine(Player player)
    {
        DisablePlayers();
        
        roundWonObject.SetActive(true);
        roundWonText.text = $"Player {player.PlayerId} won the round";
        
        yield return new WaitForSeconds(2f);
        
        ArenaManager.Instance.ResetArena();
        ResetPlayers();
        
        roundWonObject.SetActive(false);
    }

    private void UpdateText()
    {
        player1RoundsWonText.text = $"{player1.RoundsWon}/{roundsNeededToWin}";
        player2RoundsWonText.text = $"{player2.RoundsWon}/{roundsNeededToWin}";
    }

    private void GameWin(Player winningPlayer)
    {
        DisablePlayers();
        UpdateText();
        
        gameWonObject.SetActive(true);
        gameWonText.text = $"Player {winningPlayer.PlayerId} is the superior Sumo wrestler";
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void DisablePlayers()
    {
        foreach (var controller in _controllers)
        {
            controller.DisableControls();
        }
        
        foreach (var charge in _chargeAttacks)
        {
            charge.ResetChargingAndDashing();
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
