using UnityEngine;

public class GameManagerLoader : MonoBehaviour
{
    [SerializeField] private GameObject _gameManager = null;

    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            Instantiate(_gameManager);
        }

        Destroy(gameObject);
    }
}