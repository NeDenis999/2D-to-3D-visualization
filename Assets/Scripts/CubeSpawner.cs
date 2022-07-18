using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] CubesVariations;

    public delegate void MethodContainer();

    public event MethodContainer OnDestroy;

    [SerializeField] private float DelayBetweenSpawn = 3f;

    private static List<GameObject> CubesOnScene = new List<GameObject>();

    public static List<GameObject> Cubes{
        get => CubesOnScene;
        set => CubesOnScene = value;
    }

    private void Awake(){
        SpawnBlock();
        for (int i = 0; i < 10; i++)
        {
            Instantiate(CubesVariations[Random.Range(0, CubesVariations.Length)], new Vector3(i * 1, 0, 0), Quaternion.identity); 
            Instantiate(CubesVariations[Random.Range(0, CubesVariations.Length)], new Vector3(i * 1, 1, 0), Quaternion.identity);
        }
    }

    private void SpawnBlock(){
        StopAllCoroutines();
        Vector3 SpawnPosition = new Vector3(transform.position.x, transform.position.y, 0);
        Instantiate(CubesVariations[Random.Range(0, CubesVariations.Length)], SpawnPosition, Quaternion.identity);
        StartCoroutine(BlockSpawnRoutine());
    }

    private IEnumerator BlockSpawnRoutine(){
        yield return new WaitForSeconds(DelayBetweenSpawn);{
            SpawnBlock();
        }
    }
}
