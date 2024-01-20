
using Xadrez.tabuleiro;
using Xadrez.tabuleiro.Exception;
using Xadrez.Xadrez;

namespace Xadrez
{
    internal class Program
    {
        static void Main(string[] args)
        {

            try {

                PartidaXadrez partida = new PartidaXadrez();

                while (!partida.terminada) {

                    try {

                        Console.Clear();
                        Tela.ImprimirTabuleiro(partida.tabuleiro);
                        Console.WriteLine();
                        Console.WriteLine("Turno: " + partida.Turno);
                        Console.WriteLine("Aguardando jogada: " + partida.jogadorAtual);


                        Console.WriteLine();
                        Console.Write("Origem: ");
                        Posicao origem = Tela.LerPosicaoXadrez().toPosicao();
                        partida.ValidarPosicaoDeOrigem(origem);

                        bool[,] posicoesPossiveis = partida.tabuleiro.peca(origem).MovimentosPossiveis();
                        Console.Clear();

                        Tela.ImprimirTabuleiro(partida.tabuleiro, posicoesPossiveis);

                        Console.WriteLine();
                        Console.Write("Destino: ");
                        Posicao destino = Tela.LerPosicaoXadrez().toPosicao();
                        partida.ValidarPosicaoDeDestino(origem, destino);

                        partida.RealizaJogada(origem, destino);

                    }
                    catch (TabuleiroException e)
                    {
                        Console.Write(e.Message); 
                        Console.WriteLine();
                        Console.ReadLine();
                    }
                 }


                Tela.ImprimirTabuleiro(partida.tabuleiro);

            }
            catch (TabuleiroException e) { 
                Console.WriteLine(e.Message);
            }
            

            Console.WriteLine();
        }
    }
}