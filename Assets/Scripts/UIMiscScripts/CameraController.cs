using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 positionNew = transform.position + 4f * Time.deltaTime / GameSpeed.instance.speed * (Vector3)input;

        positionNew = new Vector3(Mathf.Clamp(positionNew.x, 0, 32), Mathf.Clamp(positionNew.y, 0, 32), -10);

        transform.position = positionNew;
    }
}
