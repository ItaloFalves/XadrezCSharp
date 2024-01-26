
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

            //Jogada especial roque pequeno
            if(p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemTorre = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoTorre = new Posicao(origem.Linha, origem.Coluna +1);
                Peca torre = tabuleiro.retirarPeca(origemTorre);
                torre.IncrementarQteMovimentos();
                tabuleiro.ColocarPeca(torre, destinoTorre);
            }

            //Jogada especial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemTorre = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoTorre = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca torre = tabuleiro.retirarPeca(origemTorre);
                torre.IncrementarQteMovimentos();
                tabuleiro.ColocarPeca(torre, destinoTorre);
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

            //Jogada especial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemTorre = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoTorre = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca torre = tabuleiro.retirarPeca(destinoTorre);
                torre.DecrementarQteMovimentos();
                tabuleiro.ColocarPeca(torre, origemTorre);
            }

            //Jogada especial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemTorre = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoTorre = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca torre = tabuleiro.retirarPeca(destinoTorre);
                torre.DecrementarQteMovimentos();
                tabuleiro.ColocarPeca(torre, origemTorre);
            }
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutaMovimento(origem, destino);

            if (EstaEmXeque(jogadorAtual))
            {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            Peca p = tabuleiro.peca(destino);

            if(p is Peao)
            {
                if ((p.Cor == Cor.Branca && destino.Linha == 0) || (p.Cor == Cor.Preto && destino.Linha == 7))
                {
                    p = tabuleiro.retirarPeca(destino);
                    pecas.Remove(p);
                    Peca dama = new Dama(tabuleiro, p.Cor);
                    tabuleiro.ColocarPeca(dama, destino);
                    pecas.Add(dama);
                }
            }

            if (EstaEmXeque(Adversario(jogadorAtual)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }

            if (TesteXequeMate(Adversario(jogadorAtual)))
            {
                terminada = true;
            }
            else
            {
                Turno++;
                MudaJogador();
            }
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
            if (!tabuleiro.peca(origem).MovimentoPossivel(destino)){
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

        public bool TesteXequeMate(Cor cor)
        {
            if(!EstaEmXeque(cor))
            {
                return false;
            }
            foreach(Peca x in PecasEmJogo(cor))
            {
                bool[,] mat = x.MovimentosPossiveis();
                for(int i = 0; i < tabuleiro.Linhas; i++)
                {
                    for(int j = 0; j < tabuleiro.Colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool testeXeque = EstaEmXeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tabuleiro.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }

        private void ColocarPecas()
        {
            ColocarNovaPeca('a', 1, new Torre(Cor.Branca, tabuleiro));
            ColocarNovaPeca('b', 1, new Cavalo(tabuleiro, Cor.Branca));
            ColocarNovaPeca('c', 1, new Bispo(tabuleiro, Cor.Branca));
            ColocarNovaPeca('d', 1, new Dama(tabuleiro, Cor.Branca));
            ColocarNovaPeca('e', 1, new Rei(Cor.Branca, tabuleiro, this));
            ColocarNovaPeca('f', 1, new Bispo(tabuleiro, Cor.Branca));
            ColocarNovaPeca('g', 1, new Cavalo(tabuleiro, Cor.Branca));
            ColocarNovaPeca('h', 1, new Torre(Cor.Branca, tabuleiro));
            ColocarNovaPeca('a', 2, new Peao(tabuleiro, Cor.Branca));
            ColocarNovaPeca('b', 2, new Peao(tabuleiro, Cor.Branca));
            ColocarNovaPeca('c', 2, new Peao(tabuleiro, Cor.Branca));
            ColocarNovaPeca('d', 2, new Peao(tabuleiro, Cor.Branca));
            ColocarNovaPeca('e', 2, new Peao(tabuleiro, Cor.Branca));
            ColocarNovaPeca('f', 2, new Peao(tabuleiro, Cor.Branca));
            ColocarNovaPeca('g', 2, new Peao(tabuleiro, Cor.Branca));
            ColocarNovaPeca('h', 2, new Peao(tabuleiro, Cor.Branca));

            ColocarNovaPeca('a', 8, new Torre(Cor.Preto, tabuleiro));
            ColocarNovaPeca('b', 8, new Cavalo(tabuleiro, Cor.Preto));
            ColocarNovaPeca('c', 8, new Bispo(tabuleiro, Cor.Preto));
            ColocarNovaPeca('d', 8, new Dama(tabuleiro, Cor.Preto));
            ColocarNovaPeca('e', 8, new Rei(Cor.Preto, tabuleiro, this));
            ColocarNovaPeca('f', 8, new Bispo(tabuleiro, Cor.Preto));
            ColocarNovaPeca('g', 8, new Cavalo(tabuleiro, Cor.Preto));
            ColocarNovaPeca('h', 8, new Torre(Cor.Preto, tabuleiro));
            ColocarNovaPeca('a', 7, new Peao(tabuleiro, Cor.Preto));
            ColocarNovaPeca('b', 7, new Peao(tabuleiro, Cor.Preto));
            ColocarNovaPeca('c', 7, new Peao(tabuleiro, Cor.Preto));
            ColocarNovaPeca('d', 7, new Peao(tabuleiro, Cor.Preto));
            ColocarNovaPeca('e', 7, new Peao(tabuleiro, Cor.Preto));
            ColocarNovaPeca('f', 7, new Peao(tabuleiro, Cor.Preto));
            ColocarNovaPeca('g', 7, new Peao(tabuleiro, Cor.Preto));
            ColocarNovaPeca('h', 7, new Peao(tabuleiro, Cor.Preto));

        }
        
    }
}
