using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    [SerializeField]
    bool lockXZ = false;
    // Update is called once per frame
    void Update()
    {
        if (lockXZ) {
            transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
        } else
        {
            transform.rotation = Camera.main.transform.rotation;

        }
    }
}
