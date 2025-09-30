using UnityEngine;

public class FaceSoul : MonoBehaviour
{
    public string soulTag = "Soul";
    public float rotationOffset = 0f;

    private Transform soul;

    private void Start()
    {
        GameObject soulGO = GameObject.FindWithTag(soulTag);
        if (soulGO != null)
            soul = soulGO.transform;
    }

    private void Update()
    {
        if (soul == null) return;

        Vector3 direction = soul.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);
    }
}


