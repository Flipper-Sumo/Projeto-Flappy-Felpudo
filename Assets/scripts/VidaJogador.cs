using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VidaJogador : MonoBehaviour
{
    [Header("Configurações de Vida")]
    // Manter como static garante que o valor persista entre cenas
    public static int vidas = 3; 
    public Image[] coracoes;

    private SpriteRenderer spriteRenderer;
    private Color corOriginal;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            corOriginal = spriteRenderer.color;
        }

        // Atualiza a interface gráfica com a quantidade de vidas atual
        AtualizarInterfaceCoracoes();
    }

    public void ReceberDano()
    {
        vidas--;
        StartCoroutine(PiscarDano());
        AtualizarInterfaceCoracoes();

        if (vidas <= 0)
        {
            // Se morrer, resseta a vida para 3 antes de recarregar a cena
            vidas = 3; 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void AtualizarInterfaceCoracoes()
    {
        if (coracoes == null) return;

        for (int i = 0; i < coracoes.Length; i++)
        {
            if (coracoes[i] != null)
            {
                // Mostra apenas os corações até o limite da vida atual
                coracoes[i].enabled = (i < vidas);
            }
        }
    }

    System.Collections.IEnumerator PiscarDano()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = corOriginal;
        }
    }
}