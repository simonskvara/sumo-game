using System;
using UnityEngine;

public class SumoManager : MonoBehaviour
{
    public static SumoManager Instance;

    public GameObject player1;
    public GameObject player2;

    private Transform _player1StartTransform;
    private Transform _player2StartTransform;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _player1StartTransform = player1.transform;
        _player2StartTransform = player2.transform;
    }

    public void ResetPlayers()
    {
        player1.transform.position = _player1StartTransform.position;
        player1.transform.rotation = _player1StartTransform.rotation;
        
        player2.transform.position = _player2StartTransform.position;
        player2.transform.rotation = _player2StartTransform.rotation;
    }
}
