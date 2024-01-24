
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
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;
        public bool xeque {  get; private set; }

       
        public PartidaXadrez()
        {
            tabuleiro = new Tabuleiro(8, 8);
            Turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;
            xeque = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public Peca ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tabuleiro.retirarPeca(origem);
            p.IncrementarQteMovimentos();
            Peca pecaCapturada = tabuleiro.retirarPeca(destino);
            tabuleiro.ColocarPeca(p, destino);
            if(pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }
            return pecaCapturada;
        }

        public void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tabuleiro.retirarPeca(destino);
            p.DecrementarQteMovimentos();
            if (pecaCapturada != null)
            {
                tabuleiro.ColocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }

            tabuleiro.ColocarPeca(p, origem);
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutaMovimento(origem, destino);
            if (EstaEmXeque(jogadorAtual))
            {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            if (EstaEmXeque(Adversario(jogadorAtual)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }

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

        public HashSet<Peca> pecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach(Peca x in capturadas)
            {
                if(x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> PecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in pecas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(pecasCapturadas(cor));
            return aux;
        }

        private Cor Adversario(Cor cor)
        {
            if (cor == Cor.Branca)
            {
                return Cor.Preto;
            }
            else
            {
                return Cor.Branca;
            }
        }

        private Peca PecaRei(Cor cor)
        {
            foreach (Peca x in PecasEmJogo(cor))
            {
                if ( x is Rei)
                {
                    return x;
                }
            }
            return null;
        }  

        public bool EstaEmXeque(Cor cor)
        {
            Peca r = PecaRei(cor);

            if(r == null)
            {
                throw new TabuleiroException("Não tem Rei da cor " + cor + " no tabuleiro!");
            }

            foreach(Peca x in PecasEmJogo(Adversario(cor)))
            {
                bool[,] mat = x.MovimentosPossiveis();
                if (mat[r.Posicao.Linha, r.Posicao.Coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tabuleiro.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }

        private void ColocarPecas()
        {
            ColocarNovaPeca('c', 1, new Torre(Cor.Branca, tabuleiro));
            ColocarNovaPeca('c', 2, new Torre(Cor.Branca, tabuleiro));
            ColocarNovaPeca('c', 3, new Rei(Cor.Branca, tabuleiro));

            ColocarNovaPeca('c', 7, new Torre(Cor.Preto, tabuleiro));
            ColocarNovaPeca('d', 8, new Rei(Cor.Preto, tabuleiro));


        }
        
    }
}
