using UnityEngine;
using System.Collections;



public class CherryController : MonoBehaviour
{
    [SerializeField] private LevelGrid levelGrid;
    [SerializeField] private GameObject lettucePrefab;
    [SerializeField] private float cherrySpeed = 4f;
    [SerializeField] private float spawnMargin = 1f;
    [SerializeField] private float respawnDelay =5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnCherry());
    }
    


    private IEnumerator SpawnCherry()
    {
        yield return new WaitForSeconds(respawnDelay);

        int width = levelGrid.GetWidth();
        int height = levelGrid.GetHeight();

        Vector3 topLeftCorner = levelGrid.gridToWorld(new Vector2Int(0,0));
        Vector3 bottomRightCorner = levelGrid.gridToWorld(new Vector2Int(width -1, height -1));

        Vector3 gridCentre = (topLeftCorner + bottomRightCorner) / 2 ;


        while (true)
        {
            Vector3 randomStartPosition = StartPosition(gridCentre);
            Vector3 mirrorPoint = gridCentre - (randomStartPosition-gridCentre);

            GameObject lettuce = Instantiate(lettucePrefab, randomStartPosition, Quaternion.identity);
            yield return (MoveCherry(lettuce, randomStartPosition, mirrorPoint));
            

            if(lettuce != null)
            {
                Destroy(lettuce);
            }


            yield return new WaitForSeconds(respawnDelay);
        }

       
    }

    private IEnumerator MoveCherry(GameObject lettuce, Vector3 startLocation, Vector3 destination)
    {

        float distance = Vector3.Distance(startLocation, destination);
        float duration = distance/cherrySpeed;
        float elapsed = 0f;

        while(elapsed < duration)
        {
            if(lettuce == null)
            {
                break;
            }

            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            lettuce.transform.position = Vector3.Lerp(startLocation, destination, t);
            
            
            
            yield return null;
        }


        if (Mathf.Clamp01(elapsed / duration) >= 1f && lettuce != null)
            {
                lettuce.transform.position = destination;   
            }

        
    }

    private Vector3 StartPosition(Vector3 gridCentre)
    {
        int width = levelGrid.GetWidth();
        int height = levelGrid.GetHeight();

       

        Vector3 topLeftCorner = levelGrid.gridToWorld(new Vector2Int(0,0));
        Vector3 bottomRightCorner = levelGrid.gridToWorld(new Vector2Int(width -1, height -1));

        float left = Mathf.Min(topLeftCorner.x, bottomRightCorner.x) - 0.5f - spawnMargin;
        float right = Mathf.Max(topLeftCorner.x, bottomRightCorner.x) + 0.5f + spawnMargin;
        float bottom = Mathf.Min(topLeftCorner.y, bottomRightCorner.y) - 0.5f - spawnMargin;
        float top = Mathf.Max(topLeftCorner.y, bottomRightCorner.y) + 0.5f + spawnMargin;

        switch (Random.Range(0, 4))
        {
            case 0: return new Vector3(left, Random.Range(bottom, top), gridCentre.z);
            case 1: return new Vector3(right, Random.Range(bottom, top), gridCentre.z);
            case 2: return new Vector3(Random.Range(left, right), top, gridCentre.z);
            default: return new Vector3(Random.Range(left, right), bottom, gridCentre.z);
        }
    }


}
