using System;
using System.Collections;
using __GameAssets__.Assets._00_Prog.UI;
using UnityEngine;

//Permet d'inclure directement un composant sur l'objet lors de l'ajout de ce script
[RequireComponent(typeof(Rigidbody), typeof(TestInputMapping), typeof(PlayerCamera))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _baseSpeed = 1.0f;
    [SerializeField] private float _sprintMultiplier = 2.0f;
    [SerializeField] private float _jumpForce = 2f;
    [SerializeField] private int _playerStartLife = 5;
    [SerializeField] private LayerMask _layerCanAttack;
    [SerializeField] private LayerMask _layerEnviro;

    private Rigidbody _rb;
    private TestInputMapping _inputMapping;
    private PlayerAnimation _playerAnimation;
    private PlayerAnimationEffect _playerAnimationEffect;

    private PlayerCamera _playerCamera;
    private bool _isJumping;
    private bool _showAttackGizmo;
    private bool _isSprinting;
    private float _speed;

    public int CurrentLife { get; private set; }

    private readonly float _radius = 3.0f;

    public int GetStartLife() => _playerStartLife;
   
    private void Awake()
    {
        _inputMapping = GetComponent<TestInputMapping>();
        _rb = GetComponent<Rigidbody>();
        _playerCamera = GetComponent<PlayerCamera>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        _playerAnimationEffect = GetComponent<PlayerAnimationEffect>();
        _speed = _baseSpeed;
    }
    
    public void SetLife(int life) => CurrentLife = life;

    private void Die()
    {
        UIManager.Instance.ShowDeathPanel();
        _inputMapping.DisableInputs();
        _playerCamera.lockCamera = true;
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

        //Mettre l'ancienne logique d'affichage de l'interface dans la méthode EndDied, et juste jouer l'animation ici
        if (CurrentLife == 0)
        {
            _playerAnimation.Died();
            return;
        }
        
        //Changement de logique ici aussi, en supprimant le reset après être touché pour uniquement jouer l'animation et pas reset le joueur
        GameManager.Instance.ResetPlayer();
    }

    private void Start()
    {
        CurrentLife = _playerStartLife;
        _inputMapping.OnJump += Jump;
        _inputMapping.OnAttack += Attack;
        _inputMapping.OnSprint += Sprint;
        _playerAnimation.OnDied += Die;
        _playerAnimation.OnFootLeft += PlaySmokeEffect;
        _playerAnimation.OnFootRight += PlaySmokeEffect;
        _inputMapping.RegisterInputs();
        UIManager.Instance?.SetLifeCounter(CurrentLife);
    }

    private void PlaySmokeEffect()
    {
        _playerAnimationEffect.PlaySmoke();
    }

    private void OnDestroy()
    {
        _inputMapping.OnJump -= Jump;
        _inputMapping.OnAttack -= Attack;
        _inputMapping.OnSprint -= Sprint;
        _playerAnimation.OnDied -= Die;
        _playerAnimation.OnFootLeft -= PlaySmokeEffect;
        _playerAnimation.OnFootRight -= PlaySmokeEffect;
        _inputMapping.DisableInputs();
    }


    private void Sprint(bool sprinting)
    {
        _isSprinting = sprinting;
        _speed = sprinting ? _speed * _sprintMultiplier : _baseSpeed;
    }

    private void Attack()
    {
        _playerAnimation.SetAttackMode(true);
        var hits = Physics.OverlapSphere(
            transform.position,
            _radius,
            _layerCanAttack
        );

        foreach (var hit in hits)
        {
            Debug.Log(hit.name);
            Debug.Log(hit.transform.name);
            var healComponent = hit.GetComponent<HealBox>();
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
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 0.6f);

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
        var move = new Vector3(_inputMapping.MoveInput.x, 0f, _inputMapping.MoveInput.y);
        _playerAnimation.SetSpeed(_isSprinting ? move.magnitude : move.magnitude / 2);
        
        if (move == Vector3.zero) return;

        var worldMove = (_playerCamera.CameraRight * move.x
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
        Debug.Log(other.name);
        if (!other.CompareTag("Ground")) return;
        _isJumping = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.collider.name);
        if (!other.collider.CompareTag("Ground")) return;
        _isJumping = false;
    }
}