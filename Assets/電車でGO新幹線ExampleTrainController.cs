using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class 電車でGO新幹線ExampleTrainController : MonoBehaviour
{
    [SerializeField]
    protected Transform train;

    [SerializeField]
    protected Camera trainCamera;
    protected Vector3 initialCameraRotation;
    protected Vector3 initialCameraPosition;

    [SerializeField]
    protected float powerScale = 1.0f;
    [SerializeField]
    protected float power = 0.0f;
    [SerializeField]
    protected float brakeScale = 1.0f;
    [SerializeField]
    protected float brake = 0.0f;

    [SerializeField]
    protected float speedDisplayScale = 1.0f;
    protected float speed = 0.0f;

    [SerializeField]
    protected ParticleSystem smokeStack;

    protected bool openSmokeStack = false;

    void Start()
    {
        initialCameraPosition = trainCamera.transform.position;
        initialCameraRotation = trainCamera.transform.eulerAngles;

        smokeStack.Stop();
    }

    public void Update()
    {
        train.Translate(Vector3.forward * speed * Time.deltaTime);

        speed += power * powerScale * Time.deltaTime;
        speed -= brake * brakeScale * Time.deltaTime;

        if (speed < 0)
        {
            speed = 0;
        }

        if (電車でGO.電車でGO新幹線Device.current != null)
        {
            var speedDisplay = (int)(speed * speedDisplayScale);
            if (speedDisplay < 0)
            {
                speedDisplay = 0;
            }
            電車でGO.電車でGO新幹線Device.current.SetSpeedDisplay(speedDisplay);
        }
    }

    public void SetCameraViewForward()
    {
        trainCamera.transform.eulerAngles = initialCameraRotation;
    }

    public void SetCameraViewBackward()
    {
        trainCamera.transform.eulerAngles = initialCameraRotation + new Vector3(0, 180, 0);
    }

    public void SetCameraViewLeft()
    {
        trainCamera.transform.eulerAngles = initialCameraRotation + new Vector3(0, 90, 0);
    }

    public void SetCameraViewRight()
    {
        trainCamera.transform.eulerAngles = initialCameraRotation + new Vector3(0, 270, 0);
    }

    public void SetCameraPositionA()
    {
        trainCamera.transform.position = initialCameraPosition;
        trainCamera.transform.eulerAngles = initialCameraRotation;
    }

    public void SetCameraPositionB()
    {
        trainCamera.transform.position = initialCameraPosition + new Vector3(0, 0.5f, 0);
        trainCamera.transform.eulerAngles = initialCameraRotation + new Vector3(-45, 0, 0);
    }

    public void SetCameraPositionC()
    {
        trainCamera.transform.position = initialCameraPosition + new Vector3(0.5f, 0, 0);
        trainCamera.transform.eulerAngles = initialCameraRotation + new Vector3(0, 90, 0);
    }

    public void SetCameraPositionD()
    {
        trainCamera.transform.position = initialCameraPosition + new Vector3(-0.5f, 0, 0);
        trainCamera.transform.eulerAngles = initialCameraRotation + new Vector3(0, -90, 0);
    }

    public void OnBrake(InputAction.CallbackContext context)
    {
        SetBrake(context.ReadValue<float>());
    }

    public void SetBrake(float brake)
    {
        this.brake = brake;

        if (電車でGO.電車でGO新幹線Device.current != null)
        {
            電車でGO.電車でGO新幹線Device.current.SetSmallSegmentBar((int)(brake * 電車でGO.新幹線専用コントローラ.SmallSegmentBarMaximum));
        }
    }

    public void OnPower(InputAction.CallbackContext context)
    {
        SetPower(context.ReadValue<float>());
    }

    public void SetPower(float power)
    {
        this.power = Math.Abs(power);

        if (電車でGO.電車でGO新幹線Device.current != null)
        {
            電車でGO.電車でGO新幹線Device.current.SetLargeSegmentBar((int)(power * 電車でGO.新幹線専用コントローラ.LargeSegmentBarMaximum));
        }
    }

    public void ToggleSmokeStack()
    {
        openSmokeStack = !openSmokeStack;

        if (smokeStack != null)
        {
            if (openSmokeStack)
            {
                smokeStack.Play();
            }
            else
            {
                smokeStack.Stop();
            }
        }

        if (電車でGO.電車でGO新幹線Device.current != null)
        {
            電車でGO.電車でGO新幹線Device.current.SetDoorsClosedLight(openSmokeStack);
        }
    }
}
