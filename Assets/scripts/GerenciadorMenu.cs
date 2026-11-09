using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorMenu : MonoBehaviour
{
    [Header("Configuração de Cena")]
    public string nomePrimeiraFase = "Fase1"; // Nome EXATO da sua primeira cena

    public void JogarNovamente()
    {
        // Reseta as vidas do jogador para 3 antes de reiniciar
        VidaJogador.vidas = 3;

        // Carrega a Fase 1
        SceneManager.LoadScene(nomePrimeiraFase);
    }
}