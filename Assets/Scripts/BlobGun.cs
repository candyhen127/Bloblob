using UnityEngine;

public class BlobGun : MonoBehaviour
{

    Vector2 mousePos;

    [SerializeField]
    private Transform rightHand;
    [SerializeField]
    private Transform leftHand;
    [SerializeField]
    private Transform shootPoint;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform center;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private SpriteRenderer sprite;
    
    [SerializeField]
    private float pangle;
    [SerializeField] 
    private float angle;

    [SerializeField] 
    private int ammo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if(pangle > 90 || pangle < -90)
        {
            sprite.flipY = true;
        }
        else if(pangle <=90 || pangle >= -90)
        {
            sprite.flipY = false;
        }
        
        
        if(Input.GetButtonDown("Fire1"))
        {
            if(ammo > 0)
            {
                shoot();
            }
        }
    }

    void FixedUpdate()
    {
        
        Vector2 direction = mousePos - rb.position;
        angle = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;
        //Vector2 centerPos = (center.x, center.y);
        Vector2 pdirection;
        pdirection.x = mousePos.x - center.position.x;
        pdirection.y = mousePos.x - center.position.y;
        pangle = Mathf.Atan2(pdirection.y, pdirection.x)*Mathf.Rad2Deg;

        rb.rotation = angle;
        if(pangle > 90 || pangle < -90)
        {
            rb.position = leftHand.position;
        }
        else if (pangle <=90 || pangle >= -90)
        {
            rb.position = rightHand.position;
        }
    }

    void shoot() {
        GameObject blob = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        blob.GetComponent<Rigidbody2D>().linearVelocity = shootPoint.up * 10;
    }
}
