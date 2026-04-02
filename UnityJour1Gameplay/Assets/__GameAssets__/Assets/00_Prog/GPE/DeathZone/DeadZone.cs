using System;
using UnityEngine;

public class DeadZone : GPEBase
{
   protected override void OnPlayerEnter(Collider player)
   {
       base.OnPlayerEnter(player);
       
       //On récupère le GameManager
       GameManager.Instance.ResetPlayer();
   }

   private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
