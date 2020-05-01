using System.Collections.Generic;
using UnityEngine;

public class ArrowLauncher : MonoBehaviour
{
    [SerializeField] private List<Arrow> arrows;
    private Queue<int> _arrowQueue = new Queue<int>(15);

    private void OnEnable()
    {
        for (var i = 0; i < arrows.Count; i++)
        {
            _arrowQueue.Enqueue(i);
        }
    }

    public void Launch(float force)
    {
        var positionInQueue = _arrowQueue.Dequeue();
        var arrowToFire = arrows[positionInQueue];
        arrowToFire.gameObject.transform.position = transform.position;
        arrowToFire.gameObject.transform.rotation = transform.rotation;
        arrowToFire.gameObject.SetActive(true);
        arrowToFire.arrowRigidbody.velocity = transform.right * force;
        _arrowQueue.Enqueue(positionInQueue);
    }
    
    
}
