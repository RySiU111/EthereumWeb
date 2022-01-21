using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using EthereumLib;
using Microsoft.AspNetCore.Hosting;
using Nethereum.RPC.Eth.DTOs;
using NethereumCodeGen.Contracts.ModelFactory.ContractDefinition;

namespace BlockchainAPI.Services
{
    public class BlockchainService : IBlockchainService
    {
        private readonly BlockchainManager _blockchainManager;
        public IWebHostEnvironment _env;
        
        public BlockchainService(IWebHostEnvironment env)
        {
            _env = env;

            var keyStoreEncryptedJson =
                @"{""address"":""12890d2cce102216644c59dae5baed380d84830c"",""Crypto"":{""cipher"":""aes-128-ctr"",""ciphertext"":""a1c02524d5b9de2b05a265fbec8a3e36d8dfc4920ceef453ebe95b8eb8c70d95"",""cipherparams"":{""iv"":""aefddc737e457e8e96ac3d8987dcc444""},""kdf"":""scrypt"",""kdfparams"":{""dklen"":32,""n"":262144,""p"":1,""r"":8,""salt"":""f895a52dfc1520030c739d606fe0a96f75f09892b4c501b5add2cfdc80ba7a2e""},""mac"":""fecb89f8e07b1a222d95d71373084f1d69fbbb27f31ef7a92626d64973dbec2e""},""id"":""997cce35-189c-4b05-8083-81d4b3dd3f35"",""version"":3}";
            _blockchainManager = new BlockchainManager(keyStoreEncryptedJson, "password");

            var contractAddress = File.ReadAllText($"{_env.WebRootPath}/ModelFactoryContractAddress.txt");

            if(!string.IsNullOrEmpty(contractAddress))
                _blockchainManager.ContractAddress = contractAddress;
        }

        public async Task<BigInteger> GetBlockNumber()
        {
            return await _blockchainManager.GetBlockNumber();
        }

        public async Task<List<BlockWithTransactions>> GetBlocks(int count)
        {
            return await _blockchainManager.GetBlocks(count);
        }

        public async Task<TransactionReceipt> DeployModelFactoryContract()
        {
            var transactionReceipt = await _blockchainManager.DeployModelFactoryContract();

            _blockchainManager.ContractAddress = transactionReceipt.ContractAddress;
            File.WriteAllText($"{_env.WebRootPath}/ModelFactoryContractAddress.txt", transactionReceipt.ContractAddress);

            return transactionReceipt;
        }

        public async Task<TransactionReceipt> DeployModelContract()
        {
            if(string.IsNullOrEmpty(_blockchainManager.ContractAddress))
                return null;

            return await _blockchainManager.DeployModelContract();
        }

        public async Task<List<string>> GetDeployedContracts()
        {
            if(string.IsNullOrEmpty(_blockchainManager.ContractAddress))
                return null;

            return await _blockchainManager.GetDeployedModelContracts();
        }

        public async Task<TransactionReceipt> ModelContractAddModel(string modelContractAddress)
        {
            if(string.IsNullOrEmpty(_blockchainManager.ContractAddress))
                return null;

            return await _blockchainManager.ModelContractAddModel(modelContractAddress);
        }

        public async Task<List<Model>> ModelContractGetModels(string modelContractAddress)
        {
            if(string.IsNullOrEmpty(_blockchainManager.ContractAddress))
                return null;

            return await _blockchainManager.ModelContractGetModels(modelContractAddress);
        }

        public async Task<Model> ModelContractGetModelById(string modelContractAddress, string modelId)
        {
            if(string.IsNullOrEmpty(_blockchainManager.ContractAddress))
                return null;

            return await _blockchainManager.ModelContractIdToModel(modelContractAddress, modelId);
        }
    }
}