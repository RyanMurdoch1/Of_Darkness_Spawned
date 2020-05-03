using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] private List<Projectile> projectiles;
    private readonly Queue<int> _projectileQueue = new Queue<int>(15);

    private void OnEnable()
    {
        for (var i = 0; i < projectiles.Count; i++)
        {
            _projectileQueue.Enqueue(i);
        }
    }

    public void Launch(float force)
    {
        var positionInQueue = _projectileQueue.Dequeue();
        var arrowToFire = projectiles[positionInQueue];
        arrowToFire.gameObject.transform.position = transform.position;
        arrowToFire.gameObject.transform.rotation = transform.rotation;
        arrowToFire.gameObject.SetActive(true);
        arrowToFire.arrowRigidbody.velocity = transform.right * force;
        _projectileQueue.Enqueue(positionInQueue);
    }
}
