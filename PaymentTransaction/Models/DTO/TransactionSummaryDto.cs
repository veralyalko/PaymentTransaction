public class TransactionSummaryDto
{
  
    public int TotalTransactions { get; set; }
    public List<ProviderVolumeDto> VolumePerProvider { get; set; } = new();
    public List<StatusBreakdownDto> StatusBreakdown { get; set; } = new();
}