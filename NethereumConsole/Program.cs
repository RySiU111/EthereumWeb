using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EthereumLib;
using Nethereum.BlockchainProcessing.Processor;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts.Managed;
using NethereumCodeGen.Contracts.ModelFactory;

namespace NethereumConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var keyStoreEncryptedJson =
                @"{""address"":""12890d2cce102216644c59dae5baed380d84830c"",""Crypto"":{""cipher"":""aes-128-ctr"",""ciphertext"":""a1c02524d5b9de2b05a265fbec8a3e36d8dfc4920ceef453ebe95b8eb8c70d95"",""cipherparams"":{""iv"":""aefddc737e457e8e96ac3d8987dcc444""},""kdf"":""scrypt"",""kdfparams"":{""dklen"":32,""n"":262144,""p"":1,""r"":8,""salt"":""f895a52dfc1520030c739d606fe0a96f75f09892b4c501b5add2cfdc80ba7a2e""},""mac"":""fecb89f8e07b1a222d95d71373084f1d69fbbb27f31ef7a92626d64973dbec2e""},""id"":""997cce35-189c-4b05-8083-81d4b3dd3f35"",""version"":3}"; 
            var blockchainManager = new BlockchainManager(keyStoreEncryptedJson, "password");
            System.Console.WriteLine(await blockchainManager.GetBlockNumber());
            

            foreach(var a in await blockchainManager.GetAccounts())
            {
                System.Console.WriteLine(a);
            }

            var cancellationToken = new CancellationToken();
            await blockchainManager.GetBlocks(10, cancellationToken);

            // var transactionReceipt = await blockchainManager.DeployModelFactoryContract();
            // System.Console.WriteLine(JsonSerializer.Serialize(transactionReceipt));
            
            // var transactionReceipt1 = await blockchainManager.DeployModelContract();
            // System.Console.WriteLine(JsonSerializer.Serialize(transactionReceipt1));

            var deployedContracts = await blockchainManager.GetDeployedModelContracts();
            System.Console.WriteLine(JsonSerializer.Serialize(deployedContracts));

            // var transactionReceipt2 = await blockchainManager.ModelContractAddModel(deployedContracts[0]);
            // System.Console.WriteLine(JsonSerializer.Serialize(transactionReceipt2));


            var models = await blockchainManager.ModelContractGetModels(deployedContracts[0]);
            System.Console.WriteLine(JsonSerializer.Serialize(models));

            var model = await blockchainManager.ModelContractIdToModel(deployedContracts[0], models[0].Id);
            System.Console.WriteLine(JsonSerializer.Serialize(model));

            // var transactions = new List<TransactionReceiptVO>();
            // var filterLogs = new List<FilterLogVO>();

            // var web3 = new Web3("https://rinkeby.infura.io/v3/ddd5ed15e8d443e295b696c0d07c8b02");

            // const string ContractAddress = "0x5534c67e69321278f5258f5bebd5a2078093ec19";

            // //create our processor
            // var processor = web3.Processing.Blocks.CreateBlockProcessor(steps =>
            // {
            //     //for performance we add criteria before we have the receipt to prevent unecessary data retrieval
            //     //we only want to retrieve receipts if the tx was sent to the contract
            //     steps.TransactionStep.SetMatchCriteria(t => t.Transaction.IsTo(ContractAddress));
            //     steps.TransactionReceiptStep.AddSynchronousProcessorHandler(tx => transactions.Add(tx));
            //     steps.FilterLogStep.AddSynchronousProcessorHandler(l => filterLogs.Add(l));
            // });

            // //if we need to stop the processor mid execution - call cancel on the token
            // var cancellationToken = new CancellationToken();
            // //crawl the blocks
            // await processor.ExecuteAsync(
            //     toBlockNumber: new BigInteger(2830145),
            //     cancellationToken: cancellationToken,
            //     startAtBlockNumberIfNotProcessed: new BigInteger(2830144));

            // Console.WriteLine($"Transactions. Expected: 2, Actual: {transactions.Count}");
            // Console.WriteLine($"Logs. Expected: 8, Actual: {filterLogs.Count}");

            // foreach (var l in filterLogs)
            // {
            //     var hex = new Nethereum.Hex.HexConvertors.HexBigIntegerBigEndianConvertor();
            //     System.Console.WriteLine(hex.ConvertFromHex(l.Log.Data));
            // }

            // foreach (var t in transactions)
            // {
            //     System.Console.WriteLine(t.Transaction.Value);
            // }
        }
    }
}
