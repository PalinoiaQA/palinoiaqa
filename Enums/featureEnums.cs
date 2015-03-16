using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enums
{    
    /// <summary>
    /// class to hold code for featureEnums
    /// </summary>
    public static class featureEnums
    {
        /// <summary>
        /// constructor for feature 
        /// </summary>
        public enum Feature : int
        {
            /// <summary>
            /// value for administration menu item
            /// </summary>
            AdministrationMenuItem = 1,
            /// <summary>
            /// value for business rule menu item
            /// </summary>
            BusinessRulesMenuItem = 2,
            /// <summary>
            /// value for customer service message menu item
            /// </summary>
            CustomerServiceMessagesMenuItem = 3,
            /// <summary>
            /// value for test cases menu item
            /// </summary>
            TestCasesMenuItem = 4,
            /// <summary>
            /// value for General menu item
            /// </summary>
            GeneralMenuItem = 5,
            /// <summary>
            /// value for defect tracting menu item
            /// </summary>
            DefectTrackingMenuItem = 6,
            /// <summary>
            /// value for reports menu item
            /// </summary>
            ReportsMenuItem = 7,
            /// <summary>
            /// value for admin general view
            /// </summary>
            AdminGeneralView = 8,
            /// <summary>
            /// value for admin general add
            /// </summary>
            AdminGeneralAdd = 9,
            /// <summary>
            /// value for admin general edit
            /// </summary>
            AdminGeneralEdit = 10,
            /// <summary>
            /// value for admin general delete
            /// </summary>
            AdminGeneralDelete = 11,
            /// <summary>
            /// value for admin project view
            /// </summary>
            AdminProjectsView = 12,
            /// <summary>
            /// value for admin project add
            /// </summary>
            AdminProjectsAdd = 13,
            /// <summary>
            /// value for admin project edit
            /// </summary>
            AdminProjectsEdit = 14,
            /// <summary>
            /// value for admin project delete
            /// </summary>
            AdminProjectsDelete = 15,
            /// <summary>
            /// value for admin status view
            /// </summary>
            AdminStatusView = 16,
            /// <summary>
            /// value for admin status add
            /// </summary>
            AdminStatusAdd = 17,
            /// <summary>
            /// value for admin status edit
            /// </summary>
            AdminStatusEdit = 18,
            /// <summary>
            /// value for admin status delete
            /// </summary>
            AdminStatusDelete = 19,
            /// <summary>
            /// value for admin CSM response types view
            /// </summary>
            AdminCSMResponseTypesView = 20,
            /// <summary>
            /// value for admin CSM response types add
            /// </summary>
            AdminCSMResponseTypesAdd = 21,
            /// <summary>
            /// value for admin CSM response types edit
            /// </summary>
            AdminCSMResponseTypesEdit = 22,
            /// <summary>
            /// value for admin CSM response types delete
            /// </summary>
            AdminCSMResponseTypesDelete = 23,
            /// <summary>
            /// value for admin CSM types view
            /// </summary>
            AdminCSMTypesView = 24,
            /// <summary>
            /// value for admin CSM types add
            /// </summary>
            AdminCSMTypesAdd = 25,
            /// <summary>
            /// value for admin CSM types edit
            /// </summary>
            AdminCSMTypesEdit = 26,
            /// <summary>
            /// value for admin CSM types delete
            /// </summary>
            AdminCSMTypesDelete = 27,
            /// <summary>
            /// value for admin users view
            /// </summary>
            AdminUsersView = 28,
            /// <summary>
            /// value for admin users add
            /// </summary>
            AdminUsersAdd = 29,
            /// <summary>
            /// value for admin users edit
            /// </summary>
            AdminUsersEdit = 30,
            /// <summary>
            /// value for admin users delete
            /// </summary>
            AdminUsersDelete = 31,
            /// <summary>
            /// value for admin roles view
            /// </summary>
            AdminRolesView = 32,
            /// <summary>
            /// value for admin roles add
            /// </summary>
            AdminRolesAdd = 33,
            /// <summary>
            /// value for admin roles edit
            /// </summary>
            AdminRolesEdit = 34,
            /// <summary>
            /// value for admin roles delete
            /// </summary>
            AdminRolesDelete = 35,
            /// <summary>
            /// value for admin features view
            /// </summary>
            AdminFeaturesView = 36,
            /// <summary>
            /// value for admin features add
            /// </summary>
            AdminFeaturesAdd = 37,
            /// <summary>
            /// value for admin features edit
            /// </summary>
            AdminFeaturesEdit = 38,
            /// <summary>
            /// value for admin features delete
            /// </summary>
            AdminFeaturesDelete = 39,
            /// <summary>
            /// value for admin section view
            /// </summary>
            AdminSectionsView = 40,
            /// <summary>
            /// value for admin section add
            /// </summary>
            AdminSectionsAdd = 41,
            /// <summary>
            /// value for admin section edit
            /// </summary>
            AdminSectionsEdit = 42,
            /// <summary>
            /// value foradminsectiondelete
            /// </summary>
            AdminSectionsDelete = 43,
            /// <summary>
            /// value for business rule view
            /// </summary>
            BusinessRulesView = 44,
            /// <summary>
            /// value for business rule add
            /// </summary>
            BusinessRulesAdd = 45,
            /// <summary>
            /// value for business rule edit
            /// </summary>
            BusinessRulesEdit = 46,
            /// <summary>
            /// value for business rule delete
            /// </summary>
            BusinessRulesDelete = 47,
            /// <summary>
            /// value for admin role feature view
            /// </summary>
            AdminRoleFeaturesView = 48,
            /// <summary>
            /// value for admin role feature edit
            /// </summary>
            AdminRoleFeaturesEdit = 49,
            /// <summary>
            /// value for customer service message view
            /// </summary>
            CustomerServiceMessagesView = 50,
            /// <summary>
            /// value for customer service message add
            /// </summary>
            CustomerServiceMessagesAdd = 51,
            /// <summary>
            /// value for customer service message edit
            /// </summary>
            CustomerServiceMessagesEdit = 52,
            /// <summary>
            /// value for customer service message delete
            /// </summary>
            CustomerServiceMessagesDelete = 53,
            /// <summary>
            /// value for test cases view
            /// </summary>
            TestCasesView = 54,
            /// <summary>
            /// value for test cases add
            /// </summary>
            TestCasesAdd = 55,
            /// <summary>
            /// value for test cases edit
            /// </summary>
            TestCasesEdit = 56,
            /// <summary>
            /// value for test cases delete
            /// </summary>
            TestCasesDelete = 57,
            /// <summary>
            /// value for test step view
            /// </summary>
            TestStepsView = 58,
            /// <summary>
            /// value for test steps add
            /// </summary>
            TestStepsAdd = 59,
            /// <summary>
            /// value for test steps edit
            /// </summary>
            TestStepsEdit = 60,
            /// <summary>
            /// value for test steps delete
            /// </summary>
            TestStepsDelete = 61,
            /// <summary>
            /// value for preconditions edit
            /// </summary>
            PreConditionsEdit = 63,
            /// <summary>
            /// value for preconditions add
            /// </summary>
            PreConditionsAdd = 64,
            /// <summary>
            /// value for preconditions delete
            /// </summary>
            PreConditionsDelete = 65,
            /// <summary>
            /// value for document type view
            /// </summary>
            DocumentTypeView = 70,
            /// <summary>
            /// value for document type add
            /// </summary>
            DocumentTypeAdd = 71,
            /// <summary>
            /// value for document type edit
            /// </summary>
            DocumentTypeEdit = 72,
            /// <summary>
            /// value for document type delete
            /// </summary>
            DocumentTypeDelete = 73,
            /// <summary>
            /// value for documents view
            /// </summary>
            DocumentsView = 74,
            /// <summary>
            /// value for documents add
            /// </summary>
            DocumentsAdd = 75,
            /// <summary>
            /// value for documents edit
            /// </summary>
            DocumentsEdit = 76,
            /// <summary>
            /// value for documents delete
            /// </summary>
            DocumentsDelete = 77,
            /// <summary>
            /// value for chapter types view
            /// </summary>
            ChapterTypesView = 78,
            /// <summary>
            /// value for chapter types add
            /// </summary>
            ChapterTypesAdd = 79,
            /// <summary>
            /// value for chapter types edit
            /// </summary>
            ChapterTypesEdit = 80,
            /// <summary>
            /// value for chapter types delete
            /// </summary>
            ChapterTypesDelete = 81,
            /// <summary>
            /// value for admin defect type add
            /// </summary>
            AdminDefectTypeAdd = 83,
            /// <summary>
            /// value for admin defect type view
            /// </summary>
            AdminDefectTypeView = 84,
            /// <summary>
            /// value for admin defect type edit
            /// </summary>
            AdminDefectTypeEdit = 85,
            /// <summary>
            /// value for admin defect type delete
            /// </summary>
            AdminDefectTypeDelete = 86,
            /// <summary>
            /// value for defect status add
            /// </summary>
            AdminDefectStatusAdd = 87,
            /// <summary>
            /// value for defect status view
            /// </summary>
            AdminDefectStatusView = 88,
            /// <summary>
            /// value for admin defect status edit
            /// </summary>
            AdminDefectStatusEdit = 89,
            /// <summary>
            /// value for admin defect status delete
            /// </summary>
            AdminDefectStatusDelete = 90,
            /// <summary>
            /// value for admin defect priority add
            /// </summary>
            AdminDefectPriorityAdd = 91,
            /// <summary>
            /// value for admin defect priority view
            /// </summary>
            AdminDefectPriorityView = 92,
            /// <summary>
            /// value for admin defect priority edit
            /// </summary>
            AdminDefectPriorityEdit = 93,
            /// <summary>
            /// value for admin defect priority delete
            /// </summary>
            AdminDefectPriorityDelete = 94,
            /// <summary>
            /// value for chapter add
            /// </summary>
            ChapterAdd = 95,
            /// <summary>
            /// value for chapter edit
            /// </summary>
            ChapterEdit = 96,
            /// <summary>
            /// value for chapter delete
            /// </summary>
            ChapterDelete = 97,
            /// <summary>
            /// value for chapter view
            /// </summary>
            ChapterView = 98,
            /// <summary>
            /// value for users menu item
            /// </summary>
            UsersMenuItem = 99,
            /// <summary>
            /// value for users users menu item
            /// </summary>
            UsersUsersMenuItem = 100,
            /// <summary>
            /// value for users features menu item
            /// </summary>
            UsersFeaturesMenuItem = 101,
            /// <summary>
            /// value for users roles menu item
            /// </summary>
            UsersRolesMenuItem = 105,
            /// <summary>
            /// value for defect priority menu item
            /// </summary>
            DefectPriorityMenuItem = 106,
            /// <summary>
            /// value for defect types menu item
            /// </summary>
            DefectTypesMenuItem = 107,
            /// <summary>
            /// value for defect status menu item
            /// </summary>
            DefectStatusMenuItem = 108,
            /// <summary>
            /// value for projects menu item
            /// </summary>
            ProjectsMenuItem = 109,
            /// <summary>
            /// value for admin documents menu item
            /// </summary>
            AdminDocumentsMenuItem = 110,
            /// <summary>
            /// value for admin chapter types menu item
            /// </summary>
            AdminChapterTypesMenuItem = 111,
            /// <summary>
            /// value for admin document types menu item
            /// </summary>
            AdminDocumenttypesMenuItem = 112,
            /// <summary>
            /// value for error log menu item
            /// </summary>
            ErrorLogMenuItem = 113,
            /// <summary>
            /// value for sections menu item
            /// </summary>
            SectionsMenuItem = 114,
            /// <summary>
            /// value for CSM response types menu item
            /// </summary>
            CSMResponseTypesMenuItem = 115,
            /// <summary>
            /// value for CSM types menu item
            /// </summary>
            CSMTypesMenuItem = 116,
            /// <summary>
            /// value for business rules status menu item
            /// </summary>
            BusinessRulesStatusMenuItem = 117,
            /// <summary>
            /// value for test steps menu item
            /// </summary>
            TestStepsMenuItem = 118,
            /// <summary>
            /// value for admin business rules menu item
            /// </summary>
            AdminBusinessRulesMenuItem = 119,
            /// <summary>
            /// value for admin defect menu item
            /// </summary>
            AdminDefectsMenuItem = 120,
            /// <summary>
            /// value for admin customer service messages menu item
            /// </summary>
            AdminCustomerServiceMessagesMenuItem = 121,
            /// <summary>
            /// value for document manager menu item
            /// </summary>
            DocumentManagerMenuItem = 122,
            /// <summary>
            /// id for defects view feature
            /// </summary>
            DefectsView = 123,
            /// <summary>
            /// id for defects add feature
            /// </summary>
            DefectsAdd = 124,
            /// <summary>
            /// id for defects edit feature
            /// </summary>
            DefectsEdit = 125,
            /// <summary>
            /// id for defects delete feature
            /// </summary>
            DefectsDelete = 126
        };
    }
}
