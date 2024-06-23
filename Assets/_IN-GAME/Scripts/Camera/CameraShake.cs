using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{

    private CinemachineVirtualCamera cam;
    private CinemachineBasicMultiChannelPerlin noise;
    
    private float shakeTimer = 0f,shakeTimerTotal = 0f, startingIntensity = 0f;
   
    private bool endSmoothly = false, IsShaking = false;

    private void Awake()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }


    public void ShakeCamera(float intensity, float time, bool endSmoothly = false)
    {
        noise.m_AmplitudeGain = intensity;

        shakeTimer = time;
        shakeTimerTotal = time;

        startingIntensity = intensity;

        this.endSmoothly = endSmoothly;


    }


    private void Update()
    {
        if (shakeTimer > 0f && !IsShaking)
        {
            Debug.Log("Shaking");
            IsShaking = true;
            shakeTimer -= Time.deltaTime;

            if (shakeTimer <= 0)
            {
                CinemachineBasicMultiChannelPerlin noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                if (!endSmoothly)
                    noise.m_AmplitudeGain = 0f;
                else
                    noise.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0, shakeTimer / shakeTimerTotal);
            }

            IsShaking = false;
            //Debug.Log("Inside update");

        }
    }
}
