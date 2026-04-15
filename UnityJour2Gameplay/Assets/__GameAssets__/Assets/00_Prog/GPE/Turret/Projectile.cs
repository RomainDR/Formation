using System;
using UnityEngine;

public class Projectile : GPEBase
{
    [SerializeField] private float lifeTime = 5.0f;
    [SerializeField] private float speed = 5.0f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += transform.forward * (speed * Time.deltaTime);
    }

    protected override void OnPlayerEnter(Collider player)
    {
        base.OnPlayerEnter(player);
        Debug.Log("Test");
        Destroy(gameObject);

        player.GetComponent<Player>().RemoveLife(1);
    }
}