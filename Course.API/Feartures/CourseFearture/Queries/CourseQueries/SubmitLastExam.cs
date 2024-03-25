﻿using CourseService.API.Common.ModelDTO;
using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseService.API.Feartures.CourseFearture.Queries.CourseQueries
{
    public class SubmitLastExam : IRequest<IActionResult>
    {
        public int LastExamId { get; set; }
        public int UserId { get; set; } 
        public List<ExamAnswerDto> Questions { get; set; }

        public class SubmitLastExamHandler : IRequestHandler<SubmitLastExam, IActionResult>
        {
            private readonly CourseContext _context;

            public SubmitLastExamHandler(CourseContext context)
            {
                _context = context;
            }
            public async Task<IActionResult> Handle(SubmitLastExam request, CancellationToken cancellationToken)
            {
                int totalQuestions = _context.QuestionExams.Where(c=>c.LastExamId.Equals(request.LastExamId)).Count();
                int correctAnswersCount = 0;
                int totalCorrectAnswers = 0;

                foreach (var questionWithAnswers in request.Questions)
                {
                    // Tìm câu hỏi tương ứng trong cơ sở dữ liệu
                    var dbQuestion = await _context.QuestionExams
                        .Include(q => q.AnswerExams)
                        .FirstOrDefaultAsync(q => q.Id == questionWithAnswers.ExamId);

                    if (dbQuestion == null)
                    {
                        
                        throw new Exception("Không tìm thấy câu hỏi trong cơ sở dữ liệu.");
                    }

                   
                    var correctAnswers = dbQuestion.AnswerExams.Where(a => a.CorrectAnswer == true).Select(a => a.Id).ToList();

                    
                    var selectedAnswers = questionWithAnswers.SelectedAnswerIds;

                  
                    bool isAllCorrectSelected = correctAnswers.SequenceEqual(selectedAnswers);

                   
                    if (isAllCorrectSelected)
                    {
                        totalCorrectAnswers++;
                    }
                }



                double percentage = (double)totalCorrectAnswers / totalQuestions * 100;
                var percentagePass = _context.LastExams.FirstOrDefault(c => c.Id.Equals(request.LastExamId)).PercentageCompleted;
                if(percentage> percentagePass)
                {
                    var comp = new CompletedExam
                    {
                        UserId = request.UserId,
                        LastExamId = request.LastExamId,
                    };
                    _context.CompletedExams.Add(comp);
                    await _context.SaveChangesAsync();

                    return new OkObjectResult("You have passed!");
                }

                return new BadRequestObjectResult("Please try again!") ;
            }
        }
    }
}
