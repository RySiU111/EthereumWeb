using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using BlockchainAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nethereum.RPC.Eth.DTOs;
using NethereumCodeGen.Contracts.ModelFactory.ContractDefinition;

namespace BlockchainAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlockchainController : ControllerBase
    {
        private readonly ILogger<BlockchainController> _logger;
        public readonly IBlockchainService _blockchainService;

        public BlockchainController(ILogger<BlockchainController> logger, IBlockchainService blockchainService)
        {
            _blockchainService = blockchainService;
            _logger = logger;
        }

        [HttpGet]
        [Route("blockNumber")]
        public async Task<int> GetBlockNumber()
        {
            return (int)await _blockchainService.GetBlockNumber();
        }

        [HttpGet]
        [Route("blocks/{count}")]
        public async Task<List<BlockWithTransactions>> GetBlockNumber(int count)
        {
            return await _blockchainService.GetBlocks(count);
        }

        [HttpPost]
        [Route("modelFactory/deploy")]
        public async Task<TransactionReceipt> DeployModelFactoryContract()
        {
            return await _blockchainService.DeployModelFactoryContract();
        }

        [HttpPost]
        [Route("modelContract/deploy")]
        public async Task<TransactionReceipt> DeployModelContract()
        {
            return await _blockchainService.DeployModelContract();
        }

        [HttpGet]
        [Route("modelContracts")]
        public async Task<List<string>> GetDeployedModelContracts()
        {
            return await _blockchainService.GetDeployedContracts();
        }

        [HttpPost]
        [Route("modelContract/{contractId}/model")]
        public async Task<TransactionReceipt> ModelContractAddModel(string contractId)
        {
            return await _blockchainService.ModelContractAddModel(contractId);
        }

        [HttpGet]
        [Route("modelContract/{contractId}/model/{modelId}")]
        public async Task<Model> ModelContractGetModels(string contractId, string modelId)
        {
            return await _blockchainService.ModelContractGetModelById(contractId, modelId);
        }

        [HttpGet]
        [Route("modelContract/{contractId}/models")]
        public async Task<List<Model>> ModelContractGetModels(string contractId)
        {
            return await _blockchainService.ModelContractGetModels(contractId);
        }
    }
}
