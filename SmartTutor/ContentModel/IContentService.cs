﻿using SmartTutor.ContentModel.LectureModel;
using System.Collections.Generic;
using SmartTutor.ContentModel.TraineeModel;

namespace SmartTutor.ContentModel
{
    public interface IContentService
    {
        List<Lecture> GetLectures();
        List<NodeProgress> GetKnowledgeNodes(int lectureId, int? traineeId);
    }
}