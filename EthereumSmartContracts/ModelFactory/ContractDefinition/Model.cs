using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace NethereumCodeGen.Contracts.ModelFactory.ContractDefinition
{
    public partial class Model : ModelBase { }

    public class ModelBase 
    {
        [Parameter("string", "id", 1)]
        public virtual string Id { get; set; }
        [Parameter("uint256", "timestamp", 2)]
        public virtual BigInteger Timestamp { get; set; }
        [Parameter("address", "owner", 3)]
        public virtual string Owner { get; set; }
    }
}
