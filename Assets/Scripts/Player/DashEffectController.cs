using UnityEngine;
using System.Collections;

public class DashEffectController : MonoBehaviour
{
    [Header("-------Dash--------")]
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCoolDown = 1f;
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private float ghostSpawnInterval = 0.05f;

    public bool isDashing = false;
    public bool canDash = true;
    private float ghostTimer = 0f;
    private float dashTimer = 0f;

    private Rigidbody2D rb;
    private Vector2 dashDirection;
    private SpriteRenderer playerSR;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSR = GetComponent<SpriteRenderer>();
    }


    public void StartDash(Vector2 direction)
    {
        if (!canDash || direction == Vector2.zero) return;

        isDashing = true;
        canDash = false;
        dashDirection = direction.normalized;
        dashTimer = dashDuration;
    }

    private void FixedUpdate()
    {
        if (!isDashing) return;

        rb.velocity = dashDirection * dashSpeed;

        ghostTimer -= Time.fixedDeltaTime;
        if (ghostTimer <= 0)
        {
            SpawnGhost();
            ghostTimer = ghostSpawnInterval;
        }

        dashTimer -= Time.fixedDeltaTime;
        if (dashTimer <= 0)
        {
            isDashing = false;
            StartCoroutine(DashCoolDown());
        }
    }

    private IEnumerator DashCoolDown()
    {
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }

    private void SpawnGhost()
    {
        GameObject ghost = MyPoolManager.Instance.GetFromPool(ghostPrefab, null);
        ghost.transform.position = transform.position;
        ghost.transform.rotation = transform.rotation;

        var ghostSR = ghost.GetComponent<SpriteRenderer>();
        ghostSR.sprite = playerSR.sprite;
        ghostSR.flipX = playerSR.flipX;

        StartCoroutine(FadeAndReturnToPool(ghost, 0.3f));
    }

    private IEnumerator FadeAndReturnToPool(GameObject ghost, float duration)
    {
        SpriteRenderer sr = ghost.GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        ghost.SetActive(false);
    }
}