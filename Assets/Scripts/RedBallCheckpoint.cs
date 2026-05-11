using UnityEngine;

public sealed class CheckpointTrigger : MonoBehaviour
{
    public RedBallGame Game;
    public Vector2 SpawnPoint;
    public SpriteRenderer BodyRenderer;

    private bool activated;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated || other.GetComponent<RedBallPlayer>() == null)
        {
            return;
        }

        activated = true;
        Game.ActivateCheckpoint(SpawnPoint);
        transform.localScale = Vector3.one * 1.08f;
        if (BodyRenderer != null)
        {
            BodyRenderer.color = new Color(1f, 0.95f, 0.28f, 1f);
        }
    }
}
