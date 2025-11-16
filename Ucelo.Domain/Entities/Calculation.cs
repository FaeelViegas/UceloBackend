using Ucelo.Domain.Enums;

namespace Ucelo.Domain.Entities;

public class Calculation
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public string Name { get; private set; }
    public CalculationType CalculationType { get; private set; }
    public string InputData { get; private set; }
    public string ResultData { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Calculation()
    {
    }

    public Calculation(
        int userId,
        string name,
        CalculationType calculationType,
        string inputData,
        string resultData)
    {
        UserId = userId;
        Name = name;
        CalculationType = calculationType;
        InputData = inputData;
        ResultData = resultData;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string inputData, string resultData)
    {
        Name = name;
        InputData = inputData;
        ResultData = resultData;
        UpdatedAt = DateTime.UtcNow;
    }
}