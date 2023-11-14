using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public GameObject redCursorPrefab; // �ԃJ�[�\���̃v���n�u
    private GameObject currentRedCursor; // ���ݕ\������Ă���ԃJ�[�\��

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���N���b�N�������ꂽ�ꍇ
        {
            // �N���b�N�����ʒu�̍��W���擾
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int clickedX = Mathf.RoundToInt(clickPosition.x);
            int clickedY = Mathf.RoundToInt(clickPosition.y);

            // �����̐ԃJ�[�\�����폜
            DestroyCurrentRedCursor();

            // �N���b�N�����I�u�W�F�N�g�����擾
            RaycastHit2D hit2D = Physics2D.Raycast(clickPosition, Vector2.down);
            
            if (hit2D.collider != null)
            {
                SetPosition(hit2D.transform);
                GameObject clickedObject = hit2D.collider.gameObject;

                // �N���b�N�����I�u�W�F�N�g�̃X�v���C�g��R���|�[�l���g�ɃA�N�Z�X���邱�Ƃ��\�ł�
                SpriteRenderer spriteRenderer = clickedObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    Sprite clickedSprite = spriteRenderer.sprite;
                    Debug.Log("�I�u�W�F�N�g���: " + clickedSprite.name + " �I�u�W�F�N�g���W: (" + clickedX + ", " + clickedY + ")");
                }
                // �N���b�N�ʒu�ɃJ�[�\����\��
                Vector3 cursorPosition = new Vector3(clickedX, clickedY, 0);
                currentRedCursor = Instantiate(redCursorPrefab, hit2D.transform.position, Quaternion.identity);
            }
            else
            {
                SetPosition(hit2D.transform);
                GameObject clickedObject = hit2D.collider.gameObject;
                // �v���n�u�Ɋւ��鏈����ǉ�
                Debug.Log(" �I�u�W�F�N�g���W: (" + clickedX + ", " + clickedY + ")");
                // �N���b�N�ʒu�ɃJ�[�\����\��
                Vector3 cursorPosition = new Vector3(clickedX, clickedY, 0);
                currentRedCursor = Instantiate(redCursorPrefab, hit2D.transform.position, Quaternion.identity);
            }
        }
    }

    private void SetPosition(Transform target)
    {
        transform.position = target.position;
    }

    private void DestroyCurrentRedCursor()
    {
        if (currentRedCursor != null)
        {
            Destroy(currentRedCursor);
        }
    }
}
