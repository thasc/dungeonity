using System;
using UnityEngine;

public class WsadCamera : MonoBehaviour
{
    public float PanSpeed = 0.1f;
    public float ScrollSpeed = 0.1f;
    
    private Camera m_Camera;

    private void Awake()
    {
        m_Camera = GetComponent<Camera>() ?? throw new ArgumentNullException(nameof(Camera));
    }

    private void Update()
    {
        HandlePan();
        HandleScroll();
    }

    private void HandlePan()
    {
        var goUp = Input.GetKey(KeyCode.W);
        var goDown = Input.GetKey(KeyCode.S);
        var goLeft = Input.GetKey(KeyCode.A);
        var goRight = Input.GetKey(KeyCode.D);

        var movementVector = Vector3.zero;

        if (goUp) movementVector += Vector3.up;
        if (goDown) movementVector += Vector3.down;
        if (goLeft) movementVector += Vector3.left;
        if (goRight) movementVector += Vector3.right;

        m_Camera.transform.position += movementVector * (PanSpeed * m_Camera.orthographicSize);
    }

    private void HandleScroll()
    {
        var scroll = Input.mouseScrollDelta.y * ScrollSpeed;
        var currentSize = m_Camera.orthographicSize;
        m_Camera.orthographicSize = Math.Max(1f, currentSize - (currentSize * scroll));
    }
}
