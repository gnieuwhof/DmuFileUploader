namespace DmuFileUploader.ODataFilter
{
    public interface IODataFilter
    {
        bool IsValid { get; }

        string Expression();
    }
}
