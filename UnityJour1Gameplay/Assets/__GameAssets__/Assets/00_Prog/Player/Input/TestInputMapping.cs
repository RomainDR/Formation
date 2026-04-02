using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class TestInputMapping : MonoBehaviour
{
    // Créer une variable du type du mapping.
    // Dans mon cas je l'ai appelé "Mapping" donc je mets "Mapping _mapping".
    // Si vous l'avez appelé "InputMapping", vous mettez "InputMapping _inputMapping".

    private Mapping _mapping;
    private Vector2 _moveInput;
    
    public Vector2 MoveInput => _moveInput;

    public Vector2 MoveCamera => Mouse.current.delta.ReadValue();

    public Action OnJump;
    public Action OnAttack;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void RegisterInputs()
    {
        _mapping = new Mapping();

        _mapping.Enable();

        _mapping.Player.Move.performed += RegisterMove;

        _mapping.Player.Move.canceled += CancelMove;

        _mapping.Player.Jump.performed += Jump;
        
        _mapping.Player.Attack.performed += Attack;
    }

    private void Attack(InputAction.CallbackContext obj)
    {
        Debug.Log("Attack");
        OnAttack.Invoke();
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        Debug.Log("Jump");
        OnJump.Invoke();
    }

    private void CancelMove(InputAction.CallbackContext ctx)
    {
        _moveInput = Vector2.zero;
    }

    void RegisterMove(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();
    }

    public void DisableInputs()
    {
        _mapping?.Disable();
    }
}