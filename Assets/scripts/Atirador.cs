
using UnityEngine;

public class Atirador : MonoBehaviour
{
    public GameObject prefabBala;
    public Transform pontoDeDisparo;

    public float intervaloEntreTiros = 0.5f;
    private float cronometro = 0f;

    void Update()
    {
        cronometro += Time.deltaTime;

        if (cronometro >= intervaloEntreTiros)
        {
            Atirar();

            cronometro = 0f;
        }
    }

    void Atirar()
    {
        GameObject bala = Instantiate(
            prefabBala,
            pontoDeDisparo.position,
            pontoDeDisparo.rotation
        );

        Bala scriptBala = bala.GetComponent<Bala>();

        scriptBala.Disparar(Vector2.right);
    }
}
