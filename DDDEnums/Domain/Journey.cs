using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DDDEnums.Support;
using static DDDEnums.Domain.StepStatus;

namespace DDDEnums.Domain;

public class Journey
{
    public int Id{ get; }
    public int UserId { get; }
    private readonly List<Step> _steps;

    public Journey(int id, int userId, IList<Step> steps)
    {
        UserId = userId;
        Id = id;
        ValidateSteps(steps);
        _steps = steps.ToList();
    }

    private static void ValidateSteps(IList<Step> steps)
    {
        if (steps.Count(s => s.IsActive) > 1)
            throw new Exception("Cannot have mutliple active steps");
    }

    public ReadOnlyCollection<Step> Steps => _steps.ToList().AsReadOnly();

    public void UpdateStepStatus(int stepId, StepStatus newStatus)
    {
        var stepToUpdate = _steps.LastOrDefault();
        
        if (stepToUpdate?.Id != stepId)
            throw new Exception("Can only update latest step");
        
        var stepWithNewStatus = stepToUpdate.WithNewStatus(newStatus);

        _steps[^1] = stepWithNewStatus;
        
        newStatus
            .When(Completed).Then(() => EventDispatcher.Dispatch("StepCompleted"))
            .When(Failed).Then(() => EventDispatcher.Dispatch("StepFailed"));
    }
    
}
public class NoCurrentJourney : Journey
{
    public NoCurrentJourney() : base(0,0, new List<Step>()) {}
}

public class NoCurrentJourneyException : Exception
{
}