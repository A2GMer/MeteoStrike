using UnityEngine;
using UnityEngine.UI;

public class Enemies : MonoBehaviour {

    [HideInInspector] public float direction = 0;

    public int ballLife;
    public Text lifeText;
    public Rigidbody2D money;

    private int changeColor;
    private int randomTypeOfBall;

    private void Awake()
    {
        lifeText = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
    }
    private void Start()
    {
        randomTypeOfBall = GenerateEnemies.instance.randomTypeOfBall;
        ballLife = (int)Random.Range(1,PlayerPrefs.GetInt("harderBalls"));

        lifeText.text = "" + ballLife;

        if(randomTypeOfBall == 0)
        {
            SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            sr.sprite = Resources.Load<Sprite>("Balls/ball_2_" + (int)Random.Range(1, 5));
        }
        if(randomTypeOfBall == 1)
        {
            Debug.Log("/ Balls / ball_" + (int)Random.Range(1, 4));
            SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            sr.sprite = Resources.Load<Sprite>("Balls/ball_" + (int)Random.Range(1, 4));
        }

        changeColor = (int)Random.Range(0,2);
    }
    private void Update()
    {
        LifeTextOnBall();

        if (gameObject.transform.position.y < -5.35f)
        {
            GenerateEnemies.instance.numberAliveBalls--;
            Destroy(this.gameObject);
        }
    }
    private void Bounces(Rigidbody2D obj,float bounceForce)
    {
        if(direction == 0)
        {
            float rand = Random.Range(0f,10f);

            if (rand < 5f)
                direction = 1;
            else
                direction = -1;
        }
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(direction * 350f * Time.deltaTime / 10f, 350f * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "edge")
        {
           Bounces(this.gameObject.GetComponent<Rigidbody2D>(), 1000f);
        }
        if (collision.gameObject.tag == "border_left")
        {
            this.gameObject.GetComponent<Rigidbody2D>().velocity = transform.right * 50f * Time.deltaTime;
            direction = 1;
        }
        if (collision.gameObject.tag == "border_right")
        {
            this.gameObject.GetComponent<Rigidbody2D>().velocity = -transform.right * 50f * Time.deltaTime;
            direction = -1;
        }
        if(collision.gameObject.tag == "bullet")
        {
            Destroy(collision.gameObject);
            GenerateTypeOfBalls(GenerateEnemies.instance.ballTypes[0],GenerateEnemies.instance.ballTypes[1],GenerateEnemies.instance.ballTypes[2]);
        }
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instace.timeGameOver = Time.time + 1.0f;
            GameManager.instace.gOver = true;
            GameManager.instace.GameOver();
        }
        if (collision.gameObject.tag == "balls_destroy")
        {
            Destroy(this.gameObject);
        }
    }
    private void GenerateTypeOfBalls(Rigidbody2D ball_2,Rigidbody2D ball_3,Rigidbody2D ball_4)
    {
        if(this.gameObject.name == "ball_1(Clone)")
        {
            GameManager.instace.score++;

            int powerBullet = (int)PlayerPrefs.GetFloat("powerBullet");
            ballLife -= powerBullet;
            lifeText.text = "" + ballLife;

            Debug.Log(powerBullet);

            if (ballLife <= 0)
            {
                Rigidbody2D cloneBall_1 = Instantiate(ball_2, this.gameObject.transform.position, Quaternion.identity);
                Rigidbody2D cloneBall_2 = Instantiate(ball_2, this.gameObject.transform.position, Quaternion.identity);

                int rd = (int)Random.Range(0, 1);

                if (randomTypeOfBall == 0)
                {
                    int rd2 = (int)Random.Range(1, 5);

                    SpriteRenderer sr1 = cloneBall_1.gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
                    SpriteRenderer sr2 = cloneBall_2.gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;

                    sr1.sprite = Resources.Load<Sprite>("Balls/ball_2_" + rd2);
                    sr2.sprite = Resources.Load<Sprite>("Balls/ball_2_" + rd2);
                }
                else
                {
                    int rd2 = (int)Random.Range(1, 4);

                    SpriteRenderer sr1 = cloneBall_1.gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
                    SpriteRenderer sr2 = cloneBall_2.gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;

                    sr1.sprite = Resources.Load<Sprite>("Balls/ball_" + rd2);
                    sr2.sprite = Resources.Load<Sprite>("Balls/ball_" + rd2);
                }

                cloneBall_1.bodyType = RigidbodyType2D.Dynamic;
                cloneBall_2.bodyType = RigidbodyType2D.Dynamic;

                cloneBall_1.velocity = new Vector2(60f * Time.deltaTime, 0f);
                cloneBall_2.velocity = new Vector2(-60f * Time.deltaTime, 0f);

                GenerateEnemies.instance.numberAliveBalls++;

                BallDestruction();
            }
            else
            {
                if (randomTypeOfBall == 1)
                {
                    SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
                    sr.sprite = Resources.Load<Sprite>("Balls/ball_" + (int)Random.Range(1, 4));
                }
                else
                {
                    SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
                    sr.sprite = Resources.Load<Sprite>("Balls/ball_2_" + (int)Random.Range(1, 5));
                }
            }
        }
        if(this.gameObject.name == "ball_2(Clone)")
        {
            GameManager.instace.score++;

            int powerBullet = (int)PlayerPrefs.GetFloat("powerBullet");
            ballLife -= powerBullet;
            lifeText.text = "" + ballLife;

            Debug.Log(powerBullet);

            if (ballLife <= 0)
            {
                Rigidbody2D cloneBall_1 = Instantiate(ball_3, this.gameObject.transform.position, Quaternion.identity);
                Rigidbody2D cloneBall_2 = Instantiate(ball_3, this.gameObject.transform.position, Quaternion.identity);

                int rd = (int)Random.Range(0, 1);

                if (randomTypeOfBall == 0)
                {
                    int rd2 = (int)Random.Range(1, 5);

                    SpriteRenderer sr1 = cloneBall_1.gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
                    SpriteRenderer sr2 = cloneBall_2.gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;

                    sr1.sprite = Resources.Load<Sprite>("Balls/ball_2_" + rd2);
                    sr2.sprite = Resources.Load<Sprite>("Balls/ball_2_" + rd2);
                }
                else
                {
                    int rd2 = (int)Random.Range(1, 4);

                    SpriteRenderer sr1 = cloneBall_1.gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
                    SpriteRenderer sr2 = cloneBall_2.gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;

                    sr1.sprite = Resources.Load<Sprite>("Balls/ball_" + rd2);
                    sr2.sprite = Resources.Load<Sprite>("Balls/ball_" + rd2);
                }

                cloneBall_1.bodyType = RigidbodyType2D.Dynamic;
                cloneBall_2.bodyType = RigidbodyType2D.Dynamic;

                cloneBall_1.velocity = new Vector2(60f * Time.deltaTime, 0f);
                cloneBall_2.velocity = new Vector2(-60f * Time.deltaTime, 0f);

                GenerateEnemies.instance.numberAliveBalls++;

                BallDestruction();
            }
            else
            {
                if (randomTypeOfBall == 1)
                {
                    SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
                    sr.sprite = Resources.Load<Sprite>("Balls/ball_" + (int)Random.Range(1, 4));
                }
                else
                {
                    SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
                    sr.sprite = Resources.Load<Sprite>("Balls/ball_2_" + (int)Random.Range(1, 5));
                }
            }
        }
        if (this.gameObject.name == "ball_3(Clone)")
        {
            GameManager.instace.score++;

            int powerBullet = (int)PlayerPrefs.GetFloat("powerBullet");
            ballLife -= powerBullet;
            lifeText.text = "" + ballLife;

            Debug.Log(powerBullet);

            if (ballLife <= 0)
            {
                Rigidbody2D cloneBall_1 = Instantiate(ball_4, this.gameObject.transform.position, Quaternion.identity);
                Rigidbody2D cloneBall_2 = Instantiate(ball_4, this.gameObject.transform.position, Quaternion.identity);

                if (randomTypeOfBall == 0)
                {
                    int rd2 = (int)Random.Range(1, 5);

                    SpriteRenderer sr1 = cloneBall_1.gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
                    SpriteRenderer sr2 = cloneBall_2.gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;

                    sr1.sprite = Resources.Load<Sprite>("Balls/ball_2_" + rd2);
                    sr2.sprite = Resources.Load<Sprite>("Balls/ball_2_" + rd2);
                }
                else
                {
                    int rd2 = (int)Random.Range(1, 4);

                    SpriteRenderer sr1 = cloneBall_1.gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
                    SpriteRenderer sr2 = cloneBall_2.gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;

                    sr1.sprite = Resources.Load<Sprite>("Balls/ball_" + rd2);
                    sr2.sprite = Resources.Load<Sprite>("Balls/ball_" + rd2);
                }

                cloneBall_1.bodyType = RigidbodyType2D.Dynamic;
                cloneBall_2.bodyType = RigidbodyType2D.Dynamic;

                cloneBall_1.velocity = new Vector2(60f * Time.deltaTime, 0f);
                cloneBall_2.velocity = new Vector2(-60f * Time.deltaTime, 0f);

                GenerateEnemies.instance.numberAliveBalls++;

            BallDestruction();
            }
            else
            {
                if (changeColor == 1)
                {
                    SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;

                    if (randomTypeOfBall == 0)
                    {
                        sr.sprite = Resources.Load<Sprite>("Balls/ball_2_" + (int)Random.Range(1, 5));
                    }
                    else
                    {
                        sr.sprite = Resources.Load<Sprite>("Balls/ball_" + (int)Random.Range(1, 4));
                    }
                }
            }
        }
        if (this.gameObject.name == "ball_4(Clone)")
        {
            GameManager.instace.score++;

            int powerBullet = (int)PlayerPrefs.GetFloat("powerBullet");
            ballLife -= powerBullet;
            lifeText.text = "" + ballLife;

            Debug.Log(powerBullet);

            SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;

            if (randomTypeOfBall == 0)
            {
                sr.sprite = Resources.Load<Sprite>("Balls/ball_2_" + (int)Random.Range(1, 5));
            }
            else
            {
                sr.sprite = Resources.Load<Sprite>("Balls/ball_" + (int)Random.Range(1, 4));
            }
            if (ballLife <= 0)
            {
                Rigidbody2D cloneBall_1 = Instantiate(money, this.gameObject.transform.position, Quaternion.identity);
                Rigidbody2D cloneBall_2 = Instantiate(money, this.gameObject.transform.position, Quaternion.identity);

                cloneBall_1.AddForce(new Vector2(-700f*Time.deltaTime,450f*Time.deltaTime));
                cloneBall_2.AddForce(new Vector2(700f * Time.deltaTime, 450f * Time.deltaTime));

                GenerateEnemies.instance.numberAliveBalls--;
                BallDestruction();
            }

        }
    }
    private void LifeTextOnBall()
    {
        if (this.gameObject.name == "ball_1(Clone)" || this.gameObject.name == "ball_2(Clone)")
        {
            Vector3 posLifeText = Camera.main.WorldToScreenPoint(new Vector3(gameObject.transform.position.x + -0.02f, transform.position.y + 0.23f, 0f));
            lifeText.gameObject.GetComponent<RectTransform>().position = posLifeText;
        }
        if (this.gameObject.name == "ball_3(Clone)")
        {
            Vector3 posLifeText = Camera.main.WorldToScreenPoint(new Vector3(gameObject.transform.position.x + -0.02f, transform.position.y + 0.16f, 0f));
            lifeText.gameObject.GetComponent<RectTransform>().position = posLifeText;
        }
        if (this.gameObject.name == "ball_4(Clone)")
        {
            Vector3 posLifeText = Camera.main.WorldToScreenPoint(new Vector3(gameObject.transform.position.x + -0.02f, transform.position.y + 0.1f, 0f));
            lifeText.gameObject.GetComponent<RectTransform>().position = posLifeText;
        }
    }
    private void BallDestruction()
    {
        GameManager.instace.BallDestructionSound();

        gameObject.GetComponent<Animator>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 5;
        gameObject.GetComponent<Animator>().SetBool("explosion",true);
        lifeText.enabled = false;
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        Destroy(gameObject.GetComponent<CircleCollider2D>());
        Destroy(gameObject.GetComponent<Enemies>());
        Destroy(gameObject,1.25f);
    }
}
