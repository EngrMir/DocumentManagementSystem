// Controller for Document Category Page
//app.controller('docCategoryCtrl', ["$scope", "$http", function ($scope, $http) {
//    // Model for Document Category
//    $scope.docCategory = {
//        DocCategoryID: "", OwnerLevel: { OwnerLevelID: "", LevelName: "" }, Owner: { OwnerID: "", OwnerName: "" },
//        DocCategorySL: "", UDDocCategoryCode: "", DocCategoryName: "", Status: ""
//    };
//    // Model for the data to be saved/updated
//    $scope.saveModel = [];
//    $scope.itemsByPage = 10;
//    $scope.loading = true;
//    // Get all owner levels
//    $http.get('/DocScanningModule/DocCategory/GetOwnerLevel?id=')
//        .success(function (response) { $scope.ownerLevels = response.result; })
//        .error(function () { });
//    // Get all owners for the selected owner level on owner level dropdown change
//    $scope.changeOwnerLevel = function () {
//        $http.get('/DocScanningModule/DocCategory/GetOwners?id=' + $scope.docCategory.OwnerLevel.OwnerLevelID)
//            .success(function (response) { $scope.owners = response.result; $scope.docCategories = []; $scope.loading = true; })
//            .error(function () { });
//    }
//    // Get all document categories for the selected owner on owner drop down change
//    $scope.changeOwner = function () {
//        $http.get('/DocScanningModule/DocCategory/GetDocCategoryForOwner?id=' + $scope.docCategory.Owner.OwnerID)
//            .success(function (response) { $scope.docCategories = response.result; $scope.loading = false; })
//            .error(function () { });
//    };
//    // Add new Document Category
//    $scope.toggleAdd = function () {
//        $scope.docCategory.DocCategoryID = ""; $scope.docCategory.DocCategorySL = ""; $scope.docCategory.UDDocCategoryCode = "";
//        $scope.docCategory.DocCategoryName = ""; $scope.docCategory.Status = ""; $('#addModal').modal('show');
//    };
//    // Edit Document Category
//    $scope.toggleEdit = function (row) {
//        $scope.docCategory.DocCategoryID = row.DocCategoryID; $scope.docCategory.DocCategorySL = row.DocCategorySL;
//        $scope.docCategory.UDDocCategoryCode = row.UDDocCategoryCode; $scope.docCategory.DocCategoryName = row.DocCategoryName;
//        $scope.docCategory.Status = (row.Status).toString(); $('#addModal').modal('show');
//    };
//    // Convert current model to save model
//    $scope.modelConvert = function () {
//        $scope.saveModel.DocCategoryID = $scope.docCategory.DocCategoryID; $scope.saveModel.OwnerID = $scope.docCategory.Owner.OwnerID;
//        $scope.saveModel.DocCategorySL = $scope.docCategory.DocCategorySL; $scope.saveModel.UDDocCategoryCode = $scope.docCategory.UDDocCategoryCode;
//        $scope.saveModel.DocCategoryName = $scope.docCategory.DocCategoryName; $scope.saveModel.Status = $scope.docCategory.Status;
//    };
//    // Save/Update Document Category
//    $scope.Save = function () {
//        $scope.loading = true; $scope.modelConvert();
//        if ($scope.docCategory.DocCategoryID) {
//            // Update
//            $http.post('/DocScanningModule/DocCategory/EditDocCategory/', JSON.stringify(convArrToObj($scope.saveModel)))
//                .success(function (data) {
//                    if (data._respStatus.Status === "1") {
//                        $scope.loading = false; $('#addModal').modal('hide'); $scope.changeOwner(); toastr.success(data.Message);
//                    } else { scope.loading = false; toastr.error(data.Message); }
//                })
//                .error(function (data) { $scope.loading = false; toastr.error('Update Faild.'); });
//        } else {
//            // Save
//            $http.post('/DocScanningModule/DocCategory/AddDocCategory/', JSON.stringify(convArrToObj($scope.saveModel)))
//                .success(function (data) {
//                    if (data._respStatus.Status === "1") {
//                        $scope.loading = false; $scope.changeOwner(); $('#addModal').modal('hide'); toastr.success(data.Message);
//                    } else { $scope.loading = false; toastr.error(data.Message); }
//                })
//                .error(function (data) { $scope.loading = false; toastr.error('Saved Faild.'); });
//        }
//    }
//    // Refresh Documnet Category Table
//    $scope.toggleRefreshTable = function () { $scope.loading = true; $scope.changeOwner(); };
//}]);