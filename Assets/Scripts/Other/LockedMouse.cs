using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedMouse : MonoBehaviour
{

    void Start()
    {
        //��� ������� ����, ������ �������� � �� ������ (� ����� ��� ����� ������� ��� (ESC/���))
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //CursorUnLock();
    }

    private void CursorUnLock()
    {
        //��� ������� "������", ������ ������� ���������� � ��������������, ������� ��� EscMenu
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
