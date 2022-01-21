using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;
using NethereumCodeGen.Contracts.ModelFactory.ContractDefinition;

namespace BlockchainAPI.Services
{
    public interface IBlockchainService
    {
        Task<BigInteger> GetBlockNumber();
        Task<List<BlockWithTransactions>> GetBlocks(int count);
        Task<TransactionReceipt> DeployModelFactoryContract();
        Task<TransactionReceipt> DeployModelContract();
        Task<List<string>> GetDeployedContracts();
        Task<TransactionReceipt> ModelContractAddModel(string modelContractAddress);
        Task<List<Model>> ModelContractGetModels(string modelContractAddress);
        Task<Model> ModelContractGetModelById(string modelContractAddress, string modelId);
    }
}