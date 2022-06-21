using System;
using System.Linq;
using DDDEnums.Domain;
using FluentAssertions;
using Xunit;
using static DDDEnums.Domain.StepStatus;

namespace DDDEnums;

public class StepTest
{
    private const int AnyId = 1;

    [Fact]
    public void StepIsConsideredDone_BasedOnStepStatus()
    {
        StepStatus.List.Should().HaveCount(4);
        new Step(AnyId,Completed).IsDone.Should().BeTrue();
        new Step(AnyId,Failed).IsDone.Should().BeTrue();
        new Step(AnyId, Started).IsDone.Should().BeFalse();
        new Step(AnyId, Created).IsDone.Should().BeFalse();
    }
    
    [Fact]
    public void UpdatingStepStatus_ReturnACopy()
    {
        var createdStep =  new Step(AnyId, Created);
        var updatedStep = createdStep.WithNewStatus(Started);

        updatedStep.Status.Should().Be(Started);
        createdStep.Status.Should().Be(Created);
        createdStep.Id.Should().Be(updatedStep.Id);
    }
    
    [Fact]
    public void CreatedStepCanOnlyBeStarted()
    {
        var createdStep = new Step(AnyId, Created);
        createdStep.WithNewStatus(Started).Status.Should().Be(Started);

        foreach (var illegalNewStatus in StepStatus.List.Except(new[] { Started }))
            Assert.Throws<Exception>(() => { createdStep.WithNewStatus(illegalNewStatus); });
    }
    
    [Fact]
    public void CompletedStepCannotChangeStatus()
    {
        var createdStep = new Step(AnyId, Completed);

        foreach (var illegalNewStatus in StepStatus.List)
            Assert.Throws<Exception>(() => { createdStep.WithNewStatus(illegalNewStatus); });
    }
    
    [Fact]
    public void FailedStepCannotChangeStatus()
    {
        var createdStep = new Step(AnyId, Failed);

        foreach (var illegalNewStatus in StepStatus.List)
            Assert.Throws<Exception>(() => { createdStep.WithNewStatus(illegalNewStatus); });
    }
    
    [Fact]
    public void StartedStepCanFailOnSucceed()
    {
        var createdStep = new Step(AnyId, Started);
        createdStep.WithNewStatus(Completed).Status.Should().Be(Completed);
        createdStep.WithNewStatus(Failed).Status.Should().Be(Failed);

        foreach (var illegalNewStatus in StepStatus.List.Except(new[] { Completed, Failed }))
            Assert.Throws<Exception>(() => { createdStep.WithNewStatus(illegalNewStatus); });
    }
}