﻿using CourseService.API.Common.DTO;
using CourseService.API.GrpcServices;
using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseService.API.Feartures.WishListFearture.Querries
{
    public class GetWishListByUserIdQuerry : IRequest<IActionResult>
    {
        public int UserId {  get; set; }

        public class GetWishListByUserIdQuerryHandler : IRequestHandler<GetWishListByUserIdQuerry, IActionResult>
        {
            private readonly CourseContext _context;
            private readonly GetUserInfoService _service;

            public GetWishListByUserIdQuerryHandler(CourseContext context, GetUserInfoService service)
            {
                _context = context;
                _service=service;
            }
    
            public async Task<IActionResult> Handle(GetWishListByUserIdQuerry request, CancellationToken cancellationToken)
            {
                if (request.UserId == null)
                {
                    return new BadRequestObjectResult("not found");
                }
                var querry = (from w in _context.Wishlists
                              join c in _context.Courses on w.CourseId equals c.Id
                              select new 
                              {
                                  CourseId = w.CourseId,
                                  Name=c.Name,
                                  Id=w.Id,
                                  Description=c.Description,
                                  Picture=c.Description,
                                  CreatedAt=c.CreatedAt,    
                                  Tag=c.Tag,
                                  UserId=c.CreatedBy
                              }).ToList();
                List<WishListDTO> result = new List<WishListDTO>();
                foreach(var c in querry)
                {
                    var userInfo = await _service.SendUserId(c.UserId);
                    var dto = new WishListDTO()
                    {
                        Id= c.Id,   
                        Name=c.Name,
                        UserId = c.UserId,
                        CreatedAt=c.CreatedAt,
                        Tag=c.Tag,
                        Picture=userInfo.Picture,
                        UserName=userInfo.Name,
                        Description=c.Description, 
                        CourseId=c.CourseId
                    };
                    result.Add(dto);

                }

                return new OkObjectResult(result);
            }
        }
    }
}