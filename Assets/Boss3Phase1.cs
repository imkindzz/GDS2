using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Phase1 : MonoBehaviour
{
    public GameObject Sword;

    void Start()
    {
        StartCoroutine(SlashAttack());
    }

    IEnumerator SwingOnce(Vector3 startPoint, Vector3 endPoint, Vector3 controlPoint, float duration, bool flipVertically, bool flipHorizontally)
    {
        float time = 0f;

        SoundManager.instance.PlaySound(SfxSoundName.SwordSwing);

        // Flip sword
        Vector3 scale = Sword.transform.localScale;
        scale.y = Mathf.Abs(scale.y) * (flipVertically ? -1 : 1);
        Sword.transform.localScale = scale;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            Vector3 pos = GetBezierPoint(startPoint, controlPoint, endPoint, t);
            Sword.transform.position = pos;

            if (t < 1f)
            {
                Vector3 nextPos = GetBezierPoint(startPoint, controlPoint, endPoint, t + 0.01f);
                Vector3 dir = nextPos - pos;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Sword.transform.rotation = Quaternion.Euler(0, 0, angle);
            }

            yield return null;
        }
    }

    IEnumerator SlashAttack()
    {
        float duration = 0.5f;
        Vector3 bossPos = transform.position;

        Vector3 rightPoint = bossPos + new Vector3(2f, 0f, 0f);
        Vector3 leftPoint = bossPos + new Vector3(-2f, 0f, 0f);
        Vector3 controlPoint = bossPos + new Vector3(0f, -2f, 0f);

        while (true)
        {
            // Swing right to left
            yield return StartCoroutine(SwingOnce(rightPoint, leftPoint, controlPoint, duration, flipVertically: false, flipHorizontally: false));

            yield return new WaitForSeconds(0.05f);

            // Swing left to right (flip vertically and horizontally)
            yield return StartCoroutine(SwingOnce(leftPoint, rightPoint, controlPoint, duration, flipVertically: true, flipHorizontally: true));

            yield return new WaitForSeconds(0.05f);
        }
    }

    // Quadratic Bezier formula
    Vector3 GetBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        return uu * p0 + 2 * u * t * p1 + tt * p2;
    }
}