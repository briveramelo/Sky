using UnityEngine;
using GenericFunctions;


public interface IBleedable
{
    void GenerateGuts(ref BirdStats birdStats, Vector2 gutDirection);
}

public class GutGenerator : MonoBehaviour, IBleedable
{
    [SerializeField] private GameObject _gutPrefab;
    [SerializeField] private RuntimeAnimatorController[] _gutSplosionControllers;
    
    void IBleedable.GenerateGuts(ref BirdStats birdStats, Vector2 gutDirection)
    {
        AudioManager.PlayAudio(AudioClipType.GenerateGuts);
        var totalGutValue = birdStats.GutsToSpill;
        var gutValue = 0;
        while (gutValue < totalGutValue)
        {
            var subGutValue = Mathf.Clamp(Random.Range(1, 4), 1, totalGutValue - gutValue);
            gutValue += subGutValue;

            var targetPos = (Vector2) transform.position + Random.insideUnitCircle.normalized * .05f;
            var gut = Instantiate(_gutPrefab, targetPos, Quaternion.identity);
            var targetSpeed = Random.Range(1.8f, 3f);
            var targetDir = new Vector2(Random.Range(gutDirection.x * .1f, gutDirection.x * .4f), Random.Range(3f, 8f)).normalized;
            gut.GetComponent<Rigidbody2D>().velocity = targetSpeed * targetDir;
            gut.GetComponent<Animator>().runtimeAnimatorController = _gutSplosionControllers[ConvertGutValueToIndex(subGutValue)];
            gut.transform.SetParent(transform);
            Destroy(gameObject, 2f);
        }
    }

    private static int ConvertGutValueToIndex(int subGutValue)
    {
        switch (subGutValue)
        {
            case 1:
                return 0;
            case 2:
                return Random.Range(1, 5);
            case 3:
                return Random.Range(5, 7);
        }

        return 0;
    }
}