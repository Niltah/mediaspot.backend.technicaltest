using Mediaspot.Application.Common;
using Mediaspot.Domain.Transcoding.Events;
using MediatR;

namespace Mediaspot.Application.Events;

public sealed class TranscodeJobCompletedHandler(ITranscodeJobRepository repo, IUnitOfWork uow)
    : INotificationHandler<TranscodeJobCompleted>
{
    public async Task Handle(TranscodeJobCompleted @event, CancellationToken ct)
    {
        if (@event.Success)
        {
            // Todo: Handle transcode job success
        }
        else
        {
            // Todo: Handle transcode job failure
        }
    }
}