
using Xadrez.tabuleiro;
using Xadrez.tabuleiro.Enums;

namespace Xadrez.Xadrez
{
    class PartidaXadrez
    {
        public Tabuleiro tabuleiro {  get; private set; }
        private int turno;
        private Cor jogadorAtual;
        public bool terminada { get; private set; }

       
        public PartidaXadrez()
        {
            tabuleiro = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;
            ColocarPecas();
        }

        public void ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tabuleiro.retirarPeca(origem);
            p.IncrementarQteMovimentos();
            Peca pecaCapturada = tabuleiro.retirarPeca(destino);
            tabuleiro.ColocarPeca(p, destino);
        }

        private void ColocarPecas()
        {
            tabuleiro.ColocarPeca(new Torre(Cor.Preto, tabuleiro), new PosicaoXadrez('c', 1).toPosicao());

        }
        
    }
}
