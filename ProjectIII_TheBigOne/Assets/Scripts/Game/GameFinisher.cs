using UnityEngine;

namespace Tavaris.Manager
{
    public class GameFinisher : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == GameManager.Player)
            {
                GameManager.Instance.EndGame();
            }
        }
    }
}