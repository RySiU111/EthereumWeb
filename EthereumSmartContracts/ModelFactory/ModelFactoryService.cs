using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using NethereumCodeGen.Contracts.ModelFactory.ContractDefinition;

namespace NethereumCodeGen.Contracts.ModelFactory
{
    public partial class ModelFactoryService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, ModelFactoryDeployment modelFactoryDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<ModelFactoryDeployment>().SendRequestAndWaitForReceiptAsync(modelFactoryDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, ModelFactoryDeployment modelFactoryDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<ModelFactoryDeployment>().SendRequestAsync(modelFactoryDeployment);
        }

        public static async Task<ModelFactoryService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, ModelFactoryDeployment modelFactoryDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, modelFactoryDeployment, cancellationTokenSource);
            return new ModelFactoryService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public ModelFactoryService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> AddressToModelContractQueryAsync(AddressToModelContractFunction addressToModelContractFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<AddressToModelContractFunction, string>(addressToModelContractFunction, blockParameter);
        }

        
        public Task<string> AddressToModelContractQueryAsync(string returnValue1, BlockParameter blockParameter = null)
        {
            var addressToModelContractFunction = new AddressToModelContractFunction();
                addressToModelContractFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<AddressToModelContractFunction, string>(addressToModelContractFunction, blockParameter);
        }

        public Task<string> DeployModelContractRequestAsync(DeployModelContractFunction deployModelContractFunction)
        {
             return ContractHandler.SendRequestAsync(deployModelContractFunction);
        }

        public Task<string> DeployModelContractRequestAsync()
        {
             return ContractHandler.SendRequestAsync<DeployModelContractFunction>();
        }

        public Task<TransactionReceipt> DeployModelContractRequestAndWaitForReceiptAsync(DeployModelContractFunction deployModelContractFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(deployModelContractFunction, cancellationToken);
        }

        public Task<TransactionReceipt> DeployModelContractRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<DeployModelContractFunction>(null, cancellationToken);
        }

        public Task<List<string>> GetDeployedContractsQueryAsync(GetDeployedContractsFunction getDeployedContractsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetDeployedContractsFunction, List<string>>(getDeployedContractsFunction, blockParameter);
        }

        
        public Task<List<string>> GetDeployedContractsQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetDeployedContractsFunction, List<string>>(null, blockParameter);
        }

        public Task<string> ModelContractAddModelRequestAsync(ModelContractAddModelFunction modelContractAddModelFunction)
        {
             return ContractHandler.SendRequestAsync(modelContractAddModelFunction);
        }

        public Task<TransactionReceipt> ModelContractAddModelRequestAndWaitForReceiptAsync(ModelContractAddModelFunction modelContractAddModelFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(modelContractAddModelFunction, cancellationToken);
        }

        public Task<string> ModelContractAddModelRequestAsync(string adr, string id, BigInteger timestamp)
        {
            var modelContractAddModelFunction = new ModelContractAddModelFunction();
                modelContractAddModelFunction.Adr = adr;
                modelContractAddModelFunction.Id = id;
                modelContractAddModelFunction.Timestamp = timestamp;
            
             return ContractHandler.SendRequestAsync(modelContractAddModelFunction);
        }

        public Task<TransactionReceipt> ModelContractAddModelRequestAndWaitForReceiptAsync(string adr, string id, BigInteger timestamp, CancellationTokenSource cancellationToken = null)
        {
            var modelContractAddModelFunction = new ModelContractAddModelFunction();
                modelContractAddModelFunction.Adr = adr;
                modelContractAddModelFunction.Id = id;
                modelContractAddModelFunction.Timestamp = timestamp;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(modelContractAddModelFunction, cancellationToken);
        }

        public Task<ModelContractGetModelsOutputDTO> ModelContractGetModelsQueryAsync(ModelContractGetModelsFunction modelContractGetModelsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<ModelContractGetModelsFunction, ModelContractGetModelsOutputDTO>(modelContractGetModelsFunction, blockParameter);
        }

        public Task<ModelContractGetModelsOutputDTO> ModelContractGetModelsQueryAsync(string adr, BlockParameter blockParameter = null)
        {
            var modelContractGetModelsFunction = new ModelContractGetModelsFunction();
                modelContractGetModelsFunction.Adr = adr;
            
            return ContractHandler.QueryDeserializingToObjectAsync<ModelContractGetModelsFunction, ModelContractGetModelsOutputDTO>(modelContractGetModelsFunction, blockParameter);
        }

        public Task<ModelContractIdToModelOutputDTO> ModelContractIdToModelQueryAsync(ModelContractIdToModelFunction modelContractIdToModelFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<ModelContractIdToModelFunction, ModelContractIdToModelOutputDTO>(modelContractIdToModelFunction, blockParameter);
        }

        public Task<ModelContractIdToModelOutputDTO> ModelContractIdToModelQueryAsync(string adr, string id, BlockParameter blockParameter = null)
        {
            var modelContractIdToModelFunction = new ModelContractIdToModelFunction();
                modelContractIdToModelFunction.Adr = adr;
                modelContractIdToModelFunction.Id = id;
            
            return ContractHandler.QueryDeserializingToObjectAsync<ModelContractIdToModelFunction, ModelContractIdToModelOutputDTO>(modelContractIdToModelFunction, blockParameter);
        }
    }
}
