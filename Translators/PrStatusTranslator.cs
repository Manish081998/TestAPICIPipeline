public static class PrStatusTranslator
{
    public static DeletePrStatusResponse ToDeletePrStatusResponse(string prNumber)
    {
        return new DeletePrStatusResponse { PRNumber = prNumber };
    }
}
