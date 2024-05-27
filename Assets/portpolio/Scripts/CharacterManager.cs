using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class CharacterManager : MonoBehaviour
{
    [Header("Horizontal Movement Settings")]
    [SerializeField] private float walkSpeed = 1;
    [SerializeField] private float dashCooldown;
    
    
    [Header("Vertical Movement Settings")]
    [SerializeField] private float jumpForce = 45;
    private int jumpBufferCounter = 0;
    [SerializeField] private int jumpBufferFrames;
    private float coyoteTimeCounter = 0;
    [SerializeField] private float coyoteTime;
    private int airJumpCounter = 0;
    [SerializeField] private int maxAirJumps;


    // Ground Check Settings
    [SerializeField] private Transform groundCheckPoint;
    private float groundCheckY = 0.2f;
    private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask whatIsGround;

    
    [Header("Recoil Settings")]
    // 이동에 걸리는 시간 (초)
    [SerializeField] public float moveDuration = 0.1f;    
    

    [Header("HP Settings")]
    [SerializeField] float hp = 10;
    [SerializeField] float curHp = 10;
    [SerializeField] Slider hpBar;
    [SerializeField] GameObject bloodSpurt;


    [Header("Attack Settings")]
    // for enemy attack
    [SerializeField] float damage = 1;
    [SerializeField] float meleeCooltime = 0.5f;
    float timeBetweenAttack, timeSinceAttack;
    // fireball attack
    private GameObject bullets;
    public GameObject bullet;
    public Transform bulletPos;
    [SerializeField] float bulletCooltime;
    // aura attack
    public GameObject aura;
    public Transform auraPos;
    [SerializeField] float auraCooltime;
    // Knockback
    private float knockbackSpeed = 10;
    private float originalKnockbackSpeed;
    private float defendKnockbackSpeed = 5;
    private float curTime;


    [Header("Stage Setting")]
    [SerializeField] GameObject startPoint;
    [SerializeField] GameObject endPoint;


    // Normal Variables
    private Animator anim;
    private Rigidbody2D rb;
    PlayerStateList pState;
    SpriteRenderer sr;
    bool isHurt;
    bool isKnockback;
    Color halfA = new Color(1, 1, 1, 0.5f);
    Color fullA = new Color(1, 1, 1, 1);
    private float xAxis;
    bool attack = false;
    
    
    // 리코일의 이동할 목표 위치
    private Vector2 targetPosition;
    public static CharacterManager Instance;
    private int originalLayer;

    GameObject sm;
    GameObject gm;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pState = GetComponent<PlayerStateList>();
        pState.alive = true;
        pState.lookingRight = true;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        whatIsGround = LayerMask.GetMask("Ground");
        originalLayer = gameObject.layer;

        sm = GameObject.Find("StageManager");
        gm = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    private void Update()
    {
        if (sm.GetComponent<StageManager>().isPaused)
        {
            Debug.Log("Paused!!!!");
        }
        else
        {
            if (pState.alive)
            {
                GetInputs();
                UpdateJumpVariables();
                Move();
                Jump();
                Flip();
                Dash();
                Attack();
                ShotAttack();
                AuraAttack();
                Defend();
            }
            
        }

    }

    // Recoil when Player attack with weapon
    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.tag == "Enemy")
        {
            Debug.Log("Hit");

            // 적 공격 부
            _other.GetComponent<Enemy>().Hit(damage);

            // 이부분 리코일 따로 빼줄 수 있나?
            float recoilPower = 2f;
            pState.recoilingX = true;

            if (transform.position.x - _other.transform.position.x > 0) 
            {
                // 목표 위치 설정
                targetPosition = transform.position + new Vector3(recoilPower, 0, 0);
            }
            else if (transform.position.x - _other.transform.position.x < 0)
            {
                // 목표 위치 설정
                targetPosition = transform.position + new Vector3(-recoilPower, 0, 0);
            }

            // Coroutine 시작
            StartCoroutine(MoveOverTime(targetPosition));
        }
    }


    // Recoil when Player gets hit with Enemy
    private void OnCollisionStay2D(Collision2D _other)
    {
        if (_other.gameObject.tag == "Enemy")
        {
            Debug.Log("Body Hit!");

            Hurt(_other.gameObject.GetComponent<Enemy>().enemyDamage, _other.transform.position);
        }
    }

    // 서서히 리코일하는 Coroutine
    private IEnumerator MoveOverTime(Vector2 _targetPosition)
    {
        float elapsedTime = 0f;
        Vector2 startPosition = transform.position;

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            transform.position = Vector2.Lerp(startPosition, _targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 이동 완료 후 마지막 위치 설정 (목표 위치)
        transform.position = _targetPosition;
    }

    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        attack = Input.GetMouseButtonDown(0);
    }

    void Flip()
    {
        if (xAxis < 0)
        {
            transform.localScale = new Vector2(-5, transform.localScale.y);
            pState.lookingRight = false;
        }
        else if (xAxis > 0) 
        {
            transform.localScale = new Vector2(5, transform.localScale.y);
            pState.lookingRight = true;
        }
    }

    private void Move()
    {
        if (this.transform.position.x < startPoint.transform.position.x)
        {
            StartCoroutine(Knockback(-1));
        }
        if (this.transform.position.x > endPoint.transform.position.x)
        {
            Debug.Log("StageClear!");
        }
        rb.velocity = new Vector2(walkSpeed * xAxis, rb.velocity.y);
        anim.SetBool("Walking", rb.velocity.x != 0 && Grounded());
    }

    private void Dash() 
    {
        if (Input.GetButtonDown("Dash"))
        {
            if (gm.GetComponent<GameManager>().activeDash)
            {
                StartCoroutine(PerformDash());
            }

        }
    }

    private IEnumerator PerformDash()
    {
        // Change Layer
        gameObject.layer = LayerMask.NameToLayer("PlayerDashing");

        pState.dashing = true;
        anim.SetTrigger("Dashing");
        
        // Dash time
        yield return new WaitForSeconds(0.5f);
        
        // Back to original layer
        gameObject.layer = originalLayer;

        pState.dashing = false;
    }

    public bool Grounded()
    {

        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround) 
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround) 
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround))
        {
            pState.jumping = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    void Jump()
    {
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            pState.jumping = false; 
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }

        if (!pState.jumping)
        {
            if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce);
                pState.jumping = true;
            }
            else if (!Grounded() && airJumpCounter < maxAirJumps && Input.GetButtonDown("Jump"))
            {
                pState.jumping = true;
                airJumpCounter ++;
                rb.velocity = new Vector3(rb.velocity.x, jumpForce);
            }
            anim.SetBool("Jumping", !Grounded());
        }
    }

    void UpdateJumpVariables()
    {
        if (Grounded())
        {
            pState.jumping = false;
            coyoteTimeCounter = coyoteTime;
            airJumpCounter = 0;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferFrames;
        }
        else 
        {
            jumpBufferCounter--;
        }
    }

    void Attack()
    {
        if (curTime <= 0)
        {
            timeSinceAttack = Time.deltaTime;

            if (attack && timeSinceAttack >= timeBetweenAttack)
            {
                timeSinceAttack = 0;
                anim.SetTrigger("Attack");
                curTime = meleeCooltime;
            }
        }
        else
        {
            curTime -= Time.deltaTime;
        }
    }
    
    // fireball attack
    void ShotAttack()
    {
        if (curTime <= 0)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                if(gm.GetComponent<GameManager>().activeRangedAttack)
                {
                    Debug.Log("shot!");
                    anim.SetTrigger("Fireball");
                    bullets = Instantiate(bullet, bulletPos.position, transform.rotation);
                    bullets.GetComponent<bullet>().dir = pState.lookingRight;
                    curTime = bulletCooltime;
                }
            }
        }
        curTime -= Time.deltaTime;
    }

    // aura attack
    void AuraAttack()
    {
        if (curTime <= 0)
        {
            if (Input.GetKey(KeyCode.X))
            {
                anim.SetTrigger("Attack");
                Debug.Log("Aura Attack!");
                bullets = Instantiate(aura, auraPos.position, transform.rotation);
                bullets.GetComponent<bullet>().dir = pState.lookingRight;
                curTime = auraCooltime;
            }
        }
        curTime -= Time.deltaTime;
    }

    void Defend()
    {

            if (Input.GetKeyDown(KeyCode.C) && !pState.defending)
            {
                anim.SetBool("Defending", true);
                pState.defending = true;
                Debug.Log("Defend On");
            }

            if (Input.GetKeyUp(KeyCode.C) && pState.defending)
            {
                anim.SetBool("Defending", false);
                pState.defending = false;
                Debug.Log("Defend Off");
            }
    }

    // Player gets damage function
    public void Hurt(float damage, Vector2 pos)
    {
        if (!isHurt)
        {
            isHurt = true;

            if (!pState.defending)
            {
                curHp = curHp - damage;
            }
            
            if (curHp <= 0)
            {
                //Player's blood effect
                GameObject bloodParticles = Instantiate(bloodSpurt, transform.position, Quaternion.identity);
                Destroy(bloodParticles, 1.5f);
                hpBar.value = 0;
                // player dead
                pState.alive = false;
                anim.SetTrigger("Death");

                sm.GetComponent<StageManager>().ShowGameOverWindow();
                
                Destroy(gameObject, 1);
                
            }
            else
            {
                HandleHp();
                
                float x = transform.position.x - pos.x;
                if (x < 0)
                  x = 1;
                else
                  x = -1;

                if (!pState.defending)
                {
                    anim.SetTrigger("TakeDamage");
                }
                StartCoroutine(Knockback(x));
                StartCoroutine(Invulnerable());
                StartCoroutine(AlphaBlink());
            }
        }
    }

    private void HandleHp()
    {
        hpBar.value = (float)curHp / (float)hp;
    }

    // Recoil Function
    private IEnumerator Knockback(float dir)
    {
        if (pState.defending)
        {
            originalKnockbackSpeed = defendKnockbackSpeed;
        }
        else
        {
            originalKnockbackSpeed = knockbackSpeed;
            //Player's blood effect
            GameObject bloodParticles = Instantiate(bloodSpurt, transform.position, Quaternion.identity);
            Destroy(bloodParticles, 1.5f);
        }
        
        isKnockback = true;
        float ctime = 0;
        while(ctime < 0.2f)
        {
            if (transform.rotation.y == 0)
            {
                transform.Translate(Vector2.left * originalKnockbackSpeed * Time.deltaTime * dir);
            }
            else
            {
                transform.Translate(Vector2.left * originalKnockbackSpeed * Time.deltaTime * -1f * dir);
            }

            ctime += Time.deltaTime;
            yield return null;
        }
        isKnockback = false;
    }

    // Player blinks while player's status is invulnerable
    private IEnumerator AlphaBlink()
    {
        while(isHurt)
        {
            yield return new WaitForSeconds(0.1f);
            sr.color = halfA;
            yield return new WaitForSeconds(0.1f);
            sr.color = fullA;
        }
    }
    
    // Turning player's status to invulnerable for a second
    private IEnumerator Invulnerable()
    {
        yield return new WaitForSeconds(1f);
        isHurt = false;
    }

}