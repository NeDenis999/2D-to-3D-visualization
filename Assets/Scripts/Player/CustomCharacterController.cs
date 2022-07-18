using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���������� ����� �������� ������� � �������� ������ ���������
public class CustomCharacterController : MonoBehaviour
{

    [Header("��������� �������")]
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private float staminaValue;
    [SerializeField] private float minValueStamina;
    [SerializeField] private float maxValueStamina;
    [SerializeField] private float staminaReturn;
    //private Text staminaText;

    [Header("������ �� �������")]
    public Animator anim;
    public Rigidbody rig;
    public Transform mainCamera;

    [Header("��������� ���������")]
    public float jumpForce = 3.5f;
    public float walkingSpeed = 2f;
    public float runningSpeed = 6f;
    public float stamina0_speed = 2f; 
    public float currentSpeed;
    private float animationInterpolation = 1f;
    //public Collider playerCollider;

    void Start()
    {
        // ����������� ������ � �������� ������
        Cursor.lockState = CursorLockMode.Locked;
        // � ������ ��� ���������
        Cursor.visible = false;
    }
    void Run()
    {
        animationInterpolation = Mathf.Lerp(animationInterpolation, 1.5f, Time.deltaTime * 3);
        anim.SetFloat("x", Input.GetAxis("Horizontal") * animationInterpolation);
        anim.SetFloat("y", Input.GetAxis("Vertical") * animationInterpolation);

        currentSpeed = Mathf.Lerp(currentSpeed, runningSpeed, Time.deltaTime * 3);
        staminaValue -= staminaReturn * Time.deltaTime * 5;

    }
    void Walk()
    {
        // Mathf.Lerp - ������� �� ��, ����� ������ ���� ����� animationInterpolation(� ������ ������) ������������ � ����� 1 �� ��������� Time.deltaTime * 3.
        // Time.deltaTime - ��� ����� ����� ���� ������ � ���������� ������. ��� ��������� ������ ���������� � ������ ����� �� ������� ���������� �� ������ � ������� (FPS)!!!
        animationInterpolation = Mathf.Lerp(animationInterpolation, 1f, Time.deltaTime * 3);
        anim.SetFloat("x", Input.GetAxis("Horizontal") * animationInterpolation);
        anim.SetFloat("y", Input.GetAxis("Vertical") * animationInterpolation);

        currentSpeed = Mathf.Lerp(currentSpeed, walkingSpeed, Time.deltaTime * 3);
        staminaValue += staminaReturn * Time.deltaTime * 2;
    }
    private void Update()
    {
        Stamina();

        // ������������� ������� ��������� ����� ������ �������������� 
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, mainCamera.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        // ������ �� ������ W � Shift?
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift) && staminaValue > 0)
        {
            // ������ �� ��� ������ A S D?
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                // ���� ��, �� �� ���� ������
                Walk();
            }
            // ���� ���, �� ����� �����!
            else
            {
                Run();
            }
        }
        // ���� W & Shift �� ������, �� �� ������ ���� ������
        else
        {
            Walk();
        }
        //���� ����� ������, �� � ��������� ���������� ��������� �������, ������� ���������� �������� ������
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Jump");
            Jump();    
        }*/
    }

    void FixedUpdate()
    {
        // ����� �� ������ �������� ��������� � ����������� �� ����������� � ������� ������� ������
        // ��������� ����������� ������ � ������ �� ������ 
        Vector3 camF = mainCamera.forward;
        Vector3 camR = mainCamera.right;
        // ����� ����������� ������ � ������ �� �������� �� ���� ������� �� ������ ����� ��� ����, ����� ����� �� ������� ������, �������� ����� ���� ������� ��� ����� ������� ����� ��� ����
        // ������ ���� ��������� ��� ����� ����� camF.y = 0 � camR.y = 0 :)
        camF.y = 0;
        camR.y = 0;
        Vector3 movingVector;
        // ��� �� �������� ���� ������� �� ������ W & S �� ����������� ������ ������ � ���������� � �������� �� ������ A & D � �������� �� ����������� ������ ������
        movingVector = Vector3.ClampMagnitude(camF.normalized * Input.GetAxis("Vertical") * currentSpeed + camR.normalized * Input.GetAxis("Horizontal") * currentSpeed, currentSpeed);
        // Magnitude - ��� ������ �������. � ���� ������ �� currentSpeed ��� ��� �� �������� ���� ������ �� currentSpeed �� 86 ������. � ���� �������� ����� �������� 1.
        anim.SetFloat("magnitude", movingVector.magnitude / currentSpeed);
        //Debug.Log(movingVector.magnitude / currentSpeed);
        // ����� �� ������� ���������! ������������� �������� ������ �� x & z ������ ��� �� �� ����� ����� ��� �������� ������� � ������
        rig.velocity = new Vector3(movingVector.x, rig.velocity.y, movingVector.z);
        // � ���� ��� ���, ��� �������� �������� �� ����� � ��� �������� � ������� ���� ������
        rig.angularVelocity = Vector3.zero;
    }
    /*public void Jump()
    {
        // ��������� ������ �� ������� ��������.
        rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }*/
    private void Stamina()
    {
        if (staminaValue > 100f) staminaValue = 100f;
        //staminaText.text = staminaSlider.value.ToString();
        staminaSlider.value = staminaValue;

        if (staminaValue <= 0.5f)
        {
            currentSpeed = 2;
        }
    }
}