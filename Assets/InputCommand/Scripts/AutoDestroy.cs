using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    private float destroyAt;

    void Start()
    {
        destroyAt = Time.time + lifeTime;
    }

    void Update()
    {
        if (destroyAt <= Time.time)
        {
            Destroy(gameObject);
        }
    }
}
