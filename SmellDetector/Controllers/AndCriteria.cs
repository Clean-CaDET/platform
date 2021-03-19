using System;
namespace SmellDetector.Controllers
{
    public class AndCriteria:Criteria
    {
        private Criteria firstCriteria;
        private Criteria secondCriteria;
        public AndCriteria(Criteria firstCriteria, Criteria secondCriteria)
        {
            this.firstCriteria = firstCriteria;
            this.secondCriteria = secondCriteria;
        }

        public bool meetCriteria()
        {
            return firstCriteria.meetCriteria() && secondCriteria.meetCriteria();
        }
    }
}
