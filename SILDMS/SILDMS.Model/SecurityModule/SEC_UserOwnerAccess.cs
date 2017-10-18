using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILDMS.Model.SecurityModule
{
    public class SEC_UserOwnerAccess
    {
        public string UserOwnerAccessID { get; set; }
        public string UserID { get; set; }
        public string PermittedOwnerID { get; set; }

        public string OwnerAccessPower { get; set; }
        public string AccessTimeLimit { get; set; }
        public string Remarks { get; set; }

        public string EnableOwnerSecurity { get; set; }
        public string OwnerLevelAccessID { get; set; }

        public string UserLevel { get; set; }
        public string SupervisorLevel { get; set; }

        public string DataLevelAccess { get; set; }
        public string OwnerID { get; set; }
        public string OwnerLevelID { get; set; }
        

        public string SetOn { get; set; }
        public string SetBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public int Status { get; set; }



        public string DocCategoryID { get; set; }
        public string DocTypeID { get; set; }
        public string DocPropertyID { get; set; }
        
        




        public IList<DocCategoryModel> DocCategoryModel { get; set; }
        public IList<DocTypeModel> DocTypeModel { get; set; }
        public IList<DocPropertyModel> DocPropertyModel { get; set; }
        public IList<DocPropIdentityModel> DocPropIdentityModel { get; set; }

        public IList<OwnerModel> OwnerModel { get; set; }

    }

    public class UserWisePermittedOwner
    {
        public string UserOwnerAccessID { get; set; }
        public string OwnerID { get; set; }
        public string OwnerName { get; set; }
        public bool IsSelected { get; set; }
        public string UserLevel { get; set; }
        public string SupervisorLevel { get; set; }
    }

    public class DocCategoryModel
    {
        public string CategoryID { get; set; }

        public string CategoryTime { get; set; }
        public string CategoryRemarks { get; set; }

        public int TimeValue { get; set; }
        public string TimeUnit { get; set; }
        public string ExceptionValue { get; set; }

        public string DestroyPolicyDtlID { get; set; }

    }

    public class DocTypeModel
    {
        public string TypeID { get; set; }
        public string TypeTime { get; set; }
        public string TypeRemarks { get; set; }

        public int TimeValue { get; set; }
        public string TimeUnit { get; set; }
        public string ExceptionValue { get; set; }

        public string DestroyPolicyDtlID { get; set; }

    }

    public class DocPropertyModel
    {
        public string PropertyID { get; set; }
        public string PropertyTime { get; set; }
        public string PropertyRemarks { get; set; }

        public int TimeValue { get; set; }
        public string TimeUnit { get; set; }
        public string ExceptionValue { get; set; }

        public string DestroyPolicyDtlID { get; set; }

    }

    public class DocPropIdentityModel
    {
        public string PropIdentityID { get; set; }
        public string PropIdentityTime { get; set; }
        public string PropIdentityRemarks { get; set; }
        public string PropIdentityMetaValue { get; set; }

        public int TimeValue { get; set; }
        public string TimeUnit { get; set; }
        public string ExceptionValue { get; set; }

        public string DestroyPolicyDtlID { get; set; }

    }

    public class OwnerModel
    {
        public string UserOwnerAccessID { get; set; }
        public string OwnerID { get; set; }
        public string UserLevel { get; set; }
        public string SupervisorLevel { get; set; }
        

    }



    
}
