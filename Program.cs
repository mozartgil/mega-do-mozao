
class MegaDoMozao {
    public static void Main() {

        var numeroMinimo = 1; //Número mínimo da cartela
        var numeroMaximo = 60; //Número máximo da cartela
        var numeroParaEscolher = 6; //Números a serem escolhidos pelo cliente
        var jogoA = new List<Int32>();
        var numeroValido = false; //Valida se o INPUT é um número válido 
        var numeroOrdinal = ""; //Transcrição do número ordinal, Primeiro, Segundo, Terceiro...
        var numeroEscolhido = 0; //Número INPUTADO pelo cliente

        //Gerando números Aleatórios da Mega
        var random = new Random();
        var numerosSorteados = Enumerable.Range(0,6)
                                    .Select(numero => random.Next(1, 60)).ToList();

        foreach (var item in numerosSorteados)
        {
            Console.WriteLine($"{item} - ");
            //jogoA.Add(item);
        }

        //Usuário escolhe os 6 números da cartela
        for (int item = 1; item < numeroParaEscolher+1; item++) {
            numeroOrdinal = ConverterNumeroOrdinal(item);
            numeroValido = false;

            while (!numeroValido)
            {
                Console.Write($"Escolha o {numeroOrdinal} número válido entre {numeroMinimo} e {numeroMaximo}: ");
                var strNumeroEscolhido = Console.ReadLine();
                // var strNumeroEscolhido = "22";
            
                while (!ValidaNumeroEscolhido(numeroMinimo, numeroMaximo, strNumeroEscolhido)) {
                    Console.Write($"Por favor, escolha um número válido entre {numeroMinimo} e {numeroMaximo}: ");
                    strNumeroEscolhido = Console.ReadLine();
                    //strNumeroEscolhido = "41";
                }

                numeroEscolhido = int.Parse(strNumeroEscolhido);

                //Validando se o número escolhido já não foi escolhido antes
                if (jogoA.Contains(numeroEscolhido))
                {
                    Console.WriteLine($"O número {numeroEscolhido} já foi escolhido. Por favor escolha outro!");
                    numeroValido = false;
                } else {
                    numeroValido = true;
                }   
            }
            
            jogoA.Add(numeroEscolhido);
        }    


        Console.WriteLine("---------");
        Console.WriteLine("Números escolhidos! Muito obrigado.");
        Console.WriteLine("---------");

        foreach (var numero in jogoA)
        {
            Console.WriteLine(numero);
        }
        
        //Validando quantos números foram acertados
        int quantidadeNumerosAcertados = 0;

        foreach (var numero in jogoA)
        {
            if (numerosSorteados.Contains(numero)) {
                quantidadeNumerosAcertados++;
            }
        }

        //Mensagens das premiaçÕes
        if (quantidadeNumerosAcertados == 0) {
            Console.WriteLine("Infelizmente não foi desta vez. Tente novamente");
        } else if (quantidadeNumerosAcertados >= 1 && quantidadeNumerosAcertados <= 3) {
            Console.WriteLine($"Você acertou {quantidadeNumerosAcertados} números mas infelizmente não foi desta vez. Tente novamente");
        } else if (quantidadeNumerosAcertados == 4) {
            Console.WriteLine("PARABÉNS! Você acertou uma QUADRA!!!");
        } else if (quantidadeNumerosAcertados == 5) {
            Console.WriteLine("PARABÉNS! Você acertou uma QUINA!!!");
        } else if (quantidadeNumerosAcertados == 6) {
            Console.WriteLine("PARABÉNS! VOCÊ GANHOU NA MEGA DO MOZÃO!!!");
        }
    }

    public static bool ValidaNumeroEscolhido(Int32 numeroMinimo, Int32 numeroMaximo, string strNumeroEscolhido) {
        Int32 numeroEscolhido = 0;

        //Validando se o INPUT é nulo
        if (strNumeroEscolhido is null) {
            return false;
        }

        //Validando se o INPUT é um número mesmo.
        if (!int.TryParse(strNumeroEscolhido, out numeroEscolhido)) {
            return false;
        }

        //Validando se o número escolhido se encaixa entre numeroMinimo e numeroMaximo
        if (numeroEscolhido < 0 || numeroEscolhido > 60) {
            return false;
        }

        return true;
    }

   public static string ConverterNumeroOrdinal(int number)
    {
        string numeroOrdinal = "";
        
        if (number == 1) {
            numeroOrdinal = "primeiro";
        } else if (number == 2) {
            numeroOrdinal = "segundo";
        } else if (number == 3) {
            numeroOrdinal = "terceiro";
        } else if (number == 4) {
            numeroOrdinal = "quarto";
        } else if (number == 5) {
            numeroOrdinal = "quinto";
        } else if (number == 6) {
            numeroOrdinal = "sexto";
        }

        return numeroOrdinal;
    }
}