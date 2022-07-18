using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private Rigidbody2D Rb;

    private bool IsFalledDown = false; // When cube fall down to another cubs, we can't move it. 

    private GameObject Background;

    [SerializeField] private float MoveSpeed = 10f;

    private Vector3 PreviousCubePosition;

    [SerializeField] private string[] Tags;

    private bool IsEnteredAnotherCube = false;

    private bool CanBeDetected = false;

    public delegate void DestroyContainer();

    public event DestroyContainer OnCubeDestroy;

    public bool IsFalled{
        get => IsFalledDown;
    }

    public bool CubeCanBeDetected{
        get => CanBeDetected;
    }

    private void Start(){
        CubeSpawner.Cubes.Add(gameObject);

        FindObjectOfType<CubeSpawner>().OnDestroy += () => { // When you want to destroy all objects, only activate event: OnDestroy();
            Destroy(gameObject);
        };
        
        Background = GameObject.FindGameObjectWithTag("Background");
        Rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision){
        SetCubeStatic();
    }

    protected void SetCubeStatic(){
        IsFalledDown = true;
        RoundCubePosition();
        LockRigidbody2D();
    }

    protected void LockRigidbody2D(){ // Use only when cube falled down
        //Rb.gravityScale = 0;
        //Rb.constraints = RigidbodyConstraints2D.FreezePosition;
        //Rb.bodyType = RigidbodyType2D.Static;
        CanBeDetected = true;
    }

    protected void RoundCubePosition(){
        Vector2 CurrentCubePosition = transform.position;
        float RoundedX = Mathf.RoundToInt(CurrentCubePosition.x);
        float RoundedY = Mathf.RoundToInt(CurrentCubePosition.y);
        Vector3 RoundedCubePosition = new Vector3(RoundedX, RoundedY, 0);

        transform.position = RoundedCubePosition;
    }

    protected void ClampCubeRotationToZero(){
        Quaternion ClampedRotation = Quaternion.Euler(0, 0, 0);
        transform.rotation = ClampedRotation;
    }

    private void Update() => MoveCube();

    protected void MoveCube(){
        ClampCubeRotationToZero();
        ClampCubePosition();
        if(!IsFalledDown){
            if(Input.GetKeyDown(KeyCode.RightArrow)){
                transform.position += new Vector3(1, 0, 0);
                PreventCubeOverlaying(new Vector3(-1, 0, 0));
            }
            else if(Input.GetKeyDown(KeyCode.LeftArrow)){
                transform.position -= new Vector3(1, 0, 0);
                PreventCubeOverlaying(new Vector3(1, 0, 0));
            }
        }
    }

    protected void PreventCubeOverlaying(Vector3 returnDirection){
        Vector3 CurrentPosition = transform.position;
        CurrentPosition.x = Mathf.RoundToInt(CurrentPosition.x);
        CurrentPosition.y = Mathf.RoundToInt(CurrentPosition.y);
        foreach (var cube in CubeSpawner.Cubes)
        {
            if(cube.gameObject != null){
                Vector3 AnotherCubePosition = cube.transform.position;
                AnotherCubePosition.x = Mathf.RoundToInt(AnotherCubePosition.x);
                AnotherCubePosition.y = Mathf.RoundToInt(AnotherCubePosition.y);
                if(CurrentPosition == AnotherCubePosition && cube.gameObject != gameObject){
                    transform.position += returnDirection;
                    SetCubeStatic();
                }
            }
        }
    }

    protected void ClampCubePosition(){
        Vector2 BackgroundScale = Background.transform.localScale;

        float ClampedX = Mathf.Clamp(transform.position.x, 0, Background.transform.localScale.x - 1);
        Vector2 CubeClampedPosition = new Vector2(ClampedX, transform.position.y);
        transform.position = CubeClampedPosition;
    }

    protected void DetectSameCubesOnDestroy(){
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, transform.right, 1);
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, -transform.right, 1);
        RaycastHit2D downHit = Physics2D.Raycast(transform.position, -transform.up, 1);

        if(rightHit && rightHit.collider.tag == gameObject.tag && rightHit.collider.gameObject != gameObject)
            DestroyByRay(rightHit);

        if(leftHit && leftHit.collider.tag == gameObject.tag && leftHit.collider.gameObject != gameObject)
            DestroyByRay(leftHit);

        if(downHit && downHit.collider.tag == gameObject.tag && downHit.collider.gameObject != gameObject)
            DestroyByRay(downHit);  
    }

    private void DestroyByRay(RaycastHit2D hit) => Destroy(hit.collider.gameObject);

    private void OnDestroy(){
        DetectSameCubesOnDestroy();
    }
}
