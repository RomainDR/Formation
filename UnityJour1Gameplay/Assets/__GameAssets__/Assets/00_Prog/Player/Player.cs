using System;
using System.Collections;
using __GameAssets__.Assets._00_Prog.UI;
using UnityEngine;

//Permet d'inclure directement un composant sur l'objet lors de l'ajout de ce script
[RequireComponent(typeof(Rigidbody), typeof(TestInputMapping), typeof(PlayerCamera))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpForce = 2f;
    [SerializeField] private int _playerStartLife = 5;
    [SerializeField] private LayerMask _layerCanAttack;
    [SerializeField] private LayerMask _layerEnviro;

    private Rigidbody _rb;
    private TestInputMapping _inputMapping;

    private PlayerCamera _playerCamera;
    private bool _isJumping;
    private bool _showAttackGizmo;
    
    public int CurrentLife { get; private set; }

    private readonly float _radius = 3.0f;

    private void Awake()
    {
        _inputMapping = GetComponent<TestInputMapping>();
        _rb = GetComponent<Rigidbody>();
        _playerCamera = GetComponent<PlayerCamera>();
    }

    public void AddLife(int _amount)
    {
        CurrentLife += _amount;
        UIManager.Instance.SetLifeCounter(CurrentLife);
    }

    public void RemoveLife(int _amount)
    {
        CurrentLife -= _amount;
        UIManager.Instance.SetLifeCounter(CurrentLife);

        if (CurrentLife == 0)
        {
            UIManager.Instance.ShowDeathPanel();
            _inputMapping.DisableInputs();
            _playerCamera.lockCamera = true;
        }
    }

    void Start()
    {
        CurrentLife = _playerStartLife;
        _inputMapping.OnJump += Jump;
        _inputMapping.OnAttack += Attack;
        _inputMapping.RegisterInputs();
        UIManager.Instance.SetLifeCounter(CurrentLife);
    }


    private void Attack()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            _radius,
            _layerCanAttack
        );

        foreach (Collider hit in hits)
        {
            Debug.Log(hit.name);
            Debug.Log(hit.transform.name);
            HealBox healComponent = hit.GetComponent<HealBox>();
            if (healComponent != null) healComponent.AddLife(this);
        }

        StartCoroutine(ShowAttackGizmo());
    }

    //Affiche un gizmo pendant 0.3 secondes pour du débug
    private IEnumerator ShowAttackGizmo()
    {
        _showAttackGizmo = true;
        yield return new WaitForSeconds(0.3f);
        _showAttackGizmo = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 0.6f));

        if (!_showAttackGizmo) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.forward, _radius);
    }

    private void Jump()
    {
        if (_isJumping) return;
        _rb.AddForce(new Vector3(0, _jumpForce, 0), ForceMode.Impulse);
        _isJumping = true;
    }

    private void OnDisable()
    {
        _inputMapping.DisableInputs();
    }

    private void FixedUpdate()
    {
        Vector3 move = new Vector3(_inputMapping.MoveInput.x, 0f, _inputMapping.MoveInput.y);
        if (move == Vector3.zero) return;

        Vector3 worldMove = (_playerCamera.CameraRight * move.x
                             + _playerCamera.CameraForward * move.z).normalized;

        transform.forward = worldMove;

        if (Physics.Raycast(transform.position, worldMove, 0.6f, _layerEnviro))
        {
            worldMove = Vector3.zero;
            Debug.Log("Object detected");
        }

        _rb.MovePosition(_rb.position + worldMove * (_speed * Time.fixedDeltaTime));
    }

    //Pour réactiver le jump si on touche un objet de type "ground"
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if (!other.CompareTag("Ground")) return;
        _isJumping = false;
    }
}