using System.Numerics;
using System.Threading.Tasks;

namespace BlockchainWEB.Services
{
    public interface IBlockchainService
    {
        Task<BigInteger> GetBlockNumber();
    }
}