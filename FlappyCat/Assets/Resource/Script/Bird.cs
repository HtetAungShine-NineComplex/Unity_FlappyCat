using UnityEngine;
using System;

public class Bird : MonoBehaviour
{
    private static Bird my_instance;
    private const float jumpForce = 85f;
    private Rigidbody2D bird_RB;
    private BirdState birdState;
    private SpriteRenderer spriteRenderer;

    public static Bird GetInstance { get { return my_instance; } }
    public event EventHandler OnDied;
    public event EventHandler OnStartFly;

    [SerializeField] private Sprite flyCat_Sprite;
    [SerializeField] private Sprite defaultCat_Sprite;

    private enum BirdState
    {
        WaitToFly, Fly, Die
    }
    private void Awake()
    {
        my_instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
        bird_RB = GetComponent<Rigidbody2D>();
        bird_RB.bodyType = RigidbodyType2D.Static;
        birdState = BirdState.WaitToFly;

    }
    void Update()
    {
        switch (birdState)
        {

            case BirdState.WaitToFly:
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                {
                    birdState = BirdState.Fly;
                    bird_RB.bodyType = RigidbodyType2D.Dynamic;
                    spriteRenderer.sprite = flyCat_Sprite;
                    Jump();
                    if (OnStartFly != null) OnStartFly(this, EventArgs.Empty);
                }
                break;

            case BirdState.Fly:
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                {
                    spriteRenderer.sprite = flyCat_Sprite;
                    Jump();
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    spriteRenderer.sprite = defaultCat_Sprite;
                }
                break;

            case BirdState.Die:
                break;
        }

    }

    private void Jump()
    {
        if (bird_RB.bodyType != RigidbodyType2D.Static)
        {
            bird_RB.velocity = Vector2.up * jumpForce;
            SoundManager.GetInstance.PlaySound(SoundManager.audioClipEnum.birdJump);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bird_RB.bodyType = RigidbodyType2D.Static;
        SoundManager.GetInstance.PlaySound(SoundManager.audioClipEnum.dead);
        if (OnDied != null) OnDied(this, EventArgs.Empty);
    }
    private void OnMouseUp()
    {

    }

    private void OnMouseDrag()
    {

    }
}
