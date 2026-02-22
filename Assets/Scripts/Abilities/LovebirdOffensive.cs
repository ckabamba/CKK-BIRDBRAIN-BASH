using System.Numerics;
using System.Threading.Tasks.Dataflow;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BallInteract))]
public class LoveBirdOffensive() : MonoBehaviour
{
    public float DebuffLength = 2.0f; // Time in seconds the debuff lasts
    public float Cooldown = 20.0f; // Time in seconds the cooldown lasts
    private bool _onCooldown = false;
    private bool _debuffActive = false;
    private List<GameObject> opponents = [];

    void Update()
    {
        // If the debuff is active, moves the opponents towards the net
        if (_debuffActive)
        {
            foreach (GameObject opponent in opponents)
            {
                // Gets a normalized direction vector from the opponent to the Lovebird
                Vector3 dir = this.transform.position - opponent.transform.position;
                dir.Normalize();
                // Moves opponent towards the Lovebird
                opponent.transform.position += dir * opponent.speed;
            }
        }
    }

    public void DebuffEnemy()
    {
        if (!_onCooldown)

            // Gets opponents
            if (_onLeft)
            {
                opponents.Add(gameManager.rightPlayer1);
                opponents.Add(gameManager.rightPlayer2);
            } else
            {
                opponents.Add(gameManager.leftPlayer1);
                opponents.Add(gameManager.leftPlayer2);
            }

            // Disables manual movement for AI and Players
            foreach (GameObject opponent in opponents)
            {
                if (opponent.GetComponent<CharacterMovement>())
                {
                    opponent.GetComponent<CharacterMovement>().enabled = false;
                }
                if (opponent.GetComponent<AIBehavior>())
                {
                    opponent.GetComponent<AIBehavior>().enabled = false;
                }
            }

            StartCoroutine(DebuffTimer);
            
        }

    public IEnumerator DebuffTimer()
    {
        _debuffActive = true;
        StartCoroutine(Cooldown);
        yield return new WaitForSeconds(DebuffLength);
        _debuffActive = false;

        //Re-enables manual movement for AI and Players
        foreach (GameObject opponent in opponents)
        {
            if (opponent.GetComponent<CharacterMovement>())
            {
                opponent.GetComponent<CharacterMovement>().enabled = true;
            }
            if (opponent.GetComponent<AIBehavior>())
            {
                opponent.GetComponent<AIBehavior>().enabled = true;
            }
        }
    }

    public IEnumerator Cooldown()
    {
        _onCooldown = true;
        yield return new WaitForSeconds(Cooldown);
        _onCooldown = false;
    }
}

    
