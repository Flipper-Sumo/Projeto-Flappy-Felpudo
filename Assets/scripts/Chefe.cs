using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Chefe : MonoBehaviour
{
    [Header("Movimentação")]
    public float velocidade = 2f;
    public float limiteSuperior = 3.5f;
    public float limiteInferior = -1.5f;
    private int direcao = 1;

    [Header("Sistema de Vida")]
    public int vida = 25;
    public Slider barraVidaUI;
    public string nomeCenaVitoria = "cenaFinal"; // Nome correto conforme as suas Build Settings!

    [Header("Ataque")]
    public GameObject prefabBalaChefe;
    public Transform pontoDeDisparo;
    public float intervaloEntreTiros = 2f;
    private float cronometro = 0f;

    [Header("Efeitos Visuais")]
    private SpriteRenderer spriteRenderer;
    private Color corOriginal;

    private bool foiDerrotado = false; // Evita carregar a cena múltiplas vezes

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            corOriginal = spriteRenderer.color;
        }

        if (barraVidaUI != null)
        {
            barraVidaUI.minValue = 0;
            barraVidaUI.maxValue = vida;
            barraVidaUI.value = vida;
        }
    }

    void Update()
    {
        if (foiDerrotado) return; // Para de se mover/atirar se já foi derrotado

        Movimentar();
        Atirar();
    }

    void Movimentar()
    {
        transform.Translate(Vector2.up * direcao * velocidade * Time.deltaTime);

        if (transform.position.y >= limiteSuperior)
        {
            direcao = -1;
        }
        else if (transform.position.y <= limiteInferior)
        {
            direcao = 1;
        }
    }

    void Atirar()
    {
        cronometro += Time.deltaTime;

        if (cronometro >= intervaloEntreTiros && prefabBalaChefe != null && pontoDeDisparo != null)
        {
            Instantiate(prefabBalaChefe, pontoDeDisparo.position, pontoDeDisparo.rotation);
            cronometro = 0f;
        }
    }

    public void ReceberDano(int dano)
    {
        if (foiDerrotado) return;

        vida -= dano;

        if (barraVidaUI != null)
        {
            barraVidaUI.value = vida;
        }

        Debug.Log("Vida restante do Chefe: " + vida);

        StartCoroutine(PiscarDano());

        if (vida <= 0)
        {
            foiDerrotado = true;
            Debug.Log("Chefe Derrotado! Carregando cena: " + nomeCenaVitoria);
            DerrotarChefe();
        }
    }

    void DerrotarChefe()
    {
        SceneManager.LoadScene(nomeCenaVitoria);
    }

    void OnCollisionEnter2D(Collision2D colisao)
    {
        ProcessarColisao(colisao.gameObject);
    }

    void OnTriggerEnter2D(Collider2D colisao)
    {
        ProcessarColisao(colisao.gameObject);
    }

    void ProcessarColisao(GameObject objetoColidido)
    {
        if (foiDerrotado) return;

        if (objetoColidido.GetComponent<Bala>() != null)
        {
            Destroy(objetoColidido);
            ReceberDano(1);
        }
    }

    System.Collections.IEnumerator PiscarDano()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            
            if (spriteRenderer != null)
            {
                spriteRenderer.color = corOriginal;
            }
        }
    }
}