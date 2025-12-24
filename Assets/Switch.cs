using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{
    bool isActivated;
    SpriteRenderer renderer;
    
    [SerializeField] UnityEvent onSwitchActivated; // Events to trigger when activated
    [SerializeField] UnityEvent onSwitchDeactivated; // Events to trigger when deactivated

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "Blob" || other.gameObject.tag == "Player") {
            if (isActivated) {
                onSwitchDeactivated.Invoke();
                isActivated = false;
                renderer.flipY = false;
            }
            else {
                
                onSwitchActivated.Invoke();
                isActivated = true;
                renderer.flipY = true;
            }
        }
    }
}
