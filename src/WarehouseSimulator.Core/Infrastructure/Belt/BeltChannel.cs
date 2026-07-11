using System.Threading.Channels;
using WarehouseSimulator.Core.Domain.Products;

namespace WarehouseSimulator.Core.Infrastructure.Belt;

public class BeltChannel
{
    private readonly Channel<Product> _channel;

    public BeltChannel()
    {
        _channel = Channel.CreateBounded<Product>(new BoundedChannelOptions(10)
        {
            FullMode = BoundedChannelFullMode.Wait
        });
    }

    public ChannelWriter<Product> Writer => _channel.Writer;
    public ChannelReader<Product> Reader => _channel.Reader;
    public int Count => _channel.Reader.Count;
}