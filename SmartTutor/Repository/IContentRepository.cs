using SmartTutor.ContentModel;
using System;
using System.Collections.Generic;

namespace SmartTutor.Repository
{
    public interface IContentRepository
    {
        /// <param name="smellType"> issue </param>
        /// <param name="indexOfContent"> Index of content in list of contents for some type of smell </param>
        /// <returns></returns>
        List<EducationContent> FindEducationalContent(SmellType smellType);
    }
}


