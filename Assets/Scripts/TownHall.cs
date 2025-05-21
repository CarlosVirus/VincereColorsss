using UnityEngine;

public class TownHall : Building
{
    public bool isPlayerTownHall = true;

    protected override void Die()
    {
        base.Die();

        if (isPlayerTownHall)
        {
            Debug.Log("❌ Has perdido la partida");
            // Aquí podrías cargar una pantalla de derrota o reiniciar el nivel
        }
        else
        {
            Debug.Log("✅ ¡Has ganado la partida!");
            // Aquí podrías cargar una pantalla de victoria
        }
    }
}
