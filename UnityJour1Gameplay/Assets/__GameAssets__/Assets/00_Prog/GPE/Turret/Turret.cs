using System;
using System.ComponentModel;
using UnityEngine;

[ExecuteAlways]
public class Turret : GPEBase
{
    [SerializeField] private float radiusDetection = 7.5f;
    [SerializeField, ReadOnly(true)] private Player playerTarget;
    [SerializeField, ReadOnly(true)] private float shootTime;
    [SerializeField, ReadOnly(true)] private Transform projectileInstance;

    private float _timeElapsed;
    private void Update()
    {
        if (!playerTarget) return;
        transform.LookAt(playerTarget.transform);
        _timeElapsed += Time.deltaTime;
        if (!(_timeElapsed >= shootTime)) return;
        _timeElapsed = 0;
         Instantiate(projectileInstance, transform.position + transform.forward, transform.rotation).name = "Projectile " + Guid.NewGuid();
    }

    protected override void OnPlayerEnter(Collider player)
    {
        base.OnPlayerEnter(player);
        playerTarget = player.GetComponent<Player>();
    }

    protected override void OnPlayerExit(Collider player)
    {
        base.OnPlayerExit(player);
        playerTarget = null;
    }

    private void OnDrawGizmos()
    {
        GetComponent<SphereCollider>().radius = radiusDetection;
        Gizmos.color = playerTarget != null ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusDetection);
    }
}