using UnityEngine;
using UnityEngine.VFX;

public class HealBox : Collectible
{
    [SerializeField] private int lifeToGain = 1;
    [SerializeField] private HealFX healVFX;
    
    public void AddLife(Player player)
    {
        player.AddLife(lifeToGain);
        Debug.Log("Add life to player");
        
        Instantiate(healVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}