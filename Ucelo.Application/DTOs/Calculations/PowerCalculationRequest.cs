using System.ComponentModel.DataAnnotations;

namespace Ucelo.Application.DTOs.Calculations;

public class PowerCalculationRequest
{
    [Required(ErrorMessage = "A altura do elevador é obrigatória")]
    [Range(1, 1000, ErrorMessage = "A altura deve estar entre 1 e 1000 metros")]
    public double Height { get; set; }

    [Required(ErrorMessage = "A rotação do tambor é obrigatória")]
    [Range(1, 1000, ErrorMessage = "A rotação deve estar entre 1 e 1000 rpm")]
    public double Rotation { get; set; }

    [Required(ErrorMessage = "A capacidade do elevador é obrigatória")]
    [Range(1, 10000, ErrorMessage = "A capacidade deve estar entre 1 e 10000 t/h")]
    public double Capacity { get; set; }

    [Range(9.0, 10.0, ErrorMessage = "A gravidade deve estar entre 9 e 10 m/s²")]
    public double Gravity { get; set; } = 9.81;

    [Range(0.1, 1.0, ErrorMessage = "O rendimento geral deve estar entre 0.1 e 1.0")]
    public double GeneralEfficiency { get; set; } = 0.85;

    [Range(0.1, 1.0, ErrorMessage = "A eficiência deve estar entre 0.1 e 1.0")]
    public double Efficiency { get; set; } = 0.9;

    [Range(0.1, 1.0, ErrorMessage = "O fator de potência deve estar entre 0.1 e 1.0")]
    public double PowerFactor { get; set; } = 0.8;

    [Range(1.0, 2.0, ErrorMessage = "O fator de serviço deve estar entre 1.0 e 2.0")]
    public double ServiceFactor { get; set; } = 1.5;

    [MaxLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres")]
    public string Name { get; set; } = string.Empty;
}

public class PowerCalculationResponse
{
    public double PowerKW { get; set; }
    public double PowerCV { get; set; }
    public double Moment { get; set; }
    public double MaxMoment { get; set; }
    public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;
    public int? Id { get; set; }
    public string Name { get; set; } = string.Empty;
}