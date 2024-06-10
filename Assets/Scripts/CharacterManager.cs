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
    [SerializeField] private float hp = 10;
    [SerializeField] private float curHp;
    [SerializeField] Slider hpBar;
    [SerializeField] GameObject bloodSpurt;


    [Header("Attack Settings")]
    // for enemy attack
    [SerializeField] float damage = 10;
    [SerializeField] float meleeCooltime = 0.5f;
    float timeBetweenAttack, timeSinceAttack;
    // fireball attack
    private GameObject bullets;
    public GameObject bullet;
    public Transform bulletPos;
    [SerializeField] private float bulletCooltime;
    // aura attack
    public GameObject aura;
    public Transform auraPos;
    [SerializeField] private float auraCooltime;
    // skill
    public int skillCount = 3;
    public int curSkillCount;
    private bool isRecovering = false;
    private float recoverSpeed = 1f;
    public delegate void OnSkillChangedDelegate();
    [HideInInspector] public OnSkillChangedDelegate onSkillChangedCallback;
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
    bool attack;


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


        //카드 설정
        InitPassveCard();

        curSkillCount = skillCount;
        curHp = hp;
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
                Reset();

                // Recovering Skill Counts
                if (curSkillCount < skillCount)
                {
                    StartCoroutine(RecoverSkillCount(recoverSpeed));
                }
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
        if (pState.dashing) return;
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
            // TODO: 끝나면 윈도우 띄우기.
            Debug.Log("StageClear!");
        }
        if (!pState.defending)
        {
            rb.velocity = new Vector2(walkSpeed * xAxis, rb.velocity.y);
            anim.SetBool("Walking", rb.velocity.x != 0 && Grounded());
        }

    }

    private void Dash()
    {


        if (Input.GetButtonDown("Dash"))
        {
            if (GameManager.Instance.ability == 0)
            {
                StartCoroutine(PerformDash());

            }

        }
    }

    private IEnumerator PerformDash()
    {
        if (!pState.dashing && Grounded())
        {
            SkillCount--;

            anim.SetTrigger("Dashing");

            pState.dashing = true;

            if (pState.lookingRight)
            {
                // 목표 위치 설정
                targetPosition = transform.position + new Vector3(3f, 0, 0);
            }
            else if (!pState.lookingRight)
            {
                // 목표 위치 설정
                targetPosition = transform.position + new Vector3(-3f, 0, 0);
            }

            StartCoroutine(MoveOverTime(targetPosition));

            // Change Layer
            gameObject.layer = LayerMask.NameToLayer("PlayerDashing");

            // Dash time
            yield return new WaitForSeconds(0.5f);

            // Back to original layer
            gameObject.layer = originalLayer;

            pState.dashing = false;
        }
    }

    public int SkillCount
    {
        get { return curSkillCount; }
        set
        {
            if (curSkillCount != value)
            {
                curSkillCount = Mathf.Clamp(value, 0, skillCount);

                if (onSkillChangedCallback != null)
                {
                    onSkillChangedCallback.Invoke();
                }
            }
        }
    }

    private IEnumerator RecoverSkillCount(float spd)
    {
        if (isRecovering)
        {
            yield break;
        }

        isRecovering = true;
        yield return new WaitForSeconds(spd);
        SkillCount++;
        isRecovering = false;
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

        if (!pState.jumping && !pState.dashing)
        {

            // 일반 점프
            if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce);
                pState.jumping = true;
            }
            else if (!Grounded() && airJumpCounter < maxAirJumps && Input.GetButtonDown("Jump") && curSkillCount > 0) // 더블점프?
            {
                curSkillCount--;
                pState.jumping = true;
                airJumpCounter++;
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
        if (curTime <= 0 && !pState.attacking)
        {
            timeSinceAttack = Time.deltaTime;

            if (attack && timeSinceAttack >= timeBetweenAttack && GameManager.Instance.mainAttack == 0)
            {
                timeSinceAttack = 0;
                pState.attacking = true;
                anim.SetTrigger("Attack");
                curTime = meleeCooltime;
            }
        }
        else
        {
            curTime -= Time.deltaTime;
        }

        pState.attacking = false;
    }

    // fireball attack
    void ShotAttack()
    {
        if (curTime <= 0 && !pState.dashing)
        {
            if (attack && GameManager.Instance.mainAttack == 1)
            {

                Debug.Log("shot!");
                pState.fireball = true;
                anim.SetTrigger("Fireball");
                bullets = Instantiate(bullet, bulletPos.position, transform.rotation);
                bullets.GetComponent<bullet>().dir = pState.lookingRight;
                bullets.GetComponent<bullet>().damage = damage/2;
                curTime = bulletCooltime;

            }
        }
        curTime -= Time.deltaTime;
        pState.fireball = false;
    }

    // aura attack
    void AuraAttack()
    {
        if (curTime <= 0 && !pState.dashing)
        {
            if (attack && GameManager.Instance.mainAttack == 2)
            {
                anim.SetTrigger("Attack");
                Debug.Log("Aura Attack!");
                bullets = Instantiate(aura, auraPos.position, transform.rotation);
                bullets.GetComponent<bullet>().dir = pState.lookingRight;
                bullets.GetComponent<bullet>().damage = damage;
                curTime = auraCooltime;
            }
        }
        curTime -= Time.deltaTime;
    }



    void Defend()
    {
        if (Input.GetKeyDown(KeyCode.C) && !pState.defending)
        {
            if (GameManager.Instance.ability == 2 && skillCount > 0)
            {
                SkillCount--;
                StartCoroutine(StartDefend(1.0f));
                anim.SetBool("Defending", false);
                pState.defending = false;
            }

        }

    }

    public IEnumerator StartDefend(float duration)
    {
        float time = 0.0f;

        while (time < 1.0f)
        {
            time += Time.deltaTime / duration;

            // ======= 하고자 하는 작업을 구현 =======
            anim.SetBool("Defending", true);
            pState.defending = true;
            Debug.Log("Defend On");
            // =====================================

            yield return null;
        }
        anim.SetBool("Defending", false);
        pState.defending = false;
    }

    // Player gets damage function
    public void Hurt(float damage, Vector2 pos)
    {
        if (!isHurt)
        {
            isHurt = true;

            if (!pState.defending)
            {
                curHp -= damage;
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
        while (ctime < 0.2f)
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
        while (isHurt)
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


    public void InitPassveCard()
    {
        // Ability 관리 :
        if (GameManager.Instance.ability == 1)
        {
            maxAirJumps = 2;
        }

        // passive skill 발동 : 
        if (GameManager.Instance.passiveSkill == 0)
        {
            hp = hp * 2;
            damage = damage * 2;
        }
        if (GameManager.Instance.passiveSkill == 1)
        {
            walkSpeed = walkSpeed * 2;

            meleeCooltime = 0.1f;
            bulletCooltime = 0.2f;
            auraCooltime = 0.4f;
        }
        if (GameManager.Instance.passiveSkill == 2)
        {
            skillCount = 5;
            recoverSpeed = recoverSpeed * 1/2;
        }
    }

    private void Reset()
    {
        if (Input.GetKeyDown(KeyCode.R)){
            Hurt(999999999999999, new Vector2(0, 0));

        }
    }
}