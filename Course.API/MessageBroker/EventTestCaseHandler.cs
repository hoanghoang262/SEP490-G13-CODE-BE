﻿using AutoMapper;

using CourseService.API.Feartures.CourseFearture.Command.SyncCourse;
using EventBus.Message.Event;
using MassTransit;
using MediatR;

namespace CourseService.API.MessageBroker
{
    public class EventTestCaseHandler : IConsumer<TestCaseEvent>
    {
        private readonly IMediator mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<EventHandler> logger;
        public EventTestCaseHandler(ILogger<EventHandler> _logger, IMediator _mediator, IMapper mapper)
        {

            mediator = _mediator;
            _mapper = mapper;
            logger = _logger;
        }
        public async Task Consume(ConsumeContext<TestCaseEvent> context)
        {
            var command = _mapper.Map<SyncTestCaseCommand>(context.Message);
            var result = await mediator.Send(command);
        }
    }
}