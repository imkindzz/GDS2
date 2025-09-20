using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VillagerBossPhase2 : MonoBehaviour
{
    
    private VillagerBossPhase2Attack phase2Attack;

    public float speed = 3f;
    public float moveDuration = 5f;
    public float changeDirectionInterval = 1f;
    public float returnSpeed = 5f;

    private Vector2 moveDirection;
    private bool isMoving = false;
    private Transform parentTransform;
    private Camera mainCamera;
    private Vector3 startPosition;
    public bool ready = true;

    void Start()
    {
        phase2Attack = GetComponent<VillagerBossPhase2Attack>();

        parentTransform = transform.parent;
        if (parentTransform == null)
        {
            Debug.LogError("RandomMovementEnemy: This object has no parent to move!");
            return;
        }

        mainCamera = Camera.main;
        startPosition = parentTransform.position;

        StartCoroutine(MovementLoop());
    }

    IEnumerator MovementLoop()
    {
        while (true)
        {
            yield return StartCoroutine(RandomMovementRoutine());
            yield return StartCoroutine(ReturnToStartPosition());

            phase2Attack.StartLineAttack();

            yield return new WaitForSeconds(3f);
        }
    }


    IEnumerator RandomMovementRoutine()
    {
        isMoving = true;
        float elapsedTime = 0f;

        int borderLayerMask = LayerMask.GetMask("Border");

        while (elapsedTime < moveDuration)
        {
            PickNewDirection();
            float interval = Mathf.Min(changeDirectionInterval, moveDuration - elapsedTime);

            float timer = 0f;
            while (timer < interval)
            {

                RaycastHit2D hit = Physics2D.Raycast(parentTransform.position, moveDirection, 1f, borderLayerMask);

                if (hit.collider != null)
                {
                    string borderName = hit.collider.gameObject.name;

                    switch (borderName)
                    {
                        case "Up":
                            moveDirection = Vector2.down;
                            break;
                        case "Down":
                            moveDirection = Vector2.up;
                            break;
                        case "Left":
                            moveDirection = Vector2.right;
                            break;
                        case "Right":
                            moveDirection = Vector2.left;
                            break;
                        default:

                            PickNewDirection();
                            break;
                    }
                }

                Vector3 newPos = parentTransform.position + (Vector3)(moveDirection * speed * Time.deltaTime);
                newPos = ClampToScreenBounds(newPos);
                parentTransform.position = newPos;

                timer += Time.deltaTime;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        isMoving = false;
        moveDirection = Vector2.zero;
    }


    IEnumerator ReturnToStartPosition()
    {
        while (Vector3.Distance(parentTransform.position, startPosition) > 0.01f)
        {
            parentTransform.position = Vector3.MoveTowards(
                parentTransform.position,
                startPosition,
                returnSpeed * Time.deltaTime
            );

            yield return null;
        }

        parentTransform.position = startPosition;
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(3f);

        StartCoroutine(RandomMovementRoutine());
    }

    void PickNewDirection()
    {
        float angle = Random.Range(0f, 360f);
        moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }

    Vector3 ClampToScreenBounds(Vector3 position)
    {
        Vector3 min = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 max = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        float padding = 0.5f;

        position.x = Mathf.Clamp(position.x, min.x + 2.5f, max.x - 2.5f);
        position.y = Mathf.Clamp(position.y, min.y + padding, max.y - padding);

        return position;
    }

    void OnDrawGizmosSelected()
    {
        if (isMoving && parentTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(parentTransform.position, parentTransform.position + (Vector3)(moveDirection * 2f));
        }
    }
}
