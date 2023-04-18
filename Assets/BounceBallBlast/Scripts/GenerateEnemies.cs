using UnityEngine;

public class GenerateEnemies : MonoBehaviour {

    public static GenerateEnemies instance { get; set; }
    public Transform[] ballsPrefab;
    public Rigidbody2D[] ballTypes;
    public float leftPosSide;
    public float rightPosSide;

    private Transform clone;
    private float whichSide;
    private float newPosition;
    private bool stopTranslate;
    private int n;

    public int numberAliveBalls;
    public int randomTypeOfBall;

    void Start ()
    {
        instance = this;
        stopTranslate = false;

        int rand = Random.Range(0,ballsPrefab.Length-1);
        ballsPrefab[rand].gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        GenerateEnemy(ballsPrefab[rand], leftPosSide, rightPosSide);
    }
	
	void FixedUpdate ()
    {
        if (!stopTranslate && clone != null)
        {
            if (whichSide == leftPosSide)
            {
                if (newPosition > clone.transform.position.x)
                    clone.transform.Translate(new Vector3(1f * Time.deltaTime, 0, 0));
                else
                {
                    if (clone.gameObject.GetComponent<Rigidbody2D>() != null)
                    {
                        clone.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                        stopTranslate = true;
                    }
                }
            }
            else if (newPosition < clone.transform.position.x)
            {
                clone.transform.Translate(new Vector3(-1f * Time.deltaTime, 0, 0));
            }
            else
            {
                if (clone.gameObject.GetComponent<Rigidbody2D>() != null)
                {
                    clone.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    stopTranslate = true;
                }
            }
        }
        if(numberAliveBalls <= n)
        {
            int rand = Random.Range(0, ballsPrefab.Length - 1);
            ballsPrefab[rand].gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

            if (randomTypeOfBall == 1)
            {
                SpriteRenderer sr = ballsPrefab[rand].gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
                sr.sprite = Resources.Load<Sprite>("Balls/ball_" + (int)Random.Range(1, 4));
            }
            else
            {
                SpriteRenderer sr = ballsPrefab[rand].gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
                sr.sprite = Resources.Load<Sprite>("Balls/ball_2_" + (int)Random.Range(1, 5));
            }

            GenerateEnemy(ballsPrefab[rand], leftPosSide, rightPosSide);

            stopTranslate = false;
        }
    }
    private void GenerateEnemy(Transform enemyPrefab,float leftPosSide,float rightPosSide)
    {
        float rand = Random.Range(0f,10f);

        if (rand < 5f)
            whichSide = leftPosSide;
        else
            whichSide = rightPosSide;


        Vector3 newPos = new Vector3(whichSide, Random.Range(4.323f, 1.102f), 0f);

        clone = Instantiate(enemyPrefab,newPos,Quaternion.identity);

        randomTypeOfBall = (int)Random.Range(0,2);

        Debug.Log(randomTypeOfBall);

        if (randomTypeOfBall == 0)
        {
            SpriteRenderer sr = clone.gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            sr.sprite = Resources.Load<Sprite>("Balls/ball_2_" + (int)Random.Range(1, 5));
        }
        if (randomTypeOfBall == 1)
        {
            SpriteRenderer sr = clone.gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            sr.sprite = Resources.Load<Sprite>("Balls/ball_" + (int)Random.Range(1, 4));
        }

        numberAliveBalls++;

        if (whichSide == leftPosSide)
            newPosition = whichSide + 2f;
        else
            newPosition = whichSide - 2f;
    }
    private bool CheckIfGenerateNewBall()
    {
        return true;
    }
}
