using UnityEngine;
using NaughtyAttributes;

public class DestroyTimed : MonoBehaviour
{
    public bool startOnAwake = true;

    public float time = 3;

    private bool running = false;
    private float timerTime = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (startOnAwake)
            running = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!running)
            return;

        timerTime += Time.deltaTime;

        if(timerTime >= time)
            Destroy(gameObject);
    }

    [Button]
    public void StartTimer()
    {
        running = true;
    }
}

