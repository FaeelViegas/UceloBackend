namespace Ucelo.Domain.Entities
{
    public class Material
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public double ValorImpacto { get; private set; }
        public double TensaoEscoamento { get; private set; }
        public double ModuloElasticidade { get; private set; }
        public double Abrasao { get; private set; }
        public double TemperaturaMaxima { get; private set; }
        public bool Ativo { get; private set; }

        private Material()
        {
        }

        public Material(
            string nome,
            double valorImpacto,
            double tensaoEscoamento,
            double moduloElasticidade,
            double abrasao,
            double temperaturaMaxima)
        {
            Nome = nome;
            ValorImpacto = valorImpacto;
            TensaoEscoamento = tensaoEscoamento;
            ModuloElasticidade = moduloElasticidade;
            Abrasao = abrasao;
            TemperaturaMaxima = temperaturaMaxima;
            Ativo = true;
        }
    }
}