using Player;
using UnityEngine;

public class GameFinisher : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            GameManager.Instance.EndGame();
        }
    }
}
