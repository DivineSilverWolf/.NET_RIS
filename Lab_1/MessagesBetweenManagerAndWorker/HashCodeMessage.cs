namespace MessagesBetweenManagerAndWorker
{
    public class HashCodeMessage(int MaxLengthValue, string HashCodeValue, string IdValue)
    {
        public int MaxLengthValue { get; set; } = MaxLengthValue;
        public string? HashCodeValue { get; set; } = HashCodeValue;
        public string? IdValue { get; set; } = IdValue;
    }
}
