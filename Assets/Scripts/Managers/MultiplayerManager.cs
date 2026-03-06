using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerManager : MonoBehaviour
{
    public InputActionAsset inputActions;

    [Header("Player Transforms")]
    public Transform[] playerSpawnpoints;

    [Header("Player Prefabs")]
    [SerializeField] private GameObject keyboardPrefab;

    HashSet<InputDevice> inputDevices = new HashSet<InputDevice>();

    void Awake()
    {
        // Initialize player 1 on keyboard and mouse
        PlayerInput.Instantiate(
            keyboardPrefab,
            controlScheme: "Keyboard&Mouse",
            pairWithDevice: Keyboard.current
        );
    }

    public void OnPlayerJoined(PlayerInput player)
    {
        // Potential trigger of multiple joins from same device check
        if (player.devices.Count == 0)
        {
            Debug.Log("Destroying weird player join trigger.");
            Destroy(player.gameObject);
            return;
        }
        

        // If this device already in use, do not create duplicate player
        if (inputDevices.Contains(player.devices[0]))
        {
            Debug.Log("Destroying player trying to join");
            Destroy(player.gameObject);
            return;
        }

        
        // Add this input device to the set of devices and make a player for them
        inputDevices.Add(player.devices[0]);
        MakePlayer(player.gameObject);
        Debug.Log($"Player {player.playerIndex + 1} joined with {player.devices[0]}");
    }

    public void OnPlayerLeave(PlayerInput player)
    {
        // If this player doesn't actually exist, skip
        if (player.devices.Count == 0 || !inputDevices.Contains(player.devices[0]))
        {
            return;
        }

        // Destroy the player
        Destroy(player.gameObject);
        inputDevices.Remove(player.devices[0]);
    }

    void MakePlayer(GameObject player)
    {
        // Add new character movement script
        CharacterMovement characterMovement = player.AddComponent<CharacterMovement>();

        // Set the desired fields for the character movement
        characterMovement.maxGroundSpeed = 4.0f;
        characterMovement.maxAirSpeed = characterMovement.maxGroundSpeed / 2;
        characterMovement.jumpForce = 6.0f;
        characterMovement.controlMovement(true, true);

        // Move player to their spawnpoint
        player.transform.position = playerSpawnpoints[inputDevices.Count - 1].position;
        player.transform.name = "Player " + inputDevices.Count;

        // TODO: Add ball interact stuff
    }
}
