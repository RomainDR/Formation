using UnityEngine;

public class PlateformCollision : GPEBase
{
    private Rigidbody _playerRb;
    private Vector3 _lastPlatformPosition;
    private bool _playerOnPlatform;

    private void FixedUpdate()
    {
        if (!_playerOnPlatform || _playerRb == null) return;

        Vector3 delta = transform.position - _lastPlatformPosition;
        _playerRb.MovePosition(_playerRb.position + delta);
        _lastPlatformPosition = transform.position;
    }

    protected override void OnPlayerEnter(Collider player)
    {
        base.OnPlayerEnter(player);
        _playerRb = player.GetComponent<Rigidbody>();
        _lastPlatformPosition = transform.position;
        _playerOnPlatform = true;
    }

    protected override void OnPlayerExit(Collider player)
    {
        base.OnPlayerExit(player);
        _playerRb = null;
        _playerOnPlatform = false;
    }
}