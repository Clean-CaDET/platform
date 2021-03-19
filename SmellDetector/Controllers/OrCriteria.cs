using System;
namespace SmellDetector.Controllers
{
    public class OrCriteria:Criteria
    {
        private Criteria firstCriteria;
        private Criteria secondCriteria;
        public OrCriteria(Criteria firstCriteria, Criteria secondCriteria)
        {
            this.firstCriteria = firstCriteria;
            this.secondCriteria = secondCriteria;
        }

        public bool meetCriteria()
        {
            return firstCriteria.meetCriteria() || secondCriteria.meetCriteria();
        }
    }
}
