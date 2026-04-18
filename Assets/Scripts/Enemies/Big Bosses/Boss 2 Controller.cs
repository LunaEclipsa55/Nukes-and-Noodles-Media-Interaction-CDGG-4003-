using System.Collections;
using UnityEngine;

public class StationaryBoss : MonoBehaviour
{
    [Header("Attack Positions")]
    // These are the places on the ground where attacks can appear.
    public Transform[] attackPoints;

    [Header("Prefabs")]
    // warningPrefab = visual warning first
    // hazardPrefab = actual damaging area after the warning
    public GameObject warningPrefab;
    public GameObject hazardPrefab;

    [Header("Timing")]
    // How long the warning stays before the real attack happens
    public float warningTime = 1f;

    // How long the hazard stays on the ground
    public float hazardLife = 3f;

    // Delay between one attack and the next
    public float delayBetweenAttacks = 0.5f;
    // t make the delay faster put *= 0.95f;

    void Start()
    {
        // Start the boss behavior when the scene begins
        StartCoroutine(BossLoop());
    }

    IEnumerator BossLoop()
    {
        // Repeat forever
        while (true)
        {
            // Pattern 1: left to right
            yield return StartCoroutine(PatternLeftToRight());

            //ro make the pattern random
            //int pattern = Random.Range(0, 2);
            //if(pattern = 0)
            //  yield return StartCoroutine(PatternLeftToRight());
            //else
            //  yield return StartCoroutine(PatternOutsideToInside());

            // Small pause before next pattern
            yield return new WaitForSeconds(1f);

            // Pattern 2: outside to inside
            yield return StartCoroutine(PatternOutsideToInside());

            // Small pause before restarting
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator PatternLeftToRight()
    {
        // Attack each point in order
        for (int i = 0; i < attackPoints.Length; i++)
        {
            yield return StartCoroutine(DoAttack(attackPoints[i].position));
            yield return new WaitForSeconds(delayBetweenAttacks);
        }
        //to do multiple attacks at once
        //StartCoroutine(DoAttack(attackPoints[0].position));
        //StartCoroutine(DoAttack(attackPoints[4].position));
    }

    IEnumerator PatternOutsideToInside()
    {
        // This pattern assumes at least 5 points.
        // Example order: far left, far right, middle-left, middle-right, center
        if (attackPoints.Length < 5)
            yield break;

        int[] order = { 0, 4, 1, 3, 2 };

        for (int i = 0; i < order.Length; i++)
        {
            yield return StartCoroutine(DoAttack(attackPoints[order[i]].position));
            yield return new WaitForSeconds(delayBetweenAttacks);
        }
    }

    IEnumerator DoAttack(Vector3 position)
    {
        //Spawn warning marker
        GameObject warning = Instantiate(warningPrefab, position, Quaternion.identity);

        //Wait so the player has time to react
        yield return new WaitForSeconds(warningTime);

        //Remove the warning marker
        Destroy(warning);

        //Spawn the real hazard
        GameObject hazard = Instantiate(hazardPrefab, position, Quaternion.identity);

        //Remove the hazard after a few seconds
        Destroy(hazard, hazardLife);
    }
}