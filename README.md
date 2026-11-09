# Projeto-Flappy-Felpudo
3º Projeto Parcial da matéria Programação para Jogos, para o curso de Jogos Digitais da Universidade do Estado da Bahia, desenvolvidos por Jonatas Argolo e Isabela Coelho.

PLAY UNITY:
https://play.unity.com/pt/games/21066b99-8265-4764-8590-fb9562a92d01/felpudo-flapbird

### RESUMO:
  Nosso projeto foi desenvolvido na Unity 6 utilizando C#. Na primeira fase do jogo, utilizamos scripts para movimentar o Felpudo no estilo clássico de flappy birds e para fazer um gerador de inimigos automático. Após desviar de 10 inimigos, o jogador vai para a fase do Boss, onde os scripts liberam para o jogador a habilidade de soltar projéteis contínuos, e, ao contabilizar 25 acertos dentro de 15 segundos, o boss, que também atira projéteis, é derrotado. Por fim, programamos uma animação de Felpudo e Fofucha se encontrando e fizemos o efeito de corações usando o Particle System. Mas, os personagens são separados novamente, quando o boss, que fingiu estar morto, rapta Fofucha novamente.

  Dado a época do ano, o tema do jogo foi inspirado no Halloween, com sprites de doces para os projéteis do Boss Urica Vampiro e um background fantasmagórico da floresta assombrada.


# Primeira Fase
  O jogo começa com Felpudo adentrando a floresta assombrada em busca de Fofucha, onde ele precisa desviar de Lesmões, capangas de Urica, pássaro vampiro que raptou Fofucha. Quando o contador marca 10 inimigos desviados, o jogador passa de fase, mas, se antes disso, acabar encostando no chão ou colidir com algum inimigo, a contagem resseta e deve-se reiniciar a fase.
  Aqui utilizamos scripts como MoveFundo e GeradorInimigos para, respectivamente, mover o Background (garante mais dinamicidade ao jogo) e para gerar os inimigos Lesmões de forma alatória no mapa (deixa o jogo imprevisível).

# Segunda Fase
  Depois de conseguir desviar dos capangas do Vampiro Urica, Felpudo o encontra! Agora, para derrotá-lo, puxa seu estoque de Plutonitas que conseguiu na Caça aos Doces do Halloween, e começa a lançá-las no vilão. Agora, o objetivo é atirar 100 doces no inimigo em até 15 segundos para conseguir a vitória e se reencontrar com Fofucha. Já as condições de derrota são: tocar o chão e não acertar balas o suficientes dentro do limite de tempo.
  Para esta fase, os scripts de Move|Fundo e GeradorInimigos foram desativados, pois Felpudo chegou ao final da floresta e agora vai batalhar somente com Urica. Entretanto, novos scripts foram adicionados: 
  
  **- Chefe:** responsável por gerenciar a movimentação vertical automatizada de um chefe, seu sistema de tiro periódico, barra de vida integrada à interface, feedback visual de dano e a transição para a cena de vitória ao ser derrotado;
  **- BalaChefe:** responsável por gerenciar a velocidade de disparo do projétil do chefe, sua movimentação linear para a esquerda, a destruição automática por tempo de vida e a aplicação de dano ao colidir com o jogador;
  **- Bala:** responsável por gerenciar o tempo de vida do projétil disparado pelo jogador e aplicar a velocidade direcional no momento do tiro.
  
# Cena Final
  No final, após derrotar Urica, o jogador desbloqueia a cena de finalização, em que Urica desaparece e uma mensagem de vitória aparece junto com um botão para reiniciar o jogo. Isso é controlado pelo script GerenciadorMenu.

# Principais Scripts
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

# VidaJogador
Responsável por gerenciar a quantidade de vidas do personagem, atualizar a interface visual de corações, aplicar um efeito de dano e reiniciar a cena caso as vidas acabem, script conta com 2 variáveis principais para configurar a persistência e a interface visual:

```csharp
public static int vidas = 3; 
public Image[] coracoes;
```

A variável vidas é declarada como static, o que garante que o valor da vida persista mesmo quando o jogador transita entre diferentes cenas do jogo. E quando o método ReceberDano() é chamado, a vida diminui, o efeito visual é acionado e a interface é atualizada. Se o total de vidas chegar a zero (vidas <= 0), o script redefine a vida para 3 e recarrega a cena atual usando o SceneManager:

```csharp
if (vidas <= 0)
{
    vidas = 3; 
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
```

Já o método AtualizarInterfaceCoracoes() percorre o array de imagens de corações e ativa ou desativa cada um deles com base na quantidade de vida atual do jogador (coracoes[i].enabled = (i < vidas);). E para dar um feedback claro ao jogador quando ele sofre dano, o método PiscarDano() altera temporariamente a cor do SpriteRenderer para vermelho durante uma fração de segundo (0.1f) antes de restaurar a cor original:

```csharp
if (spriteRenderer != null)
{
    spriteRenderer.color = Color.red;
    yield return new WaitForSeconds(0.1f);
    spriteRenderer.color = corOriginal;
}
```

# Atirador
Para gerenciar a cadência de tiros e instanciar novos projéteis a partir de um ponto específico do cenário, esse script conta com 4 variáveis principais para configurar o prefab do projétil, o ponto de origem e o controle de tempo entre os disparos:

```csharp
public GameObject prefabBala;
public Transform pontoDeDisparo;

public float intervaloEntreTiros = 0.5f;
private float cronometro = 0f;
```

No método Update(), o script acumula o tempo decorrido entre os quadros na variável cronometro (cronometro += Time.deltaTime;), criando um temporizador seguro e independente da taxa de quadros por segundo (framerate). Quando o cronômetro atinge ou ultrapassa o tempo limite definido (cronometro >= intervaloEntreTiros), o método Atirar() é acionado e o cronômetro é resetado para zero, garantindo um intervalo constante entre os tiros. Depois, o método Atirar() utiliza a função Instantiate() para criar uma cópia física do projétil (prefabBala) exatamente na posição e rotação do ponto de disparo (pontoDeDisparo):

```csharp
GameObject bala = Instantiate(
    prefabBala,
    pontoDeDisparo.position,
    pontoDeDisparo.rotation
);
```

Por fim, logo após instanciar o objeto, o script obtém o componente de controle do projétil (scriptBala = bala.GetComponent<Bala>();) e chama o método de disparo passando a direção desejada (scriptBala.Disparar(Vector2.right);), desacoplando a lógica de movimento do projétil do próprio atirador.
