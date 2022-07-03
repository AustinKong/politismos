using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool _shaking;

    public static CameraController instance;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 positionNew = transform.position + 4f * Time.deltaTime / GameSpeed.instance.speed * (Vector3)input;

        positionNew = new Vector3(Mathf.Clamp(positionNew.x, 0, 32), Mathf.Clamp(positionNew.y, 0, 32), -10);

        transform.position = positionNew;
    }

    public void ShakeCamera()
    {
        if (_shaking == true) return;
        StartCoroutine(ShakingCamera());
    }

    private IEnumerator ShakingCamera(float magnitude = 0.1f)
    {
        float cog = GameSpeed.instance.speed;

        Vector3 _initCamPos = transform.position;

        _shaking = true;

        float t = 0f, x, y;
        while (t < 0.2f)
        {
            x = Random.Range(-1f, 1f) * magnitude;
            y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(_initCamPos.x + x, _initCamPos.y + y, _initCamPos.z);

            t += 0.05f/cog;
            yield return new WaitForSeconds(0.05f);
        }

        transform.position = _initCamPos;
        _shaking = false;
    }
}
