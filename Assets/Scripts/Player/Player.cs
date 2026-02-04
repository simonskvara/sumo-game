using System;
using NaughtyAttributes;
using UnityEngine;

public class Player : MonoBehaviour
{
    [BoxGroup("ID")] 
    [SerializeField] 
    private int playerId;
    public int PlayerId => playerId;

    private int _roundsWon = 0;
    public int RoundsWon => _roundsWon;

    private Vector2 _startingPosition;
    private Quaternion _startingRotation;

    private void Awake()
    {
        _startingPosition = gameObject.transform.position;
        _startingRotation = gameObject.transform.rotation;
    }

    public void RoundWon()
    {
        _roundsWon++;
    }

    public void ResetPlayerPositionAndRotation()
    {
        gameObject.transform.position = _startingPosition;
        gameObject.transform.rotation = _startingRotation;
    }

    public Vector2 GetStartingPosition()
    {
        return _startingPosition;
    }

    public Quaternion GetStartingRotation()
    {
        return _startingRotation;
    }
}
