using System.Collections.Generic;
using Newtonsoft.Json;

namespace DDDEnums.Domain;

public class JourneyRepository
{
    private readonly Dictionary<int, Journey> _journeyByUserId = new();
    public Journey CurrentJourneyOfUser(int userId)
    {
        if (_journeyByUserId.ContainsKey(userId))
            return ACopyOf(_journeyByUserId[userId]);

        return new NoCurrentJourney();
    }

    private static Journey? ACopyOf(Journey journey)
    {
        return new Journey(journey.Id, journey.UserId, journey.Steps);
    }

    public void Save(Journey journey) => _journeyByUserId[journey.UserId] = journey;
}