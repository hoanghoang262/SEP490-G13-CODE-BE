﻿using System;
using System.Collections.Generic;

namespace CourseGRPC.Models
{
    public partial class CompletedPracticeQuestion
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? PracticeQuestionId { get; set; }
    }
}
