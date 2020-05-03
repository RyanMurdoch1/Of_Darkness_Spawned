using System.Collections;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private ProjectileLauncher _launcher;
    
    private void OnEnable()
    {
        StartCoroutine(FireArrows());
    }

    private IEnumerator FireArrows()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(4f);
            _launcher.Launch(10);
        }
    }
}
