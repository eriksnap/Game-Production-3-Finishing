using UnityEngine;

public class BoatCharacterSlot : MonoBehaviour
{
    private GameObject currentCharacter;

    public void SetCharacter(GameObject characterPrefab)
    {
        if (currentCharacter != null)
            Destroy(currentCharacter);

        if (characterPrefab != null)
            currentCharacter = Instantiate(characterPrefab, transform.position, transform.rotation, transform);
    }
}