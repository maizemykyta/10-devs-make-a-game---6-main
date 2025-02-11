using UnityEngine;

public class SpwnerResources : MonoBehaviour
{
    [SerializeField]
    GameObject SpawnGO;
    GameObject spawnedGameObject;
    float timer;
    float spawnTime;
    bool isSetTime = false;

    void Start()
    {
        spawnedGameObject = Instantiate(SpawnGO, transform);
    }

    
    void Update()
    {
        if(spawnedGameObject == null)
        {
            if (isSetTime)
            {
                timer += Time.deltaTime;
                if(timer >= spawnTime)
                {
                    spawnedGameObject = Instantiate(SpawnGO, transform);
                    isSetTime = false;
                }
            }
            else
            {
                timer = 0;
                spawnTime = Random.RandomRange(30, 60);
                isSetTime = true;
            }
        }
    }
}
