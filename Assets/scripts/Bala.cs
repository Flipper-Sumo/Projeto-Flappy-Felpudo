using UnityEngine;

public class Bala : MonoBehaviour
{

    public float velocidade = 10f;
    public float tempoDeVida = 3f;

    private Rigidbody2D rb;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        Destroy(gameObject, tempoDeVida);
    }

    public void Disparar(Vector2 direcao)
    {
        rb.linearVelocity = direcao.normalized * velocidade;
    }
  
}
