namespace FlowTracker.Shared.Dtos.Transaction
{
    public class UpdateTransactionRequest
    {
        public int Id { get; set; }
        public required decimal Amount { get; set; }
        public DateOnly Date { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
    }
}
