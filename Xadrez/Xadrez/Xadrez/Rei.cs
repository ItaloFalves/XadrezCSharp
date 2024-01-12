
using Xadrez.tabuleiro;
using Xadrez.tabuleiro.Enums;

namespace Xadrez.Xadrez
{
     class Rei : Peca
    {
        public Rei(Cor cor, Tabuleiro tabuleiro) : base(cor, tabuleiro) 
        {
        
        }

        public override string ToString()
        {
            return "R";
        }
    }
}
