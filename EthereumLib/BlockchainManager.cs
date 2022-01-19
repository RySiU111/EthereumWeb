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

        public async Task<List<BlockWithTransactions>> GetBlocks(int latestBlocksRange, CancellationToken cancellationToken) {
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

            System.Console.WriteLine(blocks.Count);

            foreach (var b in blocks)
            {
                System.Console.WriteLine(b.Number);
            }

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
            var _contractAddress = "0x2a212f50a2a020010ea88cc33fc4c333e6a5c5c4";
            var service = new ModelFactoryService(_web3, _contractAddress);

            var deployModelContractFunction = new DeployModelContractFunction();
            var transactionReceipt = await service.DeployModelContractRequestAndWaitForReceiptAsync();

            return transactionReceipt;
        }

        public async Task<List<string>> GetDeployedModelContracts()
        {
            var _contractAddress = "0x2a212f50a2a020010ea88cc33fc4c333e6a5c5c4";
            var service = new ModelFactoryService(_web3, _contractAddress);

            return await service.GetDeployedContractsQueryAsync();
        }

        public async Task<TransactionReceipt> ModelContractAddModel(string modelContractAddress)
        {
            var _contractAddress = "0x2a212f50a2a020010ea88cc33fc4c333e6a5c5c4";
            var service = new ModelFactoryService(_web3, _contractAddress);

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
            var _contractAddress = "0x2a212f50a2a020010ea88cc33fc4c333e6a5c5c4";
            var service = new ModelFactoryService(_web3, _contractAddress);

            var ModelContractGetModelsFunction = new ModelContractGetModelsFunction()
            {
                Adr = modelContractAddress
            };

            var models = await service.ModelContractGetModelsQueryAsync(ModelContractGetModelsFunction);

            return models.ReturnValue1;
        }

        public async Task<Model> ModelContractIdToModel(string modelContractAddress, string modelId)
        {
            var _contractAddress = "0x2a212f50a2a020010ea88cc33fc4c333e6a5c5c4";
            var service = new ModelFactoryService(_web3, _contractAddress);

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
