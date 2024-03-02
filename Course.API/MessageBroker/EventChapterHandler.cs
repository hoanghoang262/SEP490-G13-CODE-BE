﻿using AutoMapper;
using CourseService.API.Feartures.CourseFearture.Command.SyncCourse;
using EventBus.Message.Event;
using MassTransit;
using MediatR;


namespace CourseService.API.MessageBroker
{
    public class EventChapterHandler : IConsumer<ChapterEvent>
    {

        private readonly IMediator mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<EventHandler> logger;
        public EventChapterHandler(ILogger<EventHandler> _logger, IMediator _mediator, IMapper mapper)
        {

            mediator = _mediator;
            _mapper = mapper;
            logger = _logger;
        }

        public async Task Consume(ConsumeContext<ChapterEvent> context)
        {
            var command = _mapper.Map<SyncChapterCommand>(context.Message);
            var result = await mediator.Send(command);

        }
    }
}
