using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class MonsterMultiplayer : MonoBehaviour
{
    public static MonsterMultiplayer Instance { get; private set; }
    [SerializeField] float monsterSpeed;

    private void Awake() {
        //Singleton pattern
        if (Instance != null)
        {
            Debug.LogError("There's more than one PlayerChecker! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Update() {
        float yPos= gameObject.transform.position.y;
        yPos += 1 * monsterSpeed *Time.deltaTime;
        transform.position = new Vector3(0, yPos, 0);
    }

    public Transform GetMonsterPosition(){
        return transform;
    }
}
