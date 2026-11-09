using UnityEngine;

public class BalaChefe : MonoBehaviour
{
    public float velocidade = 10f;
    public float tempoDeVida = 3f;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Vector2.left empurra a bala para a esquerda
        rb.linearVelocity = Vector2.left * velocidade;
        Destroy(gameObject, tempoDeVida);
    }

    void OnTriggerEnter2D(Collider2D colisao)
    {
        // Tenta encontrar o nosso script de vida no objeto em que tocou
        VidaJogador vidaDoJogador = colisao.gameObject.GetComponent<VidaJogador>();
        
        if (vidaDoJogador != null)
        {
            vidaDoJogador.ReceberDano(); // Tira 1 vida e faz piscar
            Destroy(gameObject); // Destrói a bala inimiga
        }
    }
}