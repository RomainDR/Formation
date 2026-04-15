using System;
using UnityEngine;

public class MovablePlateform : GPEBase
{
    [SerializeField] private float _seconds = 5.0f;
    [SerializeField] private Transform _startPositionActor;
    [SerializeField] private Transform _endPositionActor;

    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private bool hasReachEnd;
    private float elapsedSeconds;

    private void Start()
    {
        _startPosition = _startPositionActor.transform.position;
        _endPosition = _endPositionActor.transform.position;
        transform.position = _startPosition;
    }

    private void Update()
    {
        elapsedSeconds += Time.deltaTime;
        if (elapsedSeconds >= _seconds)
        {
            elapsedSeconds = 0;
            hasReachEnd = !hasReachEnd;
        }

        var newPosition = !hasReachEnd
            ? Vector3.Lerp(_startPosition, _endPosition, elapsedSeconds / _seconds)
            : Vector3.Lerp(_endPosition, _startPosition, elapsedSeconds / _seconds);

        transform.position = newPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.forestGreen;
        Gizmos.DrawWireCube(_startPositionActor.position, Vector3.one);
        Gizmos.DrawWireCube(_endPositionActor.position, Vector3.one);
        Gizmos.DrawLine(_startPositionActor.position, _endPositionActor.position);
    }
}