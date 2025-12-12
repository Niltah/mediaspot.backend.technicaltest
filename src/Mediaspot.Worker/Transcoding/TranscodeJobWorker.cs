using Mediaspot.Application.Common;
using Mediaspot.Domain.Assets;

namespace Mediaspot.Worker.Transcoding;

public sealed class TranscodeJobWorker(ITranscodeJobRepository transcodeJobRepository, IAssetRepository assetRepository, IUnitOfWork uow)
{
    public async Task ProcessJobAsync(Guid transcodeJobId, CancellationToken ct)
    {
        var job = await transcodeJobRepository.GetAsync(transcodeJobId, ct)
            ?? throw new KeyNotFoundException("Transcode Job not found");

        var asset = await assetRepository.GetAsync(job.AssetId, ct)
            ?? throw new KeyNotFoundException("Asset not found");

        try
        {
            if (asset as VideoAsset != null)
            {
                await DoDummyWork_VideoAsset((VideoAsset)asset);
            }
            else if (asset as AudioAsset != null)
            {
                await DoDummyWork_AudioAsset((AudioAsset)asset);
            }
            else
            {
                throw new NotImplementedException("Asset type processing not implemented");
            }

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

    private static async Task DoDummyWork_VideoAsset(VideoAsset asset)
    {
        await DoDummyWork();
    }
    private static async Task DoDummyWork_AudioAsset(AudioAsset asset)
    {
        await DoDummyWork();
    }
    private static async Task DoDummyWork()
    {
        // Wait 3 seconds to simulate processing job
        await Task.Delay(TimeSpan.FromSeconds(3));
    }
}
