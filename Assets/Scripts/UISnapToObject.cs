using UnityEngine;

public class UISnapToObject : MonoBehaviour
{
    [SerializeField] private GameObject obj;
    [SerializeField] private RectTransform rb;
    
    private Transform t;
    
    private Vector3 screenPos;

    void Start()
    {
        t = obj.GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        screenPos = Camera.main.WorldToScreenPoint(t.position);
        rb.position = screenPos;
    }
}
