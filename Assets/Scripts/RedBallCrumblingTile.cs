using UnityEngine;

public sealed class RedBallCrumblingTile : MonoBehaviour
{
    private SpriteRenderer[] renderers;
    private Collider2D platformCollider;
    private Color[] originalColors;
    private float crumbleDelay = 0.45f;
    private float fadeSeconds = 0.18f;
    private float crumbleTimer;
    private bool crumbling;
    private bool collapsed;

    public void Initialize(SpriteRenderer[] tileRenderers, Collider2D tileCollider, float delay, float fadeDuration)
    {
        renderers = tileRenderers;
        platformCollider = tileCollider;
        crumbleDelay = Mathf.Max(0.05f, delay);
        fadeSeconds = Mathf.Max(0.05f, fadeDuration);
        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].color;
        }
    }

    private void Update()
    {
        if (!crumbling || collapsed)
        {
            return;
        }

        crumbleTimer += Time.deltaTime;
        float warningT = Mathf.Clamp01(crumbleTimer / crumbleDelay);
        float pulse = Mathf.PingPong(crumbleTimer * 14f, 1f);
        Color warning = Color.Lerp(new Color(1f, 0.82f, 0.28f, 1f), new Color(1f, 0.35f, 0.22f, 1f), pulse);
        SetColor(Color.Lerp(Color.white, warning, warningT));

        if (crumbleTimer < crumbleDelay)
        {
            return;
        }

        float fadeT = Mathf.Clamp01((crumbleTimer - crumbleDelay) / fadeSeconds);
        transform.localScale = new Vector3(1f, Mathf.Lerp(1f, 0.1f, fadeT), 1f);
        SetAlpha(1f - fadeT);

        if (fadeT >= 1f)
        {
            Collapse();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCrumblingIfPlayer(collision.collider);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        StartCrumblingIfPlayer(collision.collider);
    }

    private void StartCrumblingIfPlayer(Collider2D other)
    {
        if (collapsed || crumbling || other.GetComponent<RedBallPlayer>() == null)
        {
            return;
        }

        crumbling = true;
        crumbleTimer = 0f;
    }

    private void Collapse()
    {
        collapsed = true;
        if (platformCollider != null)
        {
            platformCollider.enabled = false;
        }

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null)
            {
                renderers[i].enabled = false;
            }
        }
    }

    private void SetColor(Color color)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null)
            {
                Color original = i < originalColors.Length ? originalColors[i] : Color.white;
                renderers[i].color = new Color(color.r * original.r, color.g * original.g, color.b * original.b, original.a);
            }
        }
    }

    private void SetAlpha(float alpha)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null)
            {
                Color color = renderers[i].color;
                color.a = alpha;
                renderers[i].color = color;
            }
        }
    }
}
