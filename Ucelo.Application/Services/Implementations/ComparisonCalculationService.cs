using System.Text.Json;
using Ucelo.Application.DTOs.Calculations;
using Ucelo.Application.Services.Interfaces;
using Ucelo.Domain.Entities;
using Ucelo.Domain.Enums;
using Ucelo.Domain.Interfaces;

namespace Ucelo.Application.Services.Implementations
{
    public class ComparisonCalculationService : IComparisonCalculationService
    {
        private readonly ICalculationRepository _calculationRepository;
        private readonly IBucketRepository _bucketRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly JsonSerializerOptions _jsonOptions;

        public ComparisonCalculationService(
            ICalculationRepository calculationRepository,
            IBucketRepository bucketRepository,
            IMaterialRepository materialRepository,
            IUnitOfWork unitOfWork)
        {
            _calculationRepository = calculationRepository;
            _bucketRepository = bucketRepository;
            _materialRepository = materialRepository;
            _unitOfWork = unitOfWork;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };
        }

        public async Task<ComparisonCalculationResponse> CalculateAsync(ComparisonCalculationRequest request)
        {
            if (request.SelectedBucketId <= 0 || request.ComparisonBucketId <= 0)
            {
                throw new ArgumentException("É necessário selecionar ambas as canecas para comparação.");
            }

            // Carregar dados das canecas do banco
            var selectedBucket = await _bucketRepository.GetByIdAsync(request.SelectedBucketId);
            var comparisonBucket = await _bucketRepository.GetByIdAsync(request.ComparisonBucketId);

            if (selectedBucket == null || comparisonBucket == null)
            {
                throw new ArgumentException("Uma ou ambas as canecas selecionadas não foram encontradas.");
            }

            // Carregar dados dos materiais
            var selectedMaterial = await _materialRepository.GetByIdAsync(selectedBucket.MaterialId);
            var comparisonMaterial = await _materialRepository.GetByIdAsync(comparisonBucket.MaterialId);

            if (selectedMaterial == null || comparisonMaterial == null)
            {
                throw new ArgumentException("Um ou ambos os materiais das canecas não foram encontrados.");
            }

            // Usar valores do banco se não forem fornecidos pelo usuário
            double pitch = request.Pitch > 0 ? request.Pitch : selectedBucket.PassoRecomendado;
            double filling = request.Filling > 0 && request.Filling <= 100 ? request.Filling : 95;
            int numberOfRows = request.NumberOfRows > 0 ? request.NumberOfRows : 1;
            double? selectedUnitPrice = request.SelectedBucketUnitPrice ?? selectedBucket.UnitPrice;
            double? comparisonUnitPrice = request.ComparisonBucketUnitPrice ?? comparisonBucket.UnitPrice;

            // Calcular valores para a caneca selecionada
            var selectedBucketDetails = CalculateBucketDetails(
                selectedBucket,
                selectedMaterial,
                request.Speed,
                request.ProductDensity,
                numberOfRows,
                pitch,
                filling,
                selectedUnitPrice);

            // Calcular valores para a caneca de comparação
            var comparisonBucketDetails = CalculateBucketDetails(
                comparisonBucket,
                comparisonMaterial,
                request.Speed,
                request.ProductDensity,
                numberOfRows,
                pitch,
                filling,
                comparisonUnitPrice);

            // Calcular as diferenças percentuais entre as canecas
            var comparisonResult = CalculateComparison(selectedBucketDetails, comparisonBucketDetails);

            return new ComparisonCalculationResponse
            {
                Name = request.Name,
                SelectedBucket = selectedBucketDetails,
                ComparisonBucket = comparisonBucketDetails,
                ComparisonResult = comparisonResult,
                CalculatedAt = DateTime.UtcNow
            };
        }

        private BucketDetailsDto CalculateBucketDetails(
            Bucket bucket,
            Material material,
            double speed,
            double productDensity,
            int numberOfRows,
            double pitch,
            double filling,
            double? unitPriceOverride = null)
        {
            // Extrair as dimensões da caneca
            var dimensions = bucket.Dimensions.Split('x').Select(double.Parse).ToArray();

            // Usar o passo informado pelo usuário
            double bucketsPerMeter = 1000 / pitch;

            // Calcular capacidade (t/h) com os valores do usuário
            // Corrigido para considerar unidades corretas (volume em litros para m³)
            double capacity = speed * productDensity * (bucket.Volume / 1000) * bucketsPerMeter * (filling / 100) * 3.6;

            // Resistência à abrasão
            double abrasionResistance = material.Abrasao;

            // Usar o preço unitário informado pelo usuário, se fornecido
            double unitPrice = unitPriceOverride ?? bucket.UnitPrice ?? 0;

            // Calcular preço por metro
            double pricePerMeter = unitPrice * bucketsPerMeter;

            return new BucketDetailsDto
            {
                Id = bucket.Id,
                Code = bucket.Codigo,
                Dimensions = bucket.Dimensions,
                Volume = bucket.Volume,
                EdgeVolume = bucket.VolumeBorda,
                MaterialId = bucket.MaterialId,
                MaterialName = material.Nome,
                Drilling = bucket.Furacao,
                Displacement = bucket.Deslocamento,
                TractionResistance = bucket.ResistenciaTracao,
                RecommendedPitch = bucket.PassoRecomendado,
                UnitPrice = unitPrice,
                BucketsPerMeter = Math.Round(bucketsPerMeter, 2),
                Filling = filling,
                Capacity = Math.Round(capacity, 2),
                AbrasionResistance = abrasionResistance,
                PricePerMeter = Math.Round(pricePerMeter, 2),
                NumberOfRows = numberOfRows
            };
        }

        private ComparisonResultDto CalculateComparison(BucketDetailsDto selected, BucketDetailsDto comparison)
        {
            // Função auxiliar para calcular diferença percentual
            double CalculatePercentDifference(double value1, double value2) =>
                value1 > 0 ? ((value2 - value1) / value1) * 100 : 0;

            // Extração das dimensões para cálculo da diferença
            var selectedDimensions = selected.Dimensions.Split('x').Select(double.Parse).ToArray();
            var comparisonDimensions = comparison.Dimensions.Split('x').Select(double.Parse).ToArray();

            // Cálculo da diferença média das dimensões
            double dimensionsDifference = 0;
            for (int i = 0; i < Math.Min(selectedDimensions.Length, comparisonDimensions.Length); i++)
            {
                dimensionsDifference += CalculatePercentDifference(selectedDimensions[i], comparisonDimensions[i]);
            }

            dimensionsDifference /= Math.Min(selectedDimensions.Length, comparisonDimensions.Length);

            return new ComparisonResultDto
            {
                DimensionsDifference = dimensionsDifference,
                VolumeDifference = CalculatePercentDifference(selected.Volume, comparison.Volume),
                BucketsPerMeterDifference =
                    CalculatePercentDifference(selected.BucketsPerMeter, comparison.BucketsPerMeter),
                FillingDifference = CalculatePercentDifference(selected.Filling, comparison.Filling),
                CapacityDifference = CalculatePercentDifference(selected.Capacity, comparison.Capacity),
                EdgeVolumeDifference = CalculatePercentDifference(selected.EdgeVolume, comparison.EdgeVolume),
                AbrasionResistanceDifference =
                    CalculatePercentDifference(selected.AbrasionResistance, comparison.AbrasionResistance),
                TractionResistanceDifference =
                    CalculatePercentDifference(selected.TractionResistance, comparison.TractionResistance),
                DisplacementDifference = CalculatePercentDifference(selected.Displacement, comparison.Displacement),
                PricePerUnitDifference = selected.UnitPrice.HasValue && comparison.UnitPrice.HasValue
                    ? CalculatePercentDifference(selected.UnitPrice.Value, comparison.UnitPrice.Value)
                    : 0,
                PricePerMeterDifference = CalculatePercentDifference(selected.PricePerMeter, comparison.PricePerMeter)
            };
        }

        public async Task<ComparisonCalculationResponse> SaveCalculationAsync(int userId,
            ComparisonCalculationRequest request)
        {
            var result = await CalculateAsync(request);

            var calculationName = string.IsNullOrWhiteSpace(request.Name)
                ? $"Comparativo de Canecas - {DateTime.UtcNow:dd/MM/yyyy HH:mm}"
                : request.Name;

            var inputData = JsonSerializer.Serialize(request, _jsonOptions);
            var resultData = JsonSerializer.Serialize(result, _jsonOptions);

            var calculation = new Calculation(
                userId,
                calculationName,
                CalculationType.Comparison,
                inputData,
                resultData
            );

            await _calculationRepository.AddAsync(calculation);
            await _unitOfWork.CommitAsync();

            result.Id = calculation.Id;
            return result;
        }

        public async Task<IEnumerable<ComparisonCalculationResponse>> GetUserCalculationsAsync(int userId)
        {
            var calculations = await _calculationRepository.GetByUserIdAndTypeAsync(
                userId,
                CalculationType.Comparison
            );

            var results = new List<ComparisonCalculationResponse>();

            foreach (var calculation in calculations)
            {
                var response = JsonSerializer.Deserialize<ComparisonCalculationResponse>(
                    calculation.ResultData,
                    _jsonOptions
                );

                if (response != null)
                {
                    response.Id = calculation.Id;
                    response.Name = calculation.Name;
                    results.Add(response);
                }
            }

            return results;
        }

        public async Task<ComparisonCalculationResponse> GetByIdAsync(int id)
        {
            var calculation = await _calculationRepository.GetByIdAsync(id);

            if (calculation == null || calculation.CalculationType != CalculationType.Comparison)
            {
                return null;
            }

            var response = JsonSerializer.Deserialize<ComparisonCalculationResponse>(
                calculation.ResultData,
                _jsonOptions
            );

            if (response != null)
            {
                response.Id = calculation.Id;
                response.Name = calculation.Name;
            }

            return response;
        }
    }
}