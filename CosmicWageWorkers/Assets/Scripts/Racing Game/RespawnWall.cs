using UnityEngine;

public class RespawnWall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        RespawnPlayer(collision.gameObject);
    }

    void RespawnPlayer(GameObject player)
    {
        RaceManager rm = RaceManager.Instance;
        if (rm == null) return;

        // Get racer progress
        if (!rm.TryGetRacerProgress(player, out RacerProgress progress))
            return;

        int checkpointIndex = progress.lastCheckpointIndex;

        // Safety check
        if (checkpointIndex < 0) return;

        // Teleport player
        Transform checkpoint = rm.GetCheckpointTransform(checkpointIndex);
        player.transform.SetPositionAndRotation(
            checkpoint.position,
            checkpoint.rotation
        );
    }
}
