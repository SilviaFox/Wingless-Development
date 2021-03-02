using UnityEngine;

public class DestroyOnAwake : MonoBehaviour
{
    [SerializeField] float waitTime = 0.5f;

    private void OnEnable()
    {
        Invoke("DestroyThis", waitTime);
    }

    void DestroyThis()
    {
        Destroy(gameObject);
    }
}
