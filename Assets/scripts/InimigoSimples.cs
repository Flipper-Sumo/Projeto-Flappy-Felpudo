using UnityEngine;

public class InimigoSimples : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D colisao)
    {
        VidaJogador vida = colisao.GetComponent<VidaJogador>();
        
        if (vida != null)
        {
            vida.ReceberDano();

            // Encontra o gerador na cena e registra que esse inimigo já foi processado
            GeradorInimigos gerador = FindFirstObjectByType<GeradorInimigos>();
            if (gerador != null)
            {
                gerador.RegistrarInimigoProcessado();
            }

            Destroy(gameObject);
        }
    }
}