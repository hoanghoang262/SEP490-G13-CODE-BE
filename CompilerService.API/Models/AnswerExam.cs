﻿using System;
using System.Collections.Generic;

namespace CompilerService.API.Models
{
    public partial class AnswerExam
    {
        public int Id { get; set; }
        public int? ExamId { get; set; }
        public string? OptionsText { get; set; }
        public bool? CorrectAnswer { get; set; }

        public virtual QuestionExam? Exam { get; set; }
    }
}
