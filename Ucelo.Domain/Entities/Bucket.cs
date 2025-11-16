namespace Ucelo.Domain.Entities
{
    public class Bucket
    {
        public int Id { get; private set; }
        public string Codigo { get; private set; }
        public string Dimensions { get; private set; }
        public double Volume { get; private set; }
        public double VolumeBorda { get; private set; }
        public int MaterialId { get; private set; }
        public string Furacao { get; private set; }
        public double Deslocamento { get; private set; }
        public double ResistenciaTracao { get; private set; }
        public double PassoRecomendado { get; private set; }
        public double? UnitPrice { get; private set; }
        public bool Ativo { get; private set; }

        public Material Material { get; private set; }

        private Bucket()
        {
        }

        public Bucket(
            string codigo,
            string dimensions,
            double volume,
            double volumeBorda,
            int materialId,
            string furacao,
            double deslocamento,
            double resistenciaTracao,
            double passoRecomendado,
            double? unitPrice = null)
        {
            Codigo = codigo;
            Dimensions = dimensions;
            Volume = volume;
            VolumeBorda = volumeBorda;
            MaterialId = materialId;
            Furacao = furacao;
            Deslocamento = deslocamento;
            ResistenciaTracao = resistenciaTracao;
            PassoRecomendado = passoRecomendado;
            UnitPrice = unitPrice;
            Ativo = true;
        }
    }
}