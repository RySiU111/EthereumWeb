// SPDX-License-Identifier: MIT

pragma solidity >=0.4.0 <0.8.12;

import "./ModelContract.sol";

contract ModelFactory {

    ModelContract[] private contracts;
    mapping(address => ModelContract) public addressToModelContract;

    function deployModelContract() public {
        ModelContract modelContract = new ModelContract();

        contracts.push(modelContract);
        addressToModelContract[address(modelContract)] = modelContract;
    }

    function getDeployedContracts() public view returns(ModelContract[] memory) {
        return contracts;
    }

    function modelContractAddModel(address adr, string memory _id, uint256 _timestamp) public {

        ModelContract modelContract = ModelContract(adr);
        modelContract.addModel(_id, _timestamp);
    }

    function modelContractGetModels(address adr) public view returns(ModelContract.Model[] memory) {

        ModelContract modelContract = ModelContract(adr);
        return modelContract.getModels();
    }

    function modelContractIdToModel(address adr, string memory _id) public view returns(ModelContract.Model memory) {

        ModelContract modelContract = ModelContract(adr);
        (string memory id, uint256 timestamp, address owner) = (modelContract.idToModel(_id));
        return ModelContract.Model(id, timestamp, owner);
    }

}