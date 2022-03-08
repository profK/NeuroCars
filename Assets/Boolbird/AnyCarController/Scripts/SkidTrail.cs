using System.Collections;
using UnityEngine;

public class SkidTrail : MonoBehaviour
{
    [SerializeField] public float persistTime = 0f;

    private IEnumerator Start()
    {
        while (true)
        {
            yield return null;

            if (transform.parent.parent == null)
            {
                Destroy(gameObject, persistTime);
            }
        }
    }
}
