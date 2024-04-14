namespace MessagesBetweenManagerAndWorker
{
    public class HashCodeMessage(int MaxLengthValue, string HashCodeValue, string IdValue, char[] LetterCheckArray)
    {
        public int MaxLengthValue { get; set; } = MaxLengthValue;
        public string? HashCodeValue { get; set; } = HashCodeValue;
        public char[] LetterCheckArray { get; set; } = LetterCheckArray;
        public string? IdValue { get; set; } = IdValue;
    }
}
