using System;
using Xadrez.tabuleiro;
using Xadrez.tabuleiro.Enums;
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
                
                    Console.Clear();
                    Tela.ImprimirTabuleiro(partida.tabuleiro);

                    Console.WriteLine();
                    Console.Write("Origem: ");
                    Posicao origem = Tela.LerPosicaoXadrez().toPosicao();
                    Console.Write("Destino: ");
                    Posicao destino = Tela.LerPosicaoXadrez() .toPosicao();

                    partida.ExecutaMovimento(origem, destino);

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