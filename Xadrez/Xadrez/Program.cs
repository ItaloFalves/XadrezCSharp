using System;
using Xadrez.tabuleiro;
using Xadrez.tabuleiro.Enums;
using Xadrez.Xadrez;

namespace Xadrez
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Tabuleiro tabuleiro = new Tabuleiro(8, 8);
            tabuleiro.ColocarPeca(new Rei(Cor.Laranja, tabuleiro), new Posicao(0, 0));

            Tela.ImprimirTabuleiro(tabuleiro);

            Console.WriteLine();
        }
    }
}