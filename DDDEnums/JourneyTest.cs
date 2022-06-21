using System;
using System.Collections.Generic;
using System.Linq;
using DDDEnums.Domain;
using DDDEnums.Support;
using FluentAssertions;
using Xunit;
using static DDDEnums.Domain.StepStatus;

namespace DDDEnums;

public class JourneyTest
{
    private const int AnyId = 42;
    
    [Fact]
    public void JourneyCannotBeCreatedWithMoreThanOneActiveStep()
    {
        CreatingAJourneyWithTheseSteps(new List<Step>()).Should().NotThrow();
        
        CreatingAJourneyWithTheseSteps(new List<Step>{new(AnyId, Completed),new(AnyId, Completed)}).Should().NotThrow();
        CreatingAJourneyWithTheseSteps(new List<Step>{new(AnyId, Completed),new(AnyId, Started)}).Should().NotThrow();
        
        CreatingAJourneyWithTheseSteps(new List<Step>{new(AnyId, Started),new(AnyId, Started)}).Should().Throw<Exception>();
        CreatingAJourneyWithTheseSteps(new List<Step>{new(AnyId, Started),new(AnyId, Created)}).Should().Throw<Exception>();
    }
    
    [Fact]
    public void ItsStepsAreReadonly()
    {
        var myJourney = new Journey(AnyId, AnyId, new List<Step>{new(AnyId, Created)});
        myJourney.Steps[0].WithNewStatus(Started);

        myJourney.Steps[0].Status.Should().Be(Created);
    }
    
    [Fact]
    public void CanOnlyUpdateLastStep()
    {
        var myJourney = new Journey(AnyId, AnyId, new List<Step>{new(42, Completed),new(55, Created)});
        Assert.Throws<Exception>(()=> myJourney.UpdateStepStatus(42, Completed));
    }
    
    [Fact]
    public void UpdatesLastStepToDesiredStatus()
    {
        var myJourney = new Journey(AnyId, AnyId, new List<Step>{new(42, Completed),new(55, Started)});
        
        myJourney.UpdateStepStatus(55, Failed);

        myJourney.Steps.Last().Status.Should().Be(Failed);
    }   
    
    [Fact]
    public void EmitsEventOnFailed()
    {
        EventDispatcher.ReceivedEvents.Clear();
        var myJourney = new Journey(AnyId, AnyId, new List<Step>{new(42, Completed),new(55, Started)});
        
        myJourney.UpdateStepStatus(55, Failed);

        EventDispatcher.ReceivedEvents.Last().Should().Be("StepFailed");
    }
    
    [Fact]
    public void EmitsEventOnStepCompletion()
    {
        EventDispatcher.ReceivedEvents.Clear();
        var myJourney = new Journey(AnyId, AnyId, new List<Step>{new(42, Completed),new(55, Started)});
        
        myJourney.UpdateStepStatus(55, Completed);

        EventDispatcher.ReceivedEvents.Last().Should().Be("StepCompleted");
    }

    private static Action CreatingAJourneyWithTheseSteps(List<Step> steps) => () =>  new Journey(AnyId, AnyId, steps);
}