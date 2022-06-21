using System;
using System.Collections.Generic;
using System.Linq;
using DDDEnums.Domain;
using FluentAssertions;
using Xunit;
using static DDDEnums.Domain.StepStatus;

namespace DDDEnums;

public class UpdateStepStatusUseCaseTest
{
    private readonly JourneyRepository _journeyRepository;
    private readonly UpdateStepStatusUseCase _sut;
    private const int userId = 42;
    private const int StepId = 55;

    public UpdateStepStatusUseCaseTest()
    {
        _journeyRepository = new JourneyRepository();
        _sut = new UpdateStepStatusUseCase(_journeyRepository);
    }

    [Fact]
    public void When_UserHasNoJourney_ThrowsException()
    {
        Assert.Throws<NoCurrentJourneyException>(()=>{ _sut.UpdateStepStatus(userId, StepId, Started);});
    }
    
    [Fact]
    public void UpdatesAndSavesUserJourneyToDesiredStatus()
    {
        _journeyRepository.Save(new Journey(55, userId, new List<Step>{new (StepId, Created)}));
        
       _sut.UpdateStepStatus(userId, StepId, Started);

       _journeyRepository.CurrentJourneyOfUser(userId).Steps.Last().Status.Should().Be(Started);
    }
}