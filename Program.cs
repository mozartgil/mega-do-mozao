
class MegaDoMozao {
    public static void Main() {

        var numeroMinimo = 1;
        var numeroMaximo = 60;
        var numeroParaEscolher = 6;
        var jogoA = new List<Int32>();

        for (int item = 1; item < numeroParaEscolher+1; item++) {
            var numeroOrdinal = ConverterNumeroOrdinal(item);
            
            Console.Write($"Escolha o {numeroOrdinal} número válido entre {numeroMinimo} e {numeroMaximo}: ");
            var strNumeroEscolhido = Console.ReadLine();
            //var strNumeroEscolhido = "76";
            
            while (!ValidaNumeroEscolhido(numeroMinimo, numeroMaximo, strNumeroEscolhido)) {
                Console.Write($"Por favor, escolha um número válido entre {numeroMinimo} e {numeroMaximo}: ");
                strNumeroEscolhido = Console.ReadLine();
                //strNumeroEscolhido = "41";
            }

            var numeroEscolhido = int.Parse(strNumeroEscolhido);

            jogoA.Add(numeroEscolhido);
        }        
        Console.WriteLine("---------");
        Console.WriteLine("Números escolhidos! Muito obrigado.");
        Console.WriteLine("---------");

        foreach (var numero in jogoA)
        {
            Console.WriteLine(numero);
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