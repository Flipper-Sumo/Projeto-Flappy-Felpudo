using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GeradorInimigos : MonoBehaviour
{
    [Header("Configurações do Prefab")]
    public GameObject inimigoPrefab;

    [Header("Configurações de Spawn")]
    public float intervalo = 3f;
    public float limiteX = 8f;
    public float limiteY = 4f;

    [Header("Configurações de Movimento")]
    public float velocidade = 4f;
    public float limiteDestruicaoX = -9f;

    [Header("Controle de Fase")]
    public int limiteInimigos = 10;
    public string nomeProximaCena = "Fase2";

    private int inimigosGerados = 0;
    private int inimigosProcessados = 0; // Conta tanto os desviados quanto os que deram dano

    void Start()
    {
        // Reseta as vidas do jogador para 3 ao iniciar a Fase 1
        VidaJogador.vidas = 3;
        InvokeRepeating("GerarInimigo", 0f, intervalo);
    }

    void GerarInimigo()
    {
        if (inimigosGerados >= limiteInimigos)
        {
            CancelInvoke("GerarInimigo");
            return;
        }

        float x = limiteX;
        float y = Random.Range(-limiteY, limiteY);
        Vector2 posicaoAleatoria = new Vector2(x, y);

        GameObject inimigo = Instantiate(inimigoPrefab, posicaoAleatoria, Quaternion.identity);
        inimigosGerados++;

        StartCoroutine(MoverInimigo(inimigo));
    }

    IEnumerator MoverInimigo(GameObject inimigo)
    {
        while (inimigo != null)
        {
            inimigo.transform.Translate(Vector2.left * velocidade * Time.deltaTime);

            if (inimigo.transform.position.x < limiteDestruicaoX)
            {
                Destroy(inimigo);
                RegistrarInimigoProcessado(); // Conta quando sai da tela
                yield break;
            }

            yield return null;
        }
    }

    // Chamado tanto quando o inimigo sai da tela quanto quando atinge o jogador
    public void RegistrarInimigoProcessado()
    {
        inimigosProcessados++;

        if (inimigosProcessados >= limiteInimigos)
        {
            SceneManager.LoadScene(nomeProximaCena);
        }
    }
}