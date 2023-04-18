using UnityEngine;

public class Player : MonoBehaviour {
    public static Player instance {get;set;}

    public Rigidbody2D prefabRocket;
    public Rigidbody2D prefabRocket5;
    public Transform posRocket;
    public float forceBullet;
    public float rateBulletTime;

    private float time;
    private float force;

    [HideInInspector] public bool activeBulletCannon5;

    Vector2 touchPos;


    void Start ()
    {
        instance = this;

        forceBullet = PlayerPrefs.GetFloat("speedBullet");

        if (forceBullet < 1000f)
            forceBullet = 2000f;

        string activeCannon = PlayerPrefs.GetString("active_cannon","null");

        if(activeCannon == "null")
            activeCannon = "cannon_1";

        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
        sr.sprite = Resources.Load<Sprite>("Cannons/" + activeCannon);
	}
	
	void FixedUpdate ()
    {
        if (Input.GetMouseButton(0))
        {
            Move();

            if (time < Time.time)
            {
                StartCoroutine(GameManager.instace.CannonShootSound());
                Shoot(prefabRocket,prefabRocket5, posRocket, forceBullet);
                time = Time.time + rateBulletTime;
            }
        }
	}
    private void Move()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.touchCount > 0)
        {
            touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        }
        else
            touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 initialPos = transform.position;

        float t = 0.25f;

        transform.position = new Vector3(Mathf.Lerp(initialPos.x, touchPos.x, t), transform.position.y, transform.position.z);

        Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);

        if (playerScreenPos.x <= 0f)
            transform.position = new Vector3(-2.44f, transform.position.y,0f);
        if(playerScreenPos.x >= Screen.width)
            transform.position = new Vector3(2.44f, transform.position.y, 0f);
    }
    private void Shoot(Rigidbody2D prefabRocket,Rigidbody2D bullet5,Transform posRocket,float force)
    {
        if (!activeBulletCannon5)
        {
            Rigidbody2D clone = Instantiate(prefabRocket, posRocket.position, Quaternion.identity);
            clone.AddForce(transform.up * force * Time.deltaTime);


            Destroy(clone.gameObject, 1f);
        }
        else
        {
            Rigidbody2D clone = Instantiate(bullet5, posRocket.position, Quaternion.identity);
            clone.AddForce(transform.up * force * Time.deltaTime);


            Destroy(clone.gameObject, 1f);
        }
    }
}
