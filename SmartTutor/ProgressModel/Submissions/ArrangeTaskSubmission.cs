using System.Collections.Generic;

namespace SmartTutor.ProgressModel.Submissions
{
    public class ArrangeTaskSubmission : Submission
    {
        public int ArrangeTaskId { get; private set; }
        public List<ArrangeTaskContainerSubmission> Containers { get; private set; }
        private ArrangeTaskSubmission() {}
        public ArrangeTaskSubmission(int arrangeTaskId, List<ArrangeTaskContainerSubmission> container): this()
        {
            ArrangeTaskId = arrangeTaskId;
            Containers = container;
        }
    }
}