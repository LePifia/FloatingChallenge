using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using System.IO;

[System.Serializable]

public class ObjectPoolItemMultiplayer
{
  public string objectName;
  public GameObject objectToPool;
  public Transform Container;

  public string floder1;
  public string folder2;
  public string folder3;
  public int amountToPool;
  public bool shouldExpand;
}

public class ObjectPoolerMultiplayer : MonoBehaviour
{

  public static ObjectPoolerMultiplayer SharedInstanceMultiplayer;
  public List<ObjectPoolItemMultiplayer> itemsToPool;
  public List<GameObject> pooledObjects;

  void Awake()
  {
    SharedInstanceMultiplayer = this;
    pooledObjects = new List<GameObject>();

    /*PhotonNetwork.Instantiate(Path.Combine("Obstacles", "Symbols", obstacleList[UnityEngine.Random.Range(0, obstacleList.Length)].name ) , 
        new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y -15, gameObject.transform.position.z)
        , Quaternion.identity); */
    foreach (ObjectPoolItemMultiplayer item in itemsToPool)
    {
      for (int i = 0; i < item.amountToPool; i++)
      {
        if (item.floder1 == null){
            GameObject obj =PhotonNetwork.Instantiate(Path.Combine(item.objectName), item.Container.position, Quaternion.identity);
            obj.SetActive(false);
             pooledObjects.Add(obj);
            

        }

        if (item.folder2 == null){
            GameObject obj =PhotonNetwork.Instantiate(Path.Combine(item.floder1,item.objectName), item.Container.position, Quaternion.identity);
            obj.SetActive(false);
             pooledObjects.Add(obj);
            }

        if (item.folder3 == null){
            GameObject obj =PhotonNetwork.Instantiate(Path.Combine(item.floder1, item.folder2, item.objectName), item.Container.position, Quaternion.identity);
            obj.SetActive(false);
             pooledObjects.Add(obj);

        }

        if (item.folder3 != null){
             GameObject obj =PhotonNetwork.Instantiate(Path.Combine(item.floder1, item.folder2, item.folder3, item.objectName), item.Container.position, Quaternion.identity);
             obj.SetActive(false);
             pooledObjects.Add(obj);
        }
        
      }
        
        
      }
  }
  

  //call this in another script to get an object from pool
  //so instead of using GameObject = instantiate(Gameobject), use GameObject = ObjectPooler.SharedInstance.GetPooledObject("tag of Object") as GameObject
  public GameObject GetPooledObjectMultiplayer(string tag)
  {
    for (int i = 0; i < pooledObjects.Count; i++)
    {
      if (!pooledObjects[i].activeSelf && pooledObjects[i].tag == tag)
      {
        return pooledObjects[i];
      }
    }
    foreach (ObjectPoolItemMultiplayer item in itemsToPool)
    {
      if (item.objectToPool.tag == tag)
      {
        if (item.shouldExpand)
        {
          GameObject obj = (GameObject)Instantiate(item.objectToPool, item.Container);
          obj.SetActive(false);
          pooledObjects.Add(obj);
          return obj;
        }
      }
    }
    return null;
  }

  //Call this when you want to destroy object 
  //so instead of calling Destroy(gameObject, time), call ObjectPooler.SharedInstance.TakePooledObject(gameObject, time)
  public void TakePooledObject(GameObject obj, float time)
  {
    StartCoroutine(DeactivateObj(obj, time));
  }

  IEnumerator DeactivateObj(GameObject gameObject, float time)
  {
    if (gameObject != null && gameObject.activeSelf)
    {
      yield return new WaitForSeconds(time);
      gameObject.SetActive(false);
    }
  }
}
