using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Vector2 movement;

    public GameManagerScript gamemanager;

    public SpriteRenderer gfx;
    public Animator gfxAnimator;
    public AudioSource audiosrc;

    private void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        audiosrc = gameObject.GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Update()
    {
        //Taking Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x > 0 || movement.y > 0) {
            audiosrc.Play();
            Debug.Log("walkingsound");
        }
        else if (movement.x == 0 && movement.y == 0) {
            audiosrc.Stop();
            Debug.Log("walkingsoundNO");
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        HandleAnimations();
    }

    private void HandleAnimations()
    {
        gfxAnimator.SetFloat("moveX", movement.x);
        gfxAnimator.SetFloat("moveY", movement.y);

        if ((movement != Vector2.zero))
        {
            
            gfxAnimator.SetBool("isMoving", true);
            
        }
        else
        {
            gfxAnimator.SetBool("isMoving", false);
            
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Photograph")) {
            collision.gameObject.transform.GetChild(1).gameObject.SetActive(true); // get the E to Interact
            if (Input.GetKey(KeyCode.E)) {
                gamemanager.addPhoto();
                Destroy(collision.gameObject);
            }
        }
    }
}
