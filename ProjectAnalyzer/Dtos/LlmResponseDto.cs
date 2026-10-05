namespace ProjectAnalyzer.Dtos;

internal record LlmResponseDto
{
    public required IEnumerable<ResultDto> Results { get; set; }

    public record ResultDto
    {
        public required string RuleId { get; set; }
        public required string RuleDescription { get; set; }
        public required string Message { get; set; }
        public required SarifDto.SeverityLevel Level { get; set; }
        public required string Path { get; set; }
        public required string Category { get; set; }

        public int StartLine { get; set; } = 1;
        public int EndLine { get; set; } = 1;

        public int StartColumn { get; set; } = 1;
        public int EndColumn { get; set; } = 1;
    }
}