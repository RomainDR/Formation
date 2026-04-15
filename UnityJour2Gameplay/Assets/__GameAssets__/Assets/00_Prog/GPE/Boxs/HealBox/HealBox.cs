using UnityEngine;

public class HealBox : Collectible
{
    [SerializeField] private int lifeToGain = 1;

    public void AddLife(Player player)
    {
        player.AddLife(lifeToGain);
        Debug.Log("Add life to player");
        Destroy(gameObject);
    }
}