using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField]
    private int maxAmmo;
    [SerializeField]
    TextMeshProUGUI ammoCounter;

    [SerializeField]
    private List<GameObject> blobTypes;

    [SerializeField]
    private List<GameObject> shotBlobs;

    [SerializeField]
    private SpriteRenderer playerSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ammo = maxAmmo;
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
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            bulletPrefab = blobTypes[0];
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            bulletPrefab = blobTypes[1];
        }

        if(Input.GetButtonDown("Fire1"))
        {
            if(ammo - bulletPrefab.GetComponent<Blob>().ammoCost >= 0)
            {
                shoot();
            }
        }
        if(Input.GetKeyDown("r")) {
            foreach(GameObject b in shotBlobs) {
                Destroy(b);
            }
            ammo = maxAmmo;
        }
        ammoCounter.text = ammo.ToString() + "/" + maxAmmo.ToString();
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
            playerSprite.flipX = true;
        }
        else if (pangle <=90 || pangle >= -90)
        {
            rb.position = rightHand.position;
            playerSprite.flipX = false;
        }
    }

    void shoot() {
        GameObject blob = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        blob.GetComponent<Rigidbody2D>().linearVelocity = shootPoint.up * 10;
        shotBlobs.Add(blob);
        ammo -= bulletPrefab.GetComponent<Blob>().ammoCost;
    }
}
