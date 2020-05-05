using System.Collections;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private ProjectileLauncher launcher;
    private readonly WaitForSeconds _fireWaitTime = new WaitForSeconds(4f);
    
    private void OnEnable()
    {
        StartCoroutine(FireArrows());
    }

    private IEnumerator FireArrows()
    {
        while (gameObject.activeSelf)
        {
            yield return _fireWaitTime;
            launcher.Launch(10);
        }
    }
}
