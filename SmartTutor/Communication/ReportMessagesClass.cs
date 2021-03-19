using SmartTutor.ContentModel;
using System;
using System.Collections.Generic;

namespace SmartTutor.Communication
{
    public sealed class ReportMessagesClass
    {
        public Dictionary<Guid, EducationContent> ReportMessages { get; set; }

        ReportMessagesClass()
        {
        }

        private static readonly object padlock = new object();
        private static ReportMessagesClass instance = null;
        public static ReportMessagesClass Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ReportMessagesClass();
                        instance.ReportMessages = new Dictionary<Guid, EducationContent>();
                    }

                    return instance;
                }
            }
        }
    }
}
