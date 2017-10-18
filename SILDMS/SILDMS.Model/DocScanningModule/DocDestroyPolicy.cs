using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.Model.SecurityModule;

namespace SILDMS.Model.DocScanningModule
{
    public class DSM_DestroyPolicy
    {
        public string DestroyPolicyID { get; set; }
        public string DestroyPolicyDtlID { get; set; }

        public string DestroyID { get; set; }
        public string DestroyDtlID { get; set; }
        public string DestroyOf { get; set; }
        public string DestroyDate { get; set; }

        public string PolicyFor { get; set; }
        public string DocumentNature { get; set; }
        public string PolicyApplicableTo { get; set; }

        public string OwnerID { get; set; }
        public string DocCategoryID { get; set; }
        public string DocTypeID { get; set; }
        public string DocPropertyID { get; set; }
        public string DocPropertyName { get; set; }
        public string DocPropIdentifyID { get; set; }
        public int UserLevel { get; set; }
        public string UserID { get; set; }
        public string Remarks { get; set; }
        public string SetOn { get; set; }
        public string SetBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public int Status { get; set; }
        public int PolicyStatus { get; set; }
        public int PolicyDetailStatus { get; set; }
        public bool IsSelected { get; set; }

        public string TimeValue { get; set; }
        public string TimeUnit { get; set; }
        public string ExceptionValue { get; set; }
        public string UploaderIP { get; set; }


        public IList<DocCategoryModel> DocCategoryModel { get; set; }
        public IList<DocTypeModel> DocTypeModel { get; set; }
        public IList<DocPropertyModel> DocPropertyModel { get; set; }
        public IList<DocPropIdentityModel> DocPropIdentityModel { get; set; }

    }

    public class DSM_DestroyPolicyDtl
    {
        public string DestroyPolicyDtlID { get; set; }
        public string DestroyPolicyID { get; set; }
       

        public string OwnerID { get; set; }
        public string DocCategoryID { get; set; }
        public string DocTypeID { get; set; }
        public string DocPropertyID { get; set; }
        public string DocPropertyName { get; set; }
        public string DocPropIdentifyID { get; set; }


        public string MetaValue { get; set; }
        public int TimeValue { get; set; }
        public string TimeUnit { get; set; }
        public string ExceptionValue { get; set; }
        public int UserLevel { get; set; }






        public string Remarks { get; set; }
        public string SetOn { get; set; }
        public string SetBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public int Status { get; set; }

    }
}
