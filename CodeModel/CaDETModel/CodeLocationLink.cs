namespace CodeModel.CaDETModel
{
    public class CodeLocationLink
    {
        public string FileLocation { get; }
        public int StartLoC  { get; }
        public int EndLoC  { get; }

        public CodeLocationLink(string fileLocation, int startLoC, int endLoC)
        {
            FileLocation = fileLocation;
            StartLoC = startLoC;
            EndLoC = endLoC;
        }
    }
}