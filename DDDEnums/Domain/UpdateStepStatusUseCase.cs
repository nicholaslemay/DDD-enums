namespace DDDEnums.Domain;

public class UpdateStepStatusUseCase
{
    private readonly JourneyRepository _journeyRepository;

    public UpdateStepStatusUseCase(JourneyRepository journeyRepository)
    {
        _journeyRepository = journeyRepository;
    }

    public void UpdateStepStatus(int userId, int stepId, StepStatus newStatus)
    {
        var journey = _journeyRepository.CurrentJourneyOfUser(userId);

        if (journey is NoCurrentJourney)
            throw new NoCurrentJourneyException();

        journey.UpdateStepStatus(stepId, newStatus);

        _journeyRepository.Save(journey);
    }

}