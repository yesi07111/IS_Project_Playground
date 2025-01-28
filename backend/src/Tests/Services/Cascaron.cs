using AutoFixture;
using Moq;

namespace Playground.Tests.Services;

public class Name
{
    private readonly Fixture _fixture;

    public Name()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

}
