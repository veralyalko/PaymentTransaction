public class TransactionSummaryDto
{
    public int TotalTransactions { get; set; }

    public List<ProviderVolumeDto> VolumePerProvider { get; set; } = new();

    public List<StatusBreakdownDto> StatusBreakdown { get; set; } = new();
}

// public class ProviderVolumeDto
// {
//     public required string ProviderName { get; set; }
//     public double TotalAmount { get; set; }
// }

// public class StatusBreakdownDto
// {
//     public required string StatusName { get; set; }
//     public int Count { get; set; }
// }