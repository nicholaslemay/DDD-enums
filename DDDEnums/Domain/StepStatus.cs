using System;
using Ardalis.SmartEnum;
using static DDDEnums.Domain.StepStatus;

namespace DDDEnums.Domain;

public class Step
{
    public Step(int id, StepStatus status)
    {
        Id = id;
        Status = status;
    }

    public int Id { get; }
    public StepStatus Status { get; }
    public bool IsDone => Status.IsDone;
    public bool IsActive => !Status.IsDone;

    public Step WithNewStatus(StepStatus newStatus)
    {
        if (IsDone)
            throw new Exception();
        if (Status == Created && newStatus != Started)
            throw new Exception();
        if (Status == Started && (newStatus != Completed && newStatus != Failed))
            throw new Exception();
        
        return new Step(Id, newStatus);
    }
}

public class StepStatus : SmartEnum<StepStatus, int>
{
    public static readonly StepStatus Created = new("Created", 1, false);
    public static readonly StepStatus Started = new("StartedAsDtoText", 2, false);
    public static readonly StepStatus Completed = new("Completed", 3, true);
    public static readonly StepStatus Failed = new("Failed", 4, true);

    private StepStatus(string name, int value, bool isDone) : base(name, value) => IsDone = isDone;

    public bool IsDone { get; }
}