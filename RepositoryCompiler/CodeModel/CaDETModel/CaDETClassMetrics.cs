namespace RepositoryCompiler.CodeModel.CaDETModel
{
    public class CaDETClassMetrics
    {
        public int LOC { get; set; }
        public double? LCOM { get; set; }
        /*{
            int maxCohesion = NAD() * NMD();
            if (maxCohesion == 0) return null;
            int fieldsAccessed = 1;
            return 1 - fieldsAccessed / (numberOfFields * numberOfMethods);
        }*/

        public int NMD { get; set; }
        public int NAD { get; set; }
        public int WMC { get; set; }
    }
}