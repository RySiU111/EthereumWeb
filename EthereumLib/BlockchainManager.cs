using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Nethereum.BlockchainProcessing.Processor;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using NethereumCodeGen.Contracts.ModelFactory;
using NethereumCodeGen.Contracts.ModelFactory.ContractDefinition;

namespace EthereumLib
{
    public class BlockchainManager
    {
        private readonly Web3 _web3;
        public string ContractAddress { get; set; }

        ///<summary>
        ///
        ///</summary>
        public BlockchainManager(string keyStoreEncryptedJson, string password)
        {
            var account = Nethereum.Web3.Accounts.Account.LoadFromKeyStore(keyStoreEncryptedJson, password, 444444444500);
            _web3 = new Web3(account);
        }

        public async Task<BigInteger> GetBlockNumber()
        {
            var blockNumber = await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            
            return blockNumber.Value;
        }

        public async Task<IEnumerable<string>> GetAccounts()
        {
            return await _web3.Personal.ListAccounts.SendRequestAsync();            
        }

        public async Task<List<BlockWithTransactions>> GetBlocks(int latestBlocksRange, CancellationToken cancellationToken = default) {
            var blocks = new List<BlockWithTransactions>();

            var processor = _web3.Processing.Blocks.CreateBlockProcessor(steps => {
                steps.BlockStep.AddSynchronousProcessorHandler(b => blocks.Add(b));
            });

            var toBlockNumber = await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            var fromBlockNumber = toBlockNumber.Value - latestBlocksRange + 1;

            await processor.ExecuteAsync(
                toBlockNumber.Value, 
                cancellationToken, 
                fromBlockNumber);

            return blocks;
        }

        public async Task<TransactionReceipt> DeployModelFactoryContract()
        {
            var modelFactoryDeployment = new ModelFactoryDeployment();
            var transactionReceipt = await ModelFactoryService.DeployContractAndWaitForReceiptAsync(_web3, modelFactoryDeployment);

            return transactionReceipt;
        }

        public async Task<TransactionReceipt> DeployModelContract()
        {
            var service = new ModelFactoryService(_web3, ContractAddress);

            var deployModelContractFunction = new DeployModelContractFunction();
            var transactionReceipt = await service.DeployModelContractRequestAndWaitForReceiptAsync();

            return transactionReceipt;
        }

        public async Task<List<string>> GetDeployedModelContracts()
        {
            var service = new ModelFactoryService(_web3, ContractAddress);

            return await service.GetDeployedContractsQueryAsync();
        }

        public async Task<TransactionReceipt> ModelContractAddModel(string modelContractAddress)
        {
            var service = new ModelFactoryService(_web3, ContractAddress);

            var modelContractAddModelFunction = new ModelContractAddModelFunction()
            {
                Adr = modelContractAddress,
                Id = Guid.NewGuid().ToString(),
                Timestamp = DateTime.Now.Ticks
            };

            var transactionReceipt = await service.ModelContractAddModelRequestAndWaitForReceiptAsync(modelContractAddModelFunction);

            return transactionReceipt;
        }

        public async Task<List<Model>> ModelContractGetModels(string modelContractAddress)
        {
            var service = new ModelFactoryService(_web3, ContractAddress);

            var ModelContractGetModelsFunction = new ModelContractGetModelsFunction()
            {
                Adr = modelContractAddress
            };

            var models = await service.ModelContractGetModelsQueryAsync(ModelContractGetModelsFunction);

            return models.ReturnValue1;
        }

        public async Task<Model> ModelContractIdToModel(string modelContractAddress, string modelId)
        {
            var service = new ModelFactoryService(_web3, ContractAddress);

            var modelContractIdToModelFunction = new ModelContractIdToModelFunction()
            {
                Adr = modelContractAddress,
                Id = modelId
            };

            var model = await service.ModelContractIdToModelQueryAsync(modelContractIdToModelFunction);

            return model.ReturnValue1;
        }
    }
}
