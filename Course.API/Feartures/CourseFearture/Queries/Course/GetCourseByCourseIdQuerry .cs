﻿using GrpcServices;
using CourseService;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CourseService.API.Common.ModelDTO;
using Microsoft.EntityFrameworkCore;
using CourseService.API.Models;

namespace CourseService.API.Feartures.CourseFearture.Queries
{
    public class GetCourseByCourseIdQuerry : IRequest<IActionResult>
    {
        public int CourseId { get; set; }

        public class GetCourseByUserHandler : IRequestHandler<GetCourseByCourseIdQuerry, IActionResult>
        {
            private readonly CourseContext _context;
            private readonly UserIdCourseGrpcService service;
            private readonly IMapper mapper;

            public GetCourseByUserHandler(CourseContext context, UserIdCourseGrpcService userIdCourseGrpcService, IMapper _mapper)
            {
                _context = context;
                service = userIdCourseGrpcService;
                mapper = _mapper;

            }
            public async Task<IActionResult> Handle(GetCourseByCourseIdQuerry request, CancellationToken cancellationToken)
            {
                var courses = _context.Courses
                          .Include(course => course.Enrollments)
                          .Include(course => course.Chapters)
                          .ThenInclude(chapter => chapter.Lessons)
                          .ThenInclude(lesson => lesson.Questions)
                          .Include(course => course.Chapters)
                          .ThenInclude(chapter => chapter.CodeQuestions)
                          .ThenInclude(codeQuestion => codeQuestion.TestCases)
                          .Where(course => course.Id == request.CourseId).ToList();

                courses.ForEach(course =>
                {
                    course.Chapters = course.Chapters.OrderBy(chapter => chapter.Part).ToList();
                });

                var result = courses.Select(course => new
                {
                    course.Id,
                    course.Name,
                    course.Description,
                    course.Picture,
                    course.Tag,
                    Chapters = course.Chapters.Select(chapter => new
                    {
                        chapter.Id,
                        chapter.Name,
                        chapter.CourseId,
                        chapter.Part,
                        chapter.IsNew,
                        CodeQuestions = chapter.CodeQuestions.Select(codeQuestion => new
                        {
                            codeQuestion.Id,
                            codeQuestion.Description,
                            TestCases = codeQuestion.TestCases.Select(testCase => new
                            {
                                testCase.Id,
                                testCase.InputTypeInt,
                                testCase.InputTypeString,
                                testCase.ExpectedResultInt,
                                testCase.CodeQuestionId,
                                testCase.ExpectedResultString,
                                testCase.InputTypeBoolean,
                                testCase.ExpectedResultBoolean,
                                testCase.InputTypeArrayInt,
                                testCase.InputTypeArrayString
                            }).ToList()
                        }).ToList(),
                        Lessons = chapter.Lessons.Select(lesson => new
                        {
                            lesson.Id,
                            lesson.Title,
                            lesson.VideoUrl,
                            lesson.ChapterId,
                            lesson.Description,
                            lesson.Duration,
                            lesson.IsCompleted,
                            Questions = lesson.Questions.Select(question => new
                            {
                                question.Id,
                                question.VideoId,
                                question.ContentQuestion,
                                question.AnswerA,
                                question.AnswerB,
                                question.AnswerC,
                                question.AnswerD,
                                question.CorrectAnswer,
                                question.Time
                            }).ToList()
                        }).ToList()
                    }).ToList()
                }).ToList();

                return new OkObjectResult(result);
            }
        }
    }
}