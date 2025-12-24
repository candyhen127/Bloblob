using UnityEngine;

public class Gate : MonoBehaviour
{

    private Transform t;
    Vector3 temp;
    Quaternion rottemp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        t = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void toPostionX(float x) {
        temp = t.position;
        temp.x = x;
        t.position = temp;
    }

    public void toPositionY(float y) {
        temp = t.position;
        temp.y = y;
        t.position = temp;
    }

    public void toRotationCW(float z) {
        rottemp = t.rotation;
        rottemp.z = z;
        t.rotation = rottemp;
    }

    void toRotationCCW(float z) {
    }

    //Change these to be smooth movements later
}
