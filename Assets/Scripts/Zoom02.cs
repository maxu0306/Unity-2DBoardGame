using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom02 : MonoBehaviour
{
    Transform tf; // Main Camera��Transform
    Camera cam;    // Main Camera��Camera
    private Vector3 touchStartPos;
    private float initialOrthographicSize;

    void Start()
    {
        tf = this.gameObject.GetComponent<Transform>(); // Main Camera��Transform���擾����B
        cam = this.gameObject.GetComponent<Camera>();     // Main Camera��Camera���擾����B
        initialOrthographicSize = cam.orthographicSize;  // ������Orthographic Size��ۑ�
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.I) || Input.touchCount == 2)
        {
            // �Y�[���C��
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - 1.0f, 10.0f, initialOrthographicSize * 2);
        }
        else if (Input.GetKey(KeyCode.O) || Input.touchCount == 3)
        {
            // �Y�[���A�E�g
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + 1.0f, 10.0f, initialOrthographicSize * 2);
        }

        // �^�b�`�W�F�X�`���[�ɂ��Y�[������
        HandleZoomGesture();
        SimulateTouchpadInput();
    }

    void HandleZoomGesture()
    {
        if (Input.touchCount == 2)
        {
            // 2�{�̎w�Ń^�b�v���Ă���ꍇ
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

            float prevMagnitude = (touch0PrevPos - touch1PrevPos).magnitude;
            float currentMagnitude = (touch0.position - touch1.position).magnitude;

            float deltaMagnitude = currentMagnitude - prevMagnitude;

            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - deltaMagnitude * 0.5f, 1.0f, initialOrthographicSize * 2);
        }
    }

    void SimulateTouchpadInput()
    {
        if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchStartPos = Input.mousePosition;
            }
            else
            {
                Vector3 touchDelta = touchStartPos - Input.mousePosition;
                // �X�N���[������
                cam.transform.Translate(touchDelta * 0.01f);
                touchStartPos = Input.mousePosition;
            }
        }
    }
}
