using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 500f;

    private int currentCamPos = 0;


    Camera mainCam;
    Vector3 movement;
    Rigidbody rb;
    Vector3[] positionV3s = new Vector3[4];

    bool cameraReady;


    [SerializeField] float cameraDistanceOffset = 15f;
    [SerializeField] float cameraHeightOffset = 5f;

   
    private void Awake()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody>();

        //add camera position vectors to the array
        positionV3s[0] = new Vector3(0, cameraHeightOffset, -cameraDistanceOffset);
        positionV3s[1] = new Vector3(cameraDistanceOffset, cameraHeightOffset, 0);
        positionV3s[2] = new Vector3(0, cameraHeightOffset, cameraDistanceOffset);
        positionV3s[3] = new Vector3(-cameraDistanceOffset, cameraHeightOffset, 0);


        cameraReady = true;

    }

    private void FixedUpdate()
    {
        rb.AddForce(movement * speed * Time.fixedDeltaTime);
    }


    private void CameraRelativeMovement()
    {
        //placeholder
    }

    public void OnRun(InputValue value)
    {
        Vector3 forward = mainCam.transform.forward;
        Vector3 right = mainCam.transform.right;
      
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;
        



        //Debug.Log("moving...");
        //Debug.Log("forward: " + forward);
        //Debug.Log("right: " + right);

        movement = value.Get<Vector3>();

        float forwardInput = movement.z;
        float rightInput = movement.x;


        //Debug.Log(movement);

        movement = forwardInput * forward + rightInput * right;
        //Debug.Log("move: " + movement);
    }


    public void RotateCamera(bool direction)
    {
        Quaternion tempRot = mainCam.transform.rotation;
        Vector3 tempPos = mainCam.transform.localPosition;
        Quaternion targetRot;
        Vector3 targetPos;
        //we're passing in a bool right now because it's more efficient than a string
        //probably not necessary because this doesn't get called very often

        //first, check if ready (to see if already rotating)
        if (cameraReady)
        {
            cameraReady = false;
            //if direction is true, we rotate left; if direction is false, we rotate right.
            if (direction)
            {
                currentCamPos--;
                if (currentCamPos < 0) currentCamPos = positionV3s.Length - 1;

                //UpdateCameraPosition(positionV3s[currentCamPos]);

                targetRot = Quaternion.Euler(mainCam.transform.rotation.eulerAngles + new Vector3(0, 90f, 0));
                //targetPos = positionV3s[currentCamPos];

                //coroutine is working, but needs to be locked so that another coroutine is not starting while this one is still running

                //StartCoroutine(CameraSlerp(tempRot, targetRot, tempPos, targetPos));
                //mainCam.transform.rotation = Quaternion.Euler(mainCam.transform.rotation.eulerAngles + new Vector3(0, 90f, 0));

            }
            else
            {
                currentCamPos++;
                if (currentCamPos > positionV3s.Length - 1) currentCamPos = 0;

                //UpdateCameraPosition(positionV3s[currentCamPos]);
                targetRot = Quaternion.Euler(mainCam.transform.rotation.eulerAngles + new Vector3(0, -90f, 0));
                //targetPos = positionV3s[currentCamPos];

                //mainCam.transform.rotation = Quaternion.Euler(mainCam.transform.rotation.eulerAngles + new Vector3(0, -90f, 0));

            }

            targetPos = positionV3s[currentCamPos];
            StartCoroutine(CameraSlerp(tempRot, targetRot, tempPos, targetPos));
        }
    }

    /*
    private void UpdateCameraPosition(Vector3 position)
    {
        mainCam.transform.localPosition = position;
    }
    */


    private IEnumerator CameraSlerp(Quaternion rot1, Quaternion rot2, Vector3 pos1, Vector3 pos2)
    {
        float timeCount = 0.0f;
        while(timeCount < 0.3f)
        {
            mainCam.transform.rotation = Quaternion.Slerp(rot1, rot2, timeCount * 5);
            mainCam.transform.localPosition = Vector3.Slerp(pos1, pos2, timeCount * 5);
            timeCount += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        mainCam.transform.rotation = rot2;
        mainCam.transform.localPosition = pos2;
        cameraReady = true;
        yield return null;


    }

    public void OnRotateCameraLeft()
    {
        RotateCamera(true);

    }

    public void OnRotateCameraRight()
    {
        RotateCamera(false);
    }

}
