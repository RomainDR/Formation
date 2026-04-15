using System;
using UnityEngine;

// [ExecuteAlways] permet de voir les changements de variables en temps réel
// dans l'éditeur, sans avoir à lancer le jeu.
[ExecuteAlways]
[RequireComponent(typeof(TestInputMapping))]
public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [SerializeField] [Range(-89f, 89f)] private float _pitch = 20f; // rotation verticale
    [SerializeField] [Range(0f, 360f)] private float _yaw = 0f; // rotation horizontale
    [SerializeField] private float _distance = 5f;
    [SerializeField] private float _sensitivity = 2f;
    [SerializeField] private LayerMask _layerCanHit;


    private TestInputMapping _inputMapping;
    public bool lockCamera { private get; set; }

    // Expose les directions horizontales de la caméra pour Player
    // Player interroge PlayerCamera - il ne parle jamais à Camera directement

    public Vector3 CameraForward
    {
        get
        {
            var forward = _camera.transform.forward;
            forward.y = 0f;
            return forward.normalized;
        }
    }

    public Vector3 CameraRight
    {
        get
        {
            var right = _camera.transform.right;
            right.y = 0f;
            return right.normalized;
        }
    }

    private void Awake()
    {
        _inputMapping = GetComponent<TestInputMapping>();
    }

    private void LateUpdate()
    {
        if (!_camera || lockCamera) return;

        // On ne lit les inputs qu'en mode jeu
        if (Application.isPlaying && _inputMapping != null)
        {
            _yaw += _inputMapping.MoveCamera.x * _sensitivity;
            _pitch -= _inputMapping.MoveCamera.y * _sensitivity;
            _pitch = Mathf.Clamp(_pitch, -89f, 89f);
            _yaw %= 360f;
        }

        // Calcul et application de la position de la caméra
        _camera.transform.position = GetClampedCameraPosition();

        // La caméra regarde toujours le joueur (le parent = Player)
        _camera.transform.LookAt(transform.position + Vector3.up * 1f);
    }

    // Principe DRY : la logique du SpringArm est écrite une seule fois
    // et réutilisée dans LateUpdate et OnDrawGizmosSelected
    private Vector3 GetClampedCameraPosition()
    {
        var rotation = Quaternion.Euler(_pitch, _yaw, 0);
        var desiredPosition = transform.position + rotation * new Vector3(0, 0, -_distance);

        var dir = desiredPosition - transform.position;
        if (Physics.Raycast(transform.position, dir.normalized, out var hit, dir.magnitude, _layerCanHit))
            // Un mur est détecté : on ramène la caméra juste devant lui
            return hit.point - dir.normalized * 0.1f;

        return desiredPosition;
    }

    private void OnDrawGizmosSelected()
    {
        var desiredPosition = GetClampedCameraPosition();
        var idealPosition = transform.position +
                            Quaternion.Euler(_pitch, _yaw, 0) * new Vector3(0, 0, -_distance);

        // Jaune si la caméra est bloquée par un mur, rouge sinon
        Gizmos.color = desiredPosition != idealPosition ? Color.yellow : Color.red;

        Gizmos.DrawLine(transform.position, desiredPosition);
        Gizmos.DrawSphere(desiredPosition, 0.1f);
    }
}