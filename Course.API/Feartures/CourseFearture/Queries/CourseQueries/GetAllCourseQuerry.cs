﻿
using AutoMapper;
using Contract.SeedWork;
using CourseService.API.Common.ModelDTO;
using CourseService.API.GrpcServices;
using CourseService.API.Models;

using MediatR;

using Microsoft.EntityFrameworkCore;


namespace CourseService.API.Feartures.CourseFearture.Queries.CourseQueries
{
    public class GetAllCourseQuerry : IRequest<PageList<CourseDTO>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string? CourseName { get; set; }
        public class GetAllCoursesHandler : IRequestHandler<GetAllCourseQuerry, PageList<CourseDTO>>
        {
            private readonly IMediator mediator;
            private readonly IMapper _mapper;
            private readonly CourseContext _context;
            private readonly GetUserInfoService _service;
            public GetAllCoursesHandler(IMediator _mediator, IMapper mapper, CourseContext context,GetUserInfoService service)
            {
                mediator = _mediator;
                _mapper = mapper;
                _context = context;
                _service = service;
            }
            public async Task<PageList<CourseDTO>> Handle(GetAllCourseQuerry request, CancellationToken cancellation)
            {
                IQueryable<Course> query = _context.Courses;

                if (!string.IsNullOrEmpty(request.CourseName))
                {
                    query = query.Where(c => c.Name.Contains(request.CourseName));
                }

                List<CourseDTO> courseDTOList = new List<CourseDTO>();
                foreach(var item in query)
                {
                    var userInfo = await _service.SendUserId(item.CreatedBy);
                    var dto = new CourseDTO
                    {
                        CreatedAt = item.CreatedAt,
                        Description = item.Description,
                        Id = item.Id,
                        Name = item.Name,
                        Picture = item.Picture,
                        Tag = item.Tag,
                        UserId = item.CreatedBy,
                        UserName = userInfo.Name
                    };
                    courseDTOList.Add(dto); 
                }

                var totalItems = await query.CountAsync();
                var result = new PageList<CourseDTO>(courseDTOList, totalItems, request.Page, request.PageSize);
                return result;
            }
        }
    }
}
