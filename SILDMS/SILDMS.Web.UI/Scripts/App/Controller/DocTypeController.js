//// Controller for Document Type page
//app.controller("docTypeCtrl", ["$scope", "$http", function ($scope, $http) {
//    // Model for Document Type
//    $scope.docType = {
//        DocTypeID: "", OwnerLevel: { OwnerLevelID: "", LevelName: "" }, Owner: { OwnerID: "", OwnerName: "" },
//        DocCategory: { DocCategoryID: "", DocCategoryName: "" }, DocTypeSL: "", UDDocTypeCode: "", DocTypeName: "", DocPreservationPolicy: "",
//        DocPhysicalLocation: "", Status: ""
//    };
//    // Model for the data to saved/updated
//    $scope.saveModel = {};
//    $scope.itemsByPage = 10;
//    $scope.loading = true;
//    // Get all owner levels
//    $http.get("/DocScanningModule/DocType/GetOwnerLevel?id=")
//        .success(function (response) { $scope.ownerLevels = response.result; $scope.loading = false; })
//        .error(function () { });
//    // Get all owners for the selected owner level on owner level dropdown change
//    $scope.changeOwnerLevel = function () {
//        $http.get("/DocScanningModule/DocType/GetOwners?id=" + $scope.docType.OwnerLevel.OwnerLevelID)
//            .success(function (response) { $scope.owners = response.result; $scope.docTypes = []; $scope.docCategories = []; $scope.loading = true; })
//            .error(function () { });
//    }
//    // Get all document categories for the selected owner on owner drop down change
//    $scope.changeOwner = function () {
//        $http.get("/DocScanningModule/DocType/GetDocCategoryForOwner?id=" + $scope.docType.Owner.OwnerID)
//            .success(function (response) { $scope.docCategories = response.result; $scope.docTypes = []; })
//            .error(function () { });
//    };
//    // Get all document types for the selected document category on documnet category drop down change
//    $scope.changeDocCategory = function () {
//        $http.get("/DocScanningModule/DocType/GetDocTypeForCategory?id=" + $scope.docType.DocCategory.DocCategoryID)
//            .success(function (response) { $scope.docTypes = response.result; $scope.loading = false; })
//            .error(function () { });
//    };
//    // Add new Document Type
//    $scope.toggleAdd = function (row) {
//        $scope.docType.DocTypeID = ""; $scope.docType.DocTypeSL = ""; $scope.docType.UDDocTypeCode = ""; $scope.docType.DocTypeName = "";
//        $scope.docType.DocPreservationPolicy = ""; $scope.docType.DocPhysicalLocation = ""; $scope.docType.Status = ""; $('#addModal').modal('show');
//    };
//    // Edit Document Type
//    $scope.toggleEdit = function (row) {
//        $scope.docType.DocTypeID = row.DocTypeID; $scope.docType.DocTypeSL = row.DocTypeSL; $scope.docType.UDDocTypeCode = row.UDDocTypeCode;
//        $scope.docType.DocTypeName = row.DocTypeName; $scope.docType.DocPreservationPolicy = row.DocPreservationPolicy; $scope.docType.DocPhysicalLocation = row.DocPhysicalLocation;
//        $scope.docType.Status = (row.Status).toString(); $("#addModal").modal("show");
//    };
//    // Convert current model to save model
//    $scope.convertModel = function () {
//        $scope.saveModel.DocTypeID = $scope.docType.DocTypeID; $scope.saveModel.OwnerID = $scope.docType.Owner.OwnerID;
//        $scope.saveModel.DocCategoryID = $scope.docType.DocCategory.DocCategoryID; $scope.saveModel.DocTypeSL = $scope.docType.DocTypeSL;
//        $scope.saveModel.UDDocTypeCode = $scope.docType.UDDocTypeCode; $scope.saveModel.DocTypeName = $scope.docType.DocTypeName;
//        $scope.saveModel.DocPreservationPolicy = $scope.docType.DocPreservationPolicy; $scope.saveModel.DocPhysicalLocation = $scope.docType.DocPhysicalLocation;
//        $scope.saveModel.Status = $scope.docType.Status.toString();
//    };
//    // Save/Update Document Type
//    $scope.Save = function () {
//        $scope.loading = true; $scope.convertModel();
//        if ($scope.docType.DocTypeID) {
//            // Update
//            $http.post("/DocScanningModule/DocType/EditDocType/", JSON.stringify(convArrToObj($scope.saveModel))).success(function (data) {
//                if (data._respStatus.Status === "1") {
//                    $scope.loading = false; scope.changeDocCategory(); ("#addModal").modal("hide");
//                    toastr.success(data.Message);
//                } else { $scope.loading = false; toastr.error(data.Message); }
//            }).error(function (data) { $scope.loading = false; toastr.error("Update Failed."); });
//        } else {
//            // Save
//            $http.post("/DocScanningModule/DocType/AddDocType/", JSON.stringify(convArrToObj($scope.saveModel))).success(function (data) {
//                if (data._respStatus.Status === "1") {
//                    $scope.loading = false; $scope.changeDocCategory(); $("#addModal").modal("hide");
//                    toastr.success(data.Message);
//                } else { $scope.loading = false; toastr.error(data.Message); }
//            })
//                .error(function (data) { $scope.loading = false; toastr.error("Save Failed."); });
//        }
//    }
//    // Refresh Documnet Type Table
//    $scope.toggleRefreshTable = function () { $scope.loading = true; $scope.changeDocCategory(); };
//}]);