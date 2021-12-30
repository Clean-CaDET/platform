using CodeModel.CaDETModel.CodeItems;

namespace DataSetExplorer.Core.DataSets.Model
{
    internal class CoupledClassStrength
    {
        public CaDETClass CoupledClass { get; private set; }
        public int CouplingStrength { get; private set; }

        public CoupledClassStrength() { }

        public CoupledClassStrength(CaDETClass coupledClass, int couplingStrength)
        {
            CoupledClass = coupledClass;
            CouplingStrength = couplingStrength;
        }
    }
}