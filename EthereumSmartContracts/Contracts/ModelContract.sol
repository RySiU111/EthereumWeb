// SPDX-License-Identifier: MIT

pragma solidity >=0.4.0 <0.8.12;

contract ModelContract {

    struct Model {
        string id;
        uint256 timestamp;
        address owner;
    }

    Model[] internal models;
    mapping(string => Model) public idToModel;

    function addModel(string memory _id, uint256 _timestamp) public {

        Model memory modelToAdd = Model( _id, _timestamp, msg.sender);
        
        models.push( modelToAdd);
        idToModel[_id] = modelToAdd;
    }

    function getModels() public view returns(Model[] memory) {
        return models;
    }
}