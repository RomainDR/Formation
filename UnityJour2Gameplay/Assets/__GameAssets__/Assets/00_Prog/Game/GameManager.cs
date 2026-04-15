using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Transform _playerStartPosition;

    [SerializeField] private List<Collectible> test = new();
    public void ResetPlayer()
    {
        _player.transform.position = _playerStartPosition.position;
        _player.transform.rotation = _playerStartPosition.rotation;
    }

    public Player GetPlayer() => _player;

    // Instance unique accessible partout
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // S'assure qu'il n'y a qu'une seule instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        foreach (Collectible collectible in FindObjectsByType<Collectible>(FindObjectsSortMode.None))
        {
            test.Add(collectible);
        }
    }
}