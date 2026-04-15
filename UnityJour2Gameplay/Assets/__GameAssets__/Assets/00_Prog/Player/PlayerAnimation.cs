using System;
using __GameAssets__.Assets._00_Prog.UI;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimationEffect))]
public class PlayerAnimation : MonoBehaviour
{
    //Définir la clé de notre variable dans l'animator
    private static readonly int Speed = Animator.StringToHash("speed");
    private static readonly int Attack = Animator.StringToHash("IsAttack");
    private static readonly int Die = Animator.StringToHash("IsDied");

    //Mettre en field l'animator à choisir
    [SerializeField] public Animator animator;

    public Action OnDied;
    public Action OnFootLeft;
    public Action OnFootRight;

    //définir une méthode pour mettre à jour la speed dans notre animator
    public void SetSpeed(float speed)
    {
        animator?.SetFloat(Speed, speed);
    }
    
    //définir une méthode pour mettre à jour le boolean de notre animator
    public void SetAttackMode(bool _attacking)
    {
        animator?.SetBool(Attack, _attacking);
    }

    private void EndAttack()
    {
        Debug.Log("End Attack");
        SetAttackMode(false);
    }
    
    public void Died()
    {
        Debug.Log("Died from animation");
        animator.SetBool(Die, true);
    }

    private void EndDie()
    {
        OnDied.Invoke();
    }

    private void FootLeft()
         {
             OnFootLeft?.Invoke();
             Debug.Log("Foot Left");
             
         }
     
         private void FootRight()
         {
             OnFootRight?.Invoke();
             Debug.Log("Foot Right");
         }
}