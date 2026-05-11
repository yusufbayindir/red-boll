using UnityEngine;

public sealed class RedBallPlayer : MonoBehaviour
{
    private readonly Collider2D[] groundHits = new Collider2D[12];

    private RedBallGame game;
    private Rigidbody2D body;
    private Transform rollingVisual;
    private ContactFilter2D groundFilter;
    private float jumpBufferTimer;
    private float coyoteTimer;
    private bool grounded;
    private int facingDirection = 1;

    public bool IsFalling => body != null && body.linearVelocity.y <= 0.2f;

    public void Initialize(RedBallGame owner, Rigidbody2D rigidbody2D, Transform bodyVisual)
    {
        game = owner;
        body = rigidbody2D;
        rollingVisual = bodyVisual;
        groundFilter = new ContactFilter2D { useTriggers = false };
    }

    private void Update()
    {
        if (game == null || body == null)
        {
            return;
        }

        grounded = CheckGrounded();
        coyoteTimer = grounded ? 0.12f : Mathf.Max(0f, coyoteTimer - Time.deltaTime);
        float moveInput = game.MoveInput;
        if (Mathf.Abs(moveInput) > 0.08f)
        {
            facingDirection = moveInput > 0f ? 1 : -1;
        }

        if (game.ConsumeJumpPressed())
        {
            jumpBufferTimer = 0.14f;
        }
        else
        {
            jumpBufferTimer = Mathf.Max(0f, jumpBufferTimer - Time.deltaTime);
        }

        if (rollingVisual != null)
        {
            rollingVisual.Rotate(Vector3.forward, -body.linearVelocity.x * 155f * Time.deltaTime);
        }

        if (transform.position.y < game.KillY)
        {
            game.DamagePlayer();
        }
    }

    private void FixedUpdate()
    {
        if (game == null || body == null)
        {
            return;
        }

        float input = game.MoveInput;
        float targetSpeed = input * 7.2f;
        float acceleration = grounded ? 12.5f : 6.5f;
        float velocityDelta = targetSpeed - body.linearVelocity.x;
        body.AddForce(Vector2.right * velocityDelta * acceleration, ForceMode2D.Force);

        if (jumpBufferTimer > 0f && (grounded || coyoteTimer > 0f))
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, 11.8f);
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
            grounded = false;
            game.PlayJumpSound();
        }

        body.linearVelocity = new Vector2(Mathf.Clamp(body.linearVelocity.x, -8.5f, 8.5f), Mathf.Clamp(body.linearVelocity.y, -22f, 16f));
    }

    public void Bounce(float force)
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, force);
        jumpBufferTimer = 0f;
        coyoteTimer = 0f;
        game.PlayBounceSound();
    }

    public void Respawn(Vector2 spawn)
    {
        body.linearVelocity = Vector2.zero;
        body.angularVelocity = 0f;
        transform.position = spawn;
    }

    private bool CheckGrounded()
    {
        int count = Physics2D.OverlapCircle((Vector2)transform.position + Vector2.down * 0.55f, 0.18f, groundFilter, groundHits);
        for (int i = 0; i < count; i++)
        {
            Collider2D hit = groundHits[i];
            if (hit == null || hit.isTrigger || hit.attachedRigidbody == body)
            {
                continue;
            }

            return true;
        }

        return false;
    }
}
