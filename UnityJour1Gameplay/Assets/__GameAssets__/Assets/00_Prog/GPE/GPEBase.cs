using UnityEngine;

public abstract class GPEBase : MonoBehaviour
{
    protected bool _isOneShot = false;
    private bool _hasBeenTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (_isOneShot && _hasBeenTriggered) return;

        _hasBeenTriggered = true;

        OnPlayerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        OnPlayerExit(other);
    }

    protected virtual void OnPlayerEnter(Collider player) { }

    protected virtual void OnPlayerExit(Collider player) { }
}
