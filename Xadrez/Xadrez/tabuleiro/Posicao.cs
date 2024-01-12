﻿using System;

namespace Xadrez.tabuleiro
{
     class Posicao
    {
        public int Linha { get; set; }
        public int Coluna { get; set; }

        public Posicao()
        {

        }

        public Posicao(int linha, int coluna)
        {
            Linha = linha;
            Coluna = coluna;
        }

        public override string ToString()
        {
            return Linha.ToString()
                +(", ")
                + Coluna.ToString();
        }
    }
}