# Projeto-Flappy-Felpudo
3º Projeto Parcial da matéria Programação para Jogos, para o curso de Jogos Digitais da Universidade do Estado da Bahia, desenvolvidos por Jonatas Argolo e Isabela Coelho.

PLAY UNITY:
https://play.unity.com/pt/upload

### RESUMO:
  Nosso projeto foi desenvolvido na Unity 6 utilizando C#. Na primeira fase do jogo, utilizamos scripts para movimentar o Felpudo no estilo clássico de flappy birds e para fazer um gerador de inimigos automático. Após desviar de 10 inimigos, o jogador vai para a fase do Boss, onde os scripts liberam para o jogador a habilidade de soltar projéteis contínuos, e, ao contabilizar 100 acertos dentro de 15 segundos, o boss é derrotado. Por fim, programamos uma animação de Felpudo e Fofucha se encontrando e fizemos o efeito de corações usando o Particle System. Mas, os personagens são separados novamente, quando o boss, que fingiu estar morto, rapta Fofucha novamente.

  Dado a época do ano, o tema do jogo foi inspirado no Halloween, com sprites de doces como projéteis, um background fantasmagórico e o Boss sendo um pássaro vampiro.


# Primeira Fase
  O jogo começa com Felpudo adentrando a floresta assombrada em busca de Fofucha, onde ele precisa desviar de Lesmões, capangas de Urica, pássaro vampiro que raptou Fofucha. 
  Quando o contador marca 10 inimigos desviados, o jogador passa de fase, mas, se antes disso, acabar encostando no chão ou colidir com algum inimigo, a contagem resseta e deve-se reiniciar a fase.

  Foram necessários X scripts: MoveFundo, ControlaJogadorMouseEsquerdo, GeradorInimigos, .

## MoveFundo
  O script foi feito de forma simples para criar o efeito de rolagem de cenário, utilizando 1 variável principal para guardar a referência do componente de renderização gráfica:

```csharp
SpriteRenderer grafico;
```


## ControlaJogadorMouseEsquerdo
  Para fazer o jogador controlar o movimento com o clique do mouse e dar ao personagem o comportamento clássico de um jogo estilo Flappy Bird, o script utiliza uma lógica focada em gravidade e impulsos verticais.
O script conta com 3 variáveis principais para gerenciar o estado e a física do jogador:

```csharp
bool comecou; 
bool acabou; 
Rigidbody2D corpoJogador; 
Vector2 forcaImpulso = new Vector2(0, 500f); 
```

  O método Input.GetButtonDown("Fire1") detecta o clique do botão esquerdo do mouse (ou toque na tela), que é o comando principal para fazer o personagem "voar" ou dar o impulso para cima. Na primeira vez que o jogador clica, a variável comecou muda para true e a propriedade isKinematic do Rigidbody2D é desativada (false). Isso faz com que a física e a gravidade passem a afetar o personagem apenas quando o jogo realmente começa. 

  Além disso, antes de aplicar a força, a velocidade atual do jogador é zerada (corpoJogador.velocity = new Vector2(0, 0)). Isso garante que o impulso para cima seja sempre consistente, independentemente de o personagem estar caindo rápido ou subindo no momento do clique. E, por fim, para fazer o personagem subir foi utilizado o método AddForce() combinado com o Vector2 forcaImpulso para aplicar uma força vertical instantânea.

```csharp
void Update () 
{ 
  if (Input.GetButtonDown ("Fire1")) 
  {
    if (!comecou) 
    { 
      comecou = true; 
      corpoJogador.isKinematic = false;
    } 
    corpoJogador.velocity = new Vector2 (0, 0);
    corpoJogador.AddForce(forcaImpulso); 
  } 
} 
```


## GeradorInimigos
  O script conta com 6 variáveis principais para configurar o comportamento de geração e os limites do cenário:

```csharp
  public GameObject inimigoPrefab; 
public float intervalo = 3f; 
public float limiteX = 8f; 
public float limiteY = 4f; 
public float velocidade = 3f; 
public float limiteDestruicaoX = -12f;
```

  No método Start(), o script utiliza a função InvokeRepeating("GerarInimigo", 0f, intervalo) para invocar a criação de inimigos de forma contínua e temporizada assim que o jogo começa. 

```csharp
void Start(){ // Começa a gerar inimigos repetidamente
  InvokeRepeating("GerarInimigo", 0f, intervalo); 
} 
```

  O método GerarInimigo() determina a posição horizontal inicial do inimigo (limiteX) e sorteia uma altura aleatória dentro de um intervalo vertical usando Random.Range(-limiteY, limiteY). Depois, o comando Instantiate(inimigoPrefab, posicaoAleatoria, Quaternion.identity) cria uma nova cópia do inimigo na cena exatamente na posição calculada.

```csharp
void GerarInimigo(){ 
  // Define posição de spawn (à direita da tela) 
  float x = limiteX; 
  float y = Random.Range(-limiteY, limiteY); 
  Vector2 posicaoAleatoria = new Vector2(x, y); 

  // Instancia o inimigo 
  GameObject inimigo = Instantiate(inimigoPrefab, posicaoAleatoria, Quaternion.identity); 

  // Inicia o movimento automático (corrotina)
  StartCoroutine(MoverInimigo(inimigo)); 
} 
```

  Para cada inimigo criado, o script dispara uma corrotina (StartCoroutine(MoverInimigo(inimigo))) que atualiza a posição do objeto quadro a quadro da direita para a esquerda (Vector2.left * velocidade * Time.deltaTime). E caso o inimigo ultrapasse o limite estipulado (limiteDestruicaoX), ele é destruído da memória para otimizar o desempenho do jogo.

```csharp
IEnumerator MoverInimigo(GameObject inimigo){ 
  while (inimigo != null) { 
    // Move o inimigo da direita para a esquerda
    inimigo.transform.Translate(Vector2.left * velocidade * Time.deltaTime);

    // Se o inimigo sair do limite visível, destrói o objeto 
    if (inimigo.transform.position.x < limiteDestruicaoX) { 
      Destroy(inimigo); 
      yield break; // Sai da corrotina
    } 

    yield return null; // Espera o próximo fram
    }
 }
```



# Segunda Fase
  Depois de conseguir desviar dos capangas do Vampiro Urica, Felpudo o encontra! Agora, para derrotá-lo, puxa seu estoque de Plutonitas que conseguiu na Caça aos Doces do Halloween, e começa a lançá-las no vilão. 
  Agora, o objetivo é atirar 100 doces no inimigo dentro de 15 segundos para conseguir a vitória e se reencontrar com Fofucha!
  A condição de derrota por tocar o chão continua e reinicia a fase, assim como se não forem acertados doces suficientes dentro do limite de tempo.

Para esta fase foram utilizados X scripts: AtirarJogador, MoveBoss, .

#AtirarJogador
  Para que o jogador tenha uma arma para lutar contra o Boss, nos inspiramos no estilo de ataque do jogo CupHead, em que os projéteis saem constantemente. 
  Por isso, foi criado um script com X variáveis:
  
```csharp

```

