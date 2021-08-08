using System;
using SkillTrail.Shared.Domain.Abstract;
using SkillTrail.Shared.Domain.Utils;

namespace SkillTrail.Modules.Lms.Domain.Entities
{
    public class CourseProgress : AggregateRoot<int>
    {
        public CourseProgress()
        {
        }
        
        public CourseProgress(int courseId, int studentId)
        {
            CourseId = courseId;
            StudentId = studentId;
            StartedAt = DateTimeProvider.Now;
        }

        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
    }
}