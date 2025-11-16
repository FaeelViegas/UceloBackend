namespace Ucelo.Application.DTOs.Calculations
{
    public class ComparisonCalculationRequest
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public double Speed { get; set; } // Velocidade (m/s)
        public double ProductDensity { get; set; } // Densidade do produto (kg/m³)

        public int NumberOfRows { get; set; } = 0; // Será substituído pelo valor padrão 1 se for 0
        public double Pitch { get; set; } = 0; // Será substituído pelo PassoRecomendado da caneca se for 0
        public double Filling { get; set; } = 0; // Será substituído pelo valor padrão 95 se for 0
        public double? SelectedBucketUnitPrice { get; set; } // Preço/Unidade da caneca selecionada
        public double? ComparisonBucketUnitPrice { get; set; } // Preço/Unidade da caneca de comparação

        public int SelectedBucketId { get; set; } // ID da caneca selecionada
        public int ComparisonBucketId { get; set; } // ID da caneca para comparação
    }

    public class ComparisonCalculationResponse
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Dados da caneca selecionada
        public BucketDetailsDto SelectedBucket { get; set; } = new BucketDetailsDto();

        // Dados da caneca para comparação
        public BucketDetailsDto ComparisonBucket { get; set; } = new BucketDetailsDto();

        // Diferenças calculadas
        public ComparisonResultDto ComparisonResult { get; set; } = new ComparisonResultDto();

        public DateTime? CalculatedAt { get; set; } = DateTime.UtcNow;
    }

    public class BucketDetailsDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Dimensions { get; set; } = string.Empty;
        public double Volume { get; set; }
        public double EdgeVolume { get; set; }
        public int MaterialId { get; set; }
        public string MaterialName { get; set; } = string.Empty;
        public string Drilling { get; set; } = string.Empty;
        public double Displacement { get; set; }
        public double TractionResistance { get; set; }
        public double RecommendedPitch { get; set; }
        public double? UnitPrice { get; set; }

        public double BucketsPerMeter { get; set; }
        public double Filling { get; set; } // Enchimento (%)
        public int NumberOfRows { get; set; } // Nº de fileiras
        public double Capacity { get; set; } // Capacidade (t/h)
        public double AbrasionResistance { get; set; }
        public double PricePerMeter { get; set; }
    }

    public class ComparisonResultDto
    {
        // Diferenças entre as canecas (valores em %)
        public double DimensionsDifference { get; set; }
        public double VolumeDifference { get; set; }
        public double BucketsPerMeterDifference { get; set; }
        public double FillingDifference { get; set; }
        public double CapacityDifference { get; set; }
        public double EdgeVolumeDifference { get; set; }
        public double AbrasionResistanceDifference { get; set; }
        public double TractionResistanceDifference { get; set; }
        public double DisplacementDifference { get; set; }
        public double PricePerUnitDifference { get; set; }
        public double PricePerMeterDifference { get; set; }
    }
}