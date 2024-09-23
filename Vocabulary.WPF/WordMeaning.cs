namespace Vocabulary.WPF
{
    public record WordMeaning (Guid Id, string Name, Guid MeaningId, string Description, string? Example, string? UserId)
    {
        public WordMeaning() : this(Guid.Empty, string.Empty, Guid.Empty, string.Empty, string.Empty, null)
        {
        }

        public bool IsEmpty()
        {
            return Id == Guid.Empty && MeaningId == Guid.Empty;
        }
    }
}
