
using Newtonsoft.Json;
using Validacao;

class MegaDoMozao
{
    public static void Main()
    {
        Console.Clear();
        var nomeArquivoJogo = "Mega.json";
        var nomeArquivoJogadores = "Jogadores.json";
        var keyOption = "1";

        Console.WriteLine("Bem vindo à MEGA DO MOZAO!");
        Console.WriteLine("--------------------------");

        while (keyOption != "3")
        {
            //Chamando MENU principal e retornando opção do usuário
            keyOption = OpcaoMega(nomeArquivoJogo);

            //Caso 1 - lê o arquivo já existente
            //Caso 2 - cria o arquivo e retorna os dados criados
            //Caso 3 - Sorteio do jogo
            var strJogoMega = "";

            if (keyOption == "1" || keyOption == "3")
            {
                strJogoMega = File.ReadAllText(nomeArquivoJogo);
            }
            else if (keyOption == "2")
            {
                strJogoMega = NovoJogoMega(nomeArquivoJogo, nomeArquivoJogadores);
            }

            var jsonJogoMega = JsonConvert.DeserializeObject<JogoMega>(strJogoMega);

            //Criando um novo jogador
            if (keyOption == "1" || keyOption == "2")
            {
                var strNovoJogador = NovoJogador(jsonJogoMega.idJogoMega, nomeArquivoJogadores, jsonJogoMega.ListNumerosSorteados);
            }
        }

        //SORTEIO DA MEGA
        if (keyOption == "3")
        {
            var strListaJogadores = File.ReadAllText(nomeArquivoJogadores);
            var jsonListaJogadores = JsonConvert.DeserializeObject<List<JogadorMega>>(strListaJogadores);

            //Selecionando jogadores que tiveram +3 acertos
            var resultadoVencedores = jsonListaJogadores.Where(vencedor => vencedor.QtdNumerosAcerto > 3)
                                                        .OrderByDescending(vencedor => vencedor.QtdNumerosAcerto)
                                                        .ToList();
            
            if (resultadoVencedores.Count > 0) {
                foreach (var vencedor in resultadoVencedores)
                {
                    Console.WriteLine($"O jogador {vencedor.CPF} teve {vencedor.QtdNumerosAcerto} ACERTOS no jogo {vencedor.idJogo}!");
                    Console.WriteLine(MensagensAcertos(vencedor.QtdNumerosAcerto));
                    Console.WriteLine("-------------------------");
                    Console.WriteLine("");
                }
            } else {
                Console.WriteLine("Não houve nenhum vencedor da MEGA!");
                Console.WriteLine("-------------------------");
                Console.WriteLine("");
            }
        }
    }

    //Verificando se já existe alguma Jogo da Mega Ativo
    public static string OpcaoMega(string nomeArquivoJogo)
    {
        var keyOption = "0";

        if (File.Exists(nomeArquivoJogo))
        {
            Console.WriteLine("Já existe uma série da MEGA DO MOZAO ativa.");
            Console.WriteLine("Tecle 1 para jogar nessa MEGA DO MOZAO ativa");
            Console.WriteLine("Tecle 2 para jogar gerar uma nova MEGA DO MOZAO");
            Console.WriteLine("Tecle 3 para realizar o SORTEIO da MEGA DO MOZAO");
            Console.WriteLine("--------------------------");
            Console.Write("Opção: ");
            keyOption = Console.ReadLine();
            Console.WriteLine("--------------------------");

            while (!ValidaNumeroEscolhido(1, 3, keyOption))
            {
                Console.WriteLine("");
                Console.WriteLine("Por favor, informe uma opção válida!");
                Console.WriteLine("Tecle 1 para jogar nessa MEGA DO MOZAO ativa");
                Console.WriteLine("Tecle 2 para jogar gerar uma nova MEGA DO MOZAO");
                Console.WriteLine("Tecle 3 para realizar o SORTEIO da MEGA DO MOZAO");
                Console.WriteLine("--------------------------");
                Console.Write("Opção: ");
                keyOption = Console.ReadLine();
                Console.WriteLine("--------------------------");
            }
        }
        else
        {
            keyOption = "2";
        }

        return keyOption;
    }

    public static string NovoJogoMega(string nomeArquivoJogo, string nomeArquivoJogadores)
    {
        //ID da Mega
        var idJogo = GetNewID();

        //Gerando números Aleatórios da Mega
        var randomNumeros = new Random();
        var listaNumerosSorteados = new List<int>();
        var numeroSorteado = 0;

        while (listaNumerosSorteados.Count < 6)
        {
            numeroSorteado = randomNumeros.Next(1, 60);

            //Verificando duplicidade no número gerado.
            if (!listaNumerosSorteados.Contains(numeroSorteado))
                listaNumerosSorteados.Add(numeroSorteado);
        }

        //COMENTAR BLOCO ABAIXO!!!
        Console.WriteLine("NÚMEROS SORTEADOS:");
        foreach (var numero in listaNumerosSorteados)
        {
            Console.Write($"{numero} - ");
        }
        Console.WriteLine("");
        Console.WriteLine("---------");
        Console.WriteLine("");

        var novoJogoMega = new JogoMega(idJogo, listaNumerosSorteados.ToList());

        //Escrevendo o novo jogo
        File.Create(nomeArquivoJogo).Close();

        //Serializando o Jogo da Mega
        var jsonNovoJogoMega = JsonConvert.SerializeObject(novoJogoMega, Formatting.Indented);
        File.AppendAllText(nomeArquivoJogo, jsonNovoJogoMega);

        //Zerando os jogadores
        File.Create(nomeArquivoJogadores).Close();

        //Retornando a string serializada do jogo da MEga
        return jsonNovoJogoMega;
    }

    public class JogoMega
    {
        public long idJogoMega { get; set; }
        public List<int> ListNumerosSorteados { get; set; }

        public DateTime datadoJogo { get; set; }
        public JogoMega(long idJogoMega, List<int> listNumerosSorteados)
        {
            this.ListNumerosSorteados = listNumerosSorteados;
            this.idJogoMega = idJogoMega;
            datadoJogo = DateTime.Now;
        }

        public JogoMega() { }
    }

    public static bool NovoJogador(long idJogoMega, string nomeArquivoJogadores, List<int> numerosSorteados)
    {
        var jogadorValido = false;
        var jogadorCPF = "";

        Console.Write("Informe o CPF do jogador: ");
        jogadorCPF = Console.ReadLine();

        //Validando CPF do Jogador
        while (!ValidaCPF.IsCpf(jogadorCPF))
        {
            Console.Write("CPF inválido! Por favor informe um CPF válido: ");
            jogadorCPF = Console.ReadLine();
        }
        Console.WriteLine("");
        Console.WriteLine("---------");
        Console.WriteLine("");

        var jogo = new List<int>();
        var numeroMinimo = 1; //Número mínimo da cartela
        var numeroMaximo = 60; //Número máximo da cartela
        var qtdNumerosParaEscolher = 6; //Números a serem escolhidos pelo cliente  var numeroValido = false; //Valida se o INPUT é um número válido 
        var numeroOrdinal = ""; //Transcrição do número ordinal, Primeiro, Segundo, Terceiro...
        var numeroEscolhido = 0; //Número INPUTADO pelo cliente
        var numeroValido = false;

        //Usuário escolhe os 6 números da cartela
        for (int item = 1; item < qtdNumerosParaEscolher + 1; item++)
        {
            numeroOrdinal = ConverterNumeroOrdinal(item);
            numeroValido = false;

            while (!numeroValido)
            {
                Console.Write($"Escolha o {numeroOrdinal} número válido entre {numeroMinimo} e {numeroMaximo}: ");
                var strNumeroEscolhido = Console.ReadLine();
                // var strNumeroEscolhido = item.ToString();

                while (!ValidaNumeroEscolhido(numeroMinimo, numeroMaximo, strNumeroEscolhido))
                {
                    Console.Write($"Por favor, escolha um número válido entre {numeroMinimo} e {numeroMaximo}: ");
                    strNumeroEscolhido = Console.ReadLine();
                    // strNumeroEscolhido = "41";
                }

                numeroEscolhido = int.Parse(strNumeroEscolhido);

                //Validando se o número escolhido já não foi escolhido antes
                if (jogo.Contains(numeroEscolhido))
                {
                    Console.WriteLine($"O número {numeroEscolhido} já foi escolhido. Por favor escolha outro!");
                    numeroValido = false;
                }
                else
                {
                    numeroValido = true;
                }
            }

            jogo.Add(numeroEscolhido);
        }

        //COMENTAR BLOCO ABAIXO!!!
        Console.WriteLine("");
        Console.WriteLine("NÚMEROS ESCOLHIDOS!");
        foreach (var numero in jogo)
        {
            Console.Write($"{numero} - ");
        }
        Console.WriteLine("");
        Console.WriteLine("---------");
        Console.WriteLine("");

        //Verificando quantos números o jogador acertou neste jogo
        var qtdNumerosAcerto = NumerosAcertados(idJogoMega, numerosSorteados, jogo);

        var novoJogador = new JogadorMega(jogadorCPF, jogo, idJogoMega, qtdNumerosAcerto, nomeArquivoJogadores);

        //Pegando o que tem dentro do Json de Jogadores
        var listaJogadoresMega = new List<JogadorMega>();
        var jsonJogadores = File.ReadAllText(nomeArquivoJogadores);

        if (new FileInfo(nomeArquivoJogadores).Length > 0)
            listaJogadoresMega = JsonConvert.DeserializeObject<List<JogadorMega>>(jsonJogadores);

        listaJogadoresMega.Add(novoJogador);

        var jsonListaJogadores = JsonConvert.SerializeObject(listaJogadoresMega, Formatting.Indented);

        File.Create(nomeArquivoJogadores).Close();
        File.AppendAllText(nomeArquivoJogadores, jsonListaJogadores);

        jogadorValido = true;

        return jogadorValido;
    }

    public class JogadorMega
    {
        public string CPF { get; set; }
        public List<Int32> numerosEscolhidos { get; set; }
        public long IdJogoMega { get; set; }
        public DateTime dataDoJogo { get; set; }
        public long idJogo { get; set; }
        public int QtdNumerosAcerto { get; set; }

        public JogadorMega(string CPF, List<Int32> numerosEscolhidos, long idJogoMega, int qtdNumerosAcerto, string nomeArquivoJogadores)
        {
            this.QtdNumerosAcerto = qtdNumerosAcerto;
            this.IdJogoMega = idJogoMega;
            this.CPF = CPF;
            this.numerosEscolhidos = numerosEscolhidos;
            dataDoJogo = DateTime.Now;

            var novoIDJogo = GetNewID();

            novoIDJogo = 1072292802;

            while (!ValidaNovoIDJogador(nomeArquivoJogadores, novoIDJogo)) {
                // Console.WriteLine("PASSOU PELO ID REPETIDO");
                novoIDJogo = GetNewID();
            }

            // Console.WriteLine("SEGUIU PELO ID REPETIDO");
            idJogo = novoIDJogo;
        }

        public JogadorMega() { }
    }

    //Validação do número escolihido pelo cliente
    public static bool ValidaNumeroEscolhido(Int32 numeroMinimo, Int32 numeroMaximo, string strNumeroEscolhido)
    {
        Int32 numeroEscolhido = 0;

        //Validando se o INPUT é nulo
        if (strNumeroEscolhido is null)
        {
            return false;
        }

        //Validando se o INPUT é um número mesmo.
        if (!int.TryParse(strNumeroEscolhido, out numeroEscolhido))
        {
            return false;
        }

        //Validando se o número escolhido se encaixa entre numeroMinimo e numeroMaximo
        if (numeroEscolhido < numeroMinimo || numeroEscolhido > numeroMaximo)
        {
            return false;
        }

        return true;
    }

    // Convertendo números das cartelas para ordinais
    public static string ConverterNumeroOrdinal(int number)
    {
        string numeroOrdinal = "";

        if (number == 1)
            numeroOrdinal = "primeiro";
        else if (number == 2)
            numeroOrdinal = "segundo";
        else if (number == 3)
            numeroOrdinal = "terceiro";
        else if (number == 4)
            numeroOrdinal = "quarto";
        else if (number == 5)
            numeroOrdinal = "quinto";
        else if (number == 6)
            numeroOrdinal = "sexto";

        return numeroOrdinal;
    }

    public static int GetNewID()
    {
        var randomID = new Random();
        var idJogo = randomID.Next();

        return idJogo;
    }

    public static bool ValidaNovoIDJogador(string nomeArquivoJogadores, int newID) {
        var idValido = true;

        //Pegando o que tem dentro do Json de Jogadores
        var listaJogadoresMega = new List<JogadorMega>();
        var jsonJogadores = File.ReadAllText(nomeArquivoJogadores);

        if (new FileInfo(nomeArquivoJogadores).Length > 0) {
            listaJogadoresMega = JsonConvert.DeserializeObject<List<JogadorMega>>(jsonJogadores);

            foreach (var jogador in listaJogadoresMega)
            {
                if (jogador.idJogo == newID)
                {
                    return false;
                }
            }
        }

        return idValido;
    }

    public static int NumerosAcertados(long idJogoMega, List<int> numerosSorteados, List<int> numerosEscolhidos)
    {
        var qtdNumerosAcerto = 0;

        foreach (var numero in numerosEscolhidos)
        {
            if (numerosSorteados.Contains(numero))
            {
                qtdNumerosAcerto++;
            }
        }

        return qtdNumerosAcerto;
    }

    public static string MensagensAcertos(int quantidadeAcertos) {
        //Mensagens das premiaçÕes
        var mensagemAcerto = "";

        if (quantidadeAcertos == 0)
        {
            mensagemAcerto = "Infelizmente não foi desta vez. Tente novamente";
        }
        else if (quantidadeAcertos >= 1 && quantidadeAcertos <= 3)
        {
            mensagemAcerto = $"Você acertou {quantidadeAcertos} números mas infelizmente não foi desta vez. Tente novamente";
        }
        else if (quantidadeAcertos == 4)
        {
            mensagemAcerto = "PARABÉNS! Você acertou uma QUADRA!!!";
        }
        else if (quantidadeAcertos == 5)
        {
            mensagemAcerto = "PARABÉNS! Você acertou uma QUINA!!!";
        }
        else if (quantidadeAcertos == 6)
        {
            mensagemAcerto = "PARABÉNS! VOCÊ GANHOU NA MEGA DO MOZÃO!!!";
        }

        return mensagemAcerto;
    }
}