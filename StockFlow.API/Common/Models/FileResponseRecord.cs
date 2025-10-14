namespace StockFlow.Common.Models;
public sealed record FileResponseRecord
{
    public string FileName { get; init; } = null!;

    public byte[] File { get; init; } = null!;

    public string? FileFormat { get; init; }
}
