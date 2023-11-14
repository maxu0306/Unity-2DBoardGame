using UnityEngine;
using System.Collections;

public class Zoom01 : MonoBehaviour
{

    //�J�����̐ݒ�
    private GameObject Object;
    private Camera cam;

    //�J�����͈͂̐ݒ�
    public float maxCamSize = 9.0f;
    public float minCamSize = 3.0f;
    public float maxCamX = 81.0f / 16.0f;
    public float minCamX = -81.0f / 16.0f;
    public float maxCamY = 9.0f;
    public float minCamY = -9.0f;

    //���O�̎w���m�̋���
    private float lastDist = 0.0f;

    //�w���m�̒��S���W
    private Vector2 centerPos;

    private Vector3 t1;
    private Vector3 t;

    //��{�w�^�b�`�������[���h���W
    private Vector2 lastTouchWorld;
    private Vector2 nowTouchWorld;

    //���O�̐G��Ă���w�̐�
    private int lastCount = 0;

    //�X�s�[�h����
    public float zoomSpeed = 1.0f;
    public float moveSpeed = 1.0f;

    //�J�����T�C�Y�̍���
    float sizeDiff;

    //�J��������ʊO���f���Ȃ��悤�ɏ���
    void CameraSlide()
    {
        if (cam.transform.position.x > maxCamX - cam.orthographicSize * 9 / 16)
        {
            cam.transform.position = new Vector3(maxCamX - cam.orthographicSize * 9 / 16, cam.transform.position.y, cam.transform.position.z);
        }
        if (cam.transform.position.x < minCamX + cam.orthographicSize * 9 / 16)
        {
            cam.transform.position = new Vector3(minCamX + cam.orthographicSize * 9 / 16, cam.transform.position.y, cam.transform.position.z);
        }
        if (cam.transform.position.y > maxCamY - cam.orthographicSize)
        {
            cam.transform.position = new Vector3(cam.transform.position.x, maxCamY - cam.orthographicSize, cam.transform.position.z);
        }
        if (cam.transform.position.y < minCamY + cam.orthographicSize)
        {
            cam.transform.position = new Vector3(cam.transform.position.x, minCamY + cam.orthographicSize, cam.transform.position.z);
        }

    }

    void Start()
    {
        // �J������ݒ�
        Object = GameObject.Find("Main Camera");
        cam = Object.GetComponent<Camera>();
        cam.orthographicSize = 9f;

        // �J�����̏����ʒu��ݒ�
        cam.transform.position = new Vector3(7f, 7f, cam.transform.position.z);
    }


    void Update()
    {
        //�^�b�`��0
        if (Input.touchCount == 0)
        {
            lastCount = 0;
        }
        //�X���C�v����i���O��touchCount��1�ȉ��ɂ��邱�ƂŃs���`�����ɔ������Ȃ��悤�ɂ��Ă��܂��B�j
        else if (Input.touchCount == 1 && lastCount <= 1)
        {
            //�^�b�`�ʒu�擾
            Touch touch3 = Input.GetTouch(0);

            //�G�ꂽ�Ƃ�
            if (touch3.phase == TouchPhase.Began)
            {
                t = touch3.position;
                lastTouchWorld = cam.ScreenToWorldPoint(t);
            }
            //��������
            else if (touch3.phase == TouchPhase.Moved)
            {
                
                //�O��̍��W��ۑ�
                nowTouchWorld = lastTouchWorld;
                //���̍��W��ۑ�
                t = touch3.position;
                nowTouchWorld = cam.ScreenToWorldPoint(t);
                //�J�������ړ�
                cam.transform.position = new Vector3(cam.transform.position.x + (lastTouchWorld.x - nowTouchWorld.x) * moveSpeed, cam.transform.position.y + (lastTouchWorld.y - nowTouchWorld.y) * moveSpeed, cam.transform.position.z);

                CameraSlide();
                
            }

            lastCount = 1;
        }
        //�s���`����
        else if (Input.touchCount >= 2)
        {
            //�^�b�`�ʒu�擾
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            //��{�ڂ��G�ꂽ�Ƃ�
            if (touch2.phase == TouchPhase.Began)
            {
                //�w���m�̋����ƒ��S���W���擾
                lastDist = Vector2.Distance(touch1.position, touch2.position);
                centerPos = (touch1.position + touch2.position) * 0.5f;
                centerPos = cam.ScreenToWorldPoint(centerPos);
            }

            //�ǂ��炩�̎w����������
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                //�w���m�̋������v�Z
                float newDist = Vector2.Distance(touch1.position, touch2.position);

                //�Y�[������
                sizeDiff = cam.orthographicSize * cam.orthographicSize * (newDist - lastDist) / 10000.0f * zoomSpeed;
                cam.orthographicSize -= sizeDiff;


                //�J�������g��i�k���j���������ꍇ�̐���
                if (cam.orthographicSize > maxCamSize)
                {
                    cam.orthographicSize = maxCamSize;
                }
                else if (cam.orthographicSize < minCamSize)
                {
                    cam.orthographicSize = minCamSize;
                }
                //�w���m�̒��S�ʒu���s���`����̒��S�ƂȂ�悤�ɃJ�����𕽍s�ړ�
                else
                {
                    cam.transform.position = new Vector3(centerPos.x + cam.orthographicSize / (cam.orthographicSize + sizeDiff) * (cam.transform.position.x - centerPos.x), centerPos.y + cam.orthographicSize / (cam.orthographicSize + sizeDiff) * (cam.transform.position.y - centerPos.y), cam.transform.position.z);
                }
                CameraSlide();
            }
            lastCount = 2;
        }
    }
}