using System.Text.Json;
using System.Text.Json.Serialization;
using Ardalis.SmartEnum.SystemTextJson;
using DDDEnums.Domain;
using FluentAssertions;
using Xunit;
using static DDDEnums.Domain.StepStatus;

namespace DDDEnums;

public class StepStatusTests
{

    public class StepDto
    {
        [JsonConverter(typeof(SmartEnumNameConverter<StepStatus, int>))]
        public StepStatus StepStatus { get; set; }
    }

    [Fact]
    public void CanCompareStepStatusValues() => Completed.Should().BeGreaterThan(Created);

    [Fact]
    public void CanBeSerializedToJson()
    {
        const string expectedJson = @"{""StepStatus"":""StartedAsDtoText""}";
        var stepDto = new StepDto { StepStatus = Started };
        var json = JsonSerializer.Serialize(stepDto);

        json.Should().Be(expectedJson);
    }


}