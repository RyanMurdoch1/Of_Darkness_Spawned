using System.Collections;
using UnityEngine;

public class CrowFlyController : MonoBehaviour
{
    [SerializeField] private  Animator animator;
    private static readonly int Fly = Animator.StringToHash("Flying");
    private bool _spooked;
    private readonly WaitForSeconds _waitToDestroy = new WaitForSeconds(5f);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || _spooked) return;
        animator.SetBool(Fly, true);
        StartCoroutine(WaitToRemove());
        _spooked = true;
    }

    private void FixedUpdate()
    {
        if (_spooked)
        {
            gameObject.transform.Translate(0.06f, 0.03f, 0);
        }
    }

    private IEnumerator WaitToRemove()
    {
        yield return _waitToDestroy;
        Destroy(gameObject);
    }
}
