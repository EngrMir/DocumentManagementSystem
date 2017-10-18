angular.module("SILDMSApp", [])
.controller("MultiScanController", function ($scope) {
    $scope.DocumentCategory = {
        availableOptions: [
          { id: "PersonalFile", name: "Personal File" },
          { id: "Policy", name: "Policy" },
          { id: "PurchaseRecipt", name: "Purchase Recipt" }
        ],
        selectedOption: { id: "PersonalFile", name: "Personal File" } //This sets the default value of the select in the ui
    };
})