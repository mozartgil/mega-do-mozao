using System.Text.Json;
using Validacao;

class MegaDoMozao
{
    public static void Main()
    {
        Console.Clear();

        // Gerando um novo jogo da Mega
        var strNovoJogoMega = NovoJogoMega();
        var jsonNovoJogoMega = JsonSerializer.Deserialize<JogoMega>(strNovoJogoMega);

        //Gerando um novo jogador
        var novoJogador = NovoJogador(jsonNovoJogoMega.idJogoMega);
        var jsonNovoJogador = JsonSerializer.Deserialize<ClienteMega>(novoJogador);

        //Validando quantos números foram acertados
        int quantidadeNumerosAcertados = 0;
        var numerosSorteados = jsonNovoJogoMega.ListNumerosSorteados.ToList();
        var listJogo = jsonNovoJogador.numerosEscolhidos.ToList();

        foreach (var numero in listJogo)
        {
            if (numerosSorteados.Contains(numero))
            {
                quantidadeNumerosAcertados++;
            }
        }

        //Mensagens das premiaçÕes
        if (quantidadeNumerosAcertados == 0)
        {
            Console.WriteLine("Infelizmente não foi desta vez. Tente novamente");
        }
        else if (quantidadeNumerosAcertados >= 1 && quantidadeNumerosAcertados <= 3)
        {
            Console.WriteLine($"Você acertou {quantidadeNumerosAcertados} números mas infelizmente não foi desta vez. Tente novamente");
        }
        else if (quantidadeNumerosAcertados == 4)
        {
            Console.WriteLine("PARABÉNS! Você acertou uma QUADRA!!!");
        }
        else if (quantidadeNumerosAcertados == 5)
        {
            Console.WriteLine("PARABÉNS! Você acertou uma QUINA!!!");
        }
        else if (quantidadeNumerosAcertados == 6)
        {
            Console.WriteLine("PARABÉNS! VOCÊ GANHOU NA MEGA DO MOZÃO!!!");
        }
    }

    public static string NovoJogoMega()
    {
        //ID da Mega
        var randomID = new Random();
        var idJogo = randomID.Next();

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

        //Serializando o Jogo da Mega
        var jsonNovoJogoMega = JsonSerializer.Serialize(novoJogoMega);
        
        //Escrevendo o novo jogo
        File.Create("Mega do Mozao.txt").Close();
        File.AppendAllText("Mega do Mozao.txt", jsonNovoJogoMega);

        //Zerando os jogadores
        File.Create("Jogadores Mega do Mozao.txt").Close();

        //Retornando a string serializada do jogo da MEga
        return jsonNovoJogoMega;
    }

    public class JogoMega
    {
        public long idJogoMega { get; set; }
        public List<int> ListNumerosSorteados { get; set; }

        public DateTime datadoJogo { get; private set; }
        public JogoMega(long idJogoMega, List<int> listNumerosSorteados)
        {
            this.ListNumerosSorteados = listNumerosSorteados;
            this.idJogoMega = idJogoMega;
            datadoJogo = DateTime.Now;
        }
    }

    public static string NovoJogador(long idJogoMega)
    {
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

        var jogoA = new List<int>();
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
                if (jogoA.Contains(numeroEscolhido))
                {
                    Console.WriteLine($"O número {numeroEscolhido} já foi escolhido. Por favor escolha outro!");
                    numeroValido = false;
                }
                else
                {
                    numeroValido = true;
                }
            }

            jogoA.Add(numeroEscolhido);
        }

        //COMENTAR BLOCO ABAIXO!!!
        Console.WriteLine("");
        Console.WriteLine("NÚMEROS ESCOLHIDOS!");
        foreach (var numero in jogoA)
        {
            Console.Write($"{numero} - ");
        }
        Console.WriteLine("");
        Console.WriteLine("---------");
        Console.WriteLine("");

        var cliente = new ClienteMega(jogadorCPF, jogoA, idJogoMega);

        var jsonNovoCliente = JsonSerializer.Serialize(cliente);

        File.AppendAllText("Jogadores Mega do Mozao.txt", jsonNovoCliente);

        return jsonNovoCliente;
    }

    public class ClienteMega
    {
        public string CPF { get; set; }
        public List<Int32> numerosEscolhidos { get; set; }
        public long IdJogoMega { get; set; }

        public DateTime dataDoJogo { get; private set; }

        public ClienteMega(string CPF, List<Int32> numerosEscolhidos, long idJogoMega)
        {
            this.IdJogoMega = idJogoMega;
            this.CPF = CPF;
            this.numerosEscolhidos = numerosEscolhidos;
            dataDoJogo = DateTime.Now;
        }
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
        if (numeroEscolhido < 0 || numeroEscolhido > 60)
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
        {
            numeroOrdinal = "primeiro";
        }
        else if (number == 2)
        {
            numeroOrdinal = "segundo";
        }
        else if (number == 3)
        {
            numeroOrdinal = "terceiro";
        }
        else if (number == 4)
        {
            numeroOrdinal = "quarto";
        }
        else if (number == 5)
        {
            numeroOrdinal = "quinto";
        }
        else if (number == 6)
        {
            numeroOrdinal = "sexto";
        }

        return numeroOrdinal;
    }
}