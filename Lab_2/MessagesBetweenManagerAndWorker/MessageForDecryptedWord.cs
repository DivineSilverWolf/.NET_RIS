namespace MessagesBetweenManagerAndWorker
{
    public record MessageForDecryptedWord(string Word, string Id, char[] LetterCheckArray, bool ErrorTimeoutFlag = false)
    {
        public bool ErrorTimeoutFlag { get; set; } = ErrorTimeoutFlag;
        public string? Word { get; set; } = Word;
        public string? Id { get; set; } = Id;
        public char[] LetterCheckArray { get; set; } = LetterCheckArray;
    }
}
