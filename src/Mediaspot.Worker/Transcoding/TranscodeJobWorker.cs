using Mediaspot.Application.Common;

namespace Mediaspot.Worker.Transcoding;

public sealed class TranscodeJobWorker(ITranscodeJobRepository repo, IUnitOfWork uow)
{
    public async Task ProcessJobAsync(Guid transcodeJobId, CancellationToken ct)
    {
        var job = await repo.GetAsync(transcodeJobId, ct)
            ?? throw new KeyNotFoundException("Transcode Job not found"); ;

        try
        {
            await DoDummyWork();

            job.MarkSucceeded();
        }
        catch
        {
            job.MarkFailed();
        }
        finally
        {
            await uow.SaveChangesAsync(ct);
        }
    }

    private static async Task DoDummyWork()
    {
        // Wait 3 seconds to simulate processing job
        await Task.Delay(TimeSpan.FromSeconds(3));
    }
}
