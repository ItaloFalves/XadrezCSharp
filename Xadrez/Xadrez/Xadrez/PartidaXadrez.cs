
using Xadrez.tabuleiro;
using Xadrez.tabuleiro.Enums;
using Xadrez.tabuleiro.Exception;

namespace Xadrez.Xadrez
{
    class PartidaXadrez
    {
        public Tabuleiro tabuleiro {  get; private set; }
        public int Turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool terminada { get; private set; }

       
        public PartidaXadrez()
        {
            tabuleiro = new Tabuleiro(8, 8);
            Turno = 1;
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

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            ExecutaMovimento(origem, destino);
            Turno++;
            MudaJogador();
        }

        public void ValidarPosicaoDeOrigem(Posicao posicao)
        {
            if(tabuleiro.peca(posicao) == null){
                throw new TabuleiroException("Não existe peça na posição de origem escolhida!");
            }
            if(jogadorAtual != tabuleiro.peca(posicao).Cor)
            {
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            }
            if (!tabuleiro.peca(posicao).ExisteMovimentosPossiveis())
            {
                throw new TabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");
            }
        }

        public void ValidarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!tabuleiro.peca(origem).PodeMoverPara(destino)){
                throw new TabuleiroException("Posição inválida!");
            }
        }

        private void MudaJogador()
        {
            if(jogadorAtual == Cor.Branca)
            {
                jogadorAtual = Cor.Preto;
            }
            else
            {
                jogadorAtual = Cor.Branca;
            }
        }

        private void ColocarPecas()
        {
            tabuleiro.ColocarPeca(new Torre(Cor.Preto, tabuleiro), new PosicaoXadrez('c', 1).toPosicao());
            tabuleiro.ColocarPeca(new Rei(Cor.Preto, tabuleiro), new PosicaoXadrez('d', 5).toPosicao());

            tabuleiro.ColocarPeca(new Rei(Cor.Branca, tabuleiro), new PosicaoXadrez('d', 8).toPosicao());

        }
        
    }
}
