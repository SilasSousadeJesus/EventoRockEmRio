

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class Usuario
{
    public string Login { get; set; }
    public string Senha { get; set; }
}

public class Program
{
    private const int CapacidadeEvento = 2;
    private const int TempoLimiteCompraSegundos = 20;

    private static Queue<string> filaEspera = new Queue<string>();
    private static int ingressosDisponiveis = CapacidadeEvento;
    private static List<Usuario> usuarios = new List<Usuario>();

    public static void Main(string[] args)
    {
        Console.WriteLine("Bem-vindo ao sistema de venda de ingressos para o Rock in Rio!");

        CriarUsuario();

        while (true)
        {
            var usuario = FazerLogin();
            if (usuario != null)
            {
                Console.WriteLine("\nPressione qualquer tecla para entrar na fila de espera ou 'S' para sair:");
                var key = Console.ReadKey();

                if (key.KeyChar.ToString().ToUpper() == "S")
                {
                    break;
                }

                filaEspera.Enqueue(usuario.Login);
                Console.WriteLine($"\nVocê entrou na fila de espera. Posição na fila: {filaEspera.Count}");

                if (ingressosDisponiveis > 0)
                {
                    Thread.Sleep(2000);


                    ComprarIngresso(usuario);
                }
                else
                {
                    Console.WriteLine("Desculpe, todos os ingressos foram vendidos. Por favor, tente novamente mais tarde.");
                }
            }
            else
            {
                Console.WriteLine("\nLogin ou senha incorretos. Tente novamente.");
            }
        }

        Console.WriteLine("\nObrigado por usar nosso sistema de venda de ingressos. Até mais!");
    }

    private static void CriarUsuario()
    {
        while (true)
        {
            Console.WriteLine("\nCrie seu login e senha:");
            Console.Write("Login: ");
            string login = Console.ReadLine();
            Console.Write("Senha: ");
            string senha = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(senha))
            {
                usuarios.Add(new Usuario { Login = login, Senha = senha });
                Console.WriteLine("Usuário criado com sucesso!\n");
                break;
            }
            else
            {
                Console.WriteLine("Login e senha não podem ser em branco. Por favor, tente novamente.\n");
            }
        }
    }

    private static Usuario FazerLogin()
    {
        Console.WriteLine("\nFaça login:");
        Console.Write("Login: ");
        string login = Console.ReadLine();
        Console.Write("Senha: ");
        string senha = Console.ReadLine();

        return usuarios.FirstOrDefault(u => u.Login == login && u.Senha == senha);
    }

    private static void ComprarIngresso(Usuario usuario)
    {
        Console.WriteLine($"\n{usuario.Login}, é sua vez de comprar o ingresso!");
        var tempoInicioCompra = DateTime.Now;
        Console.WriteLine($"Pressione Enter para comprar o ingresso. Você tem 60 segundos.");
        Console.WriteLine("Tempo restante: 60 segundos");

        while ((DateTime.Now - tempoInicioCompra).TotalSeconds < TempoLimiteCompraSegundos)
        {
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                ingressosDisponiveis--;
                Console.WriteLine($"{usuario.Login}, seu ingresso foi comprado com sucesso!");
                Console.WriteLine($"Ingressos restantes: {ingressosDisponiveis}");
                return;
            }
            else
            {
                int tempoRestante = TempoLimiteCompraSegundos - (int)(DateTime.Now - tempoInicioCompra).TotalSeconds;
                if (tempoRestante > 0)
                {
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.WriteLine($"Tempo restante: {tempoRestante} segundos");
                }
            }

            Thread.Sleep(1000);
        }

        Console.WriteLine("Tempo limite de compra expirado. Seu ingresso não pôde ser comprado.");
    }
}
