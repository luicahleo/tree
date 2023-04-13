
public static class ServicesCodes
{
    private static class TBO_ID
    {
        public const string GENERIC = "TBO_001";
        public const string SITES = "TBO_002";
        public const string INVENTORY_ELEMENT = "TBO_003";
        public const string MAINTENANCE_INCIDENT = "TBO_004";
    }
    
    public static class GENERIC
    {
        public const string COD_TBO_SUCCESS_CODE = TBO_ID.GENERIC + "_001";
        public const string COD_TBO_EXCEPTION_CODE = TBO_ID.GENERIC + "_002";
        public const string COD_TBO_MISSING_DATA_CODE = TBO_ID.GENERIC + "_003";
        public const string COD_TBO_OBJECT_NOT_FOUND_CODE = TBO_ID.GENERIC + "_004";
        public const string COD_TBO_OBJECT_NOT_CREATION_CODE = TBO_ID.GENERIC + "_005";
        public const string COD_TBO_USER_NOT_FOUND_CODE = TBO_ID.GENERIC + "_006";
        public const string COD_TBO_CLIENT_NOT_FOUND_CODE = TBO_ID.GENERIC + "_007";
        public const string COD_TBO_PROYECT_NOT_FOUND_CODE = TBO_ID.GENERIC + "_008";
        public const string COD_TBO_AGENCY_NOT_FOUND_CODE = TBO_ID.GENERIC + "_009";
        public const string COD_TBO_SEVERITY_NOT_FOUND_CODE = TBO_ID.GENERIC + "_010";
        public const string COD_TBO_TYPOLOGY_NOT_FOUND_CODE = TBO_ID.GENERIC + "_011";
        public const string COD_TBO_CONFLICT_LEVEL_NOT_FOUND_CODE = TBO_ID.GENERIC + "_012";
        public const string COD_TBO_SITE_NOT_FOUND_CODE = TBO_ID.GENERIC + "_013";
        public const string COD_TBO_CUSTOMER_WITH_CONEXION_CODE_NOT_FOUND_CODE = TBO_ID.GENERIC + "_014";
        public const string COD_TBO_LENGTH_DATA_EXCEEDS_CODE = TBO_ID.GENERIC + "_015";
        public const string COD_TBO_INCORRECT_DATE_FORMAT_CODE = TBO_ID.GENERIC + "_016";
        public const string COD_TBO_CUSTOMER_NOT_FOUND_CODE = TBO_ID.GENERIC + "_017";
        public const string COD_TBO_CURRENCY_NOT_FOUND_CODE = TBO_ID.GENERIC + "_018";
        public const string COD_TBO_LOCATION_NOT_FOUND_CODE = TBO_ID.GENERIC + "_019";
        public const string COD_TBO_RANGE_DAYS_NOT_VALID_CODE = TBO_ID.GENERIC + "_20";


        public const string COD_TBO_SUCCESS_DESCRIPTION = "The transaction was performed successfully";
        public const string COD_TBO_EXCEPTION_DESCRIPTION = "An exception occurs when managing the request: ";
        public const string COD_TBO_MISSING_DATA_DESCRIPTION = "Some data was missing";
        public const string COD_TBO_OBJECT_NOT_FOUND_DESCRIPTION = "Object not found";
        public const string COD_TBO_OBJECT_NOT_CREATION_DESCRIPTION = "It was not possible to create the object";
        public const string COD_TBO_USER_NOT_FOUND_DESCRIPTION = "User not found";
        public const string COD_TBO_CLIENT_NOT_FOUND_DESCRIPTION = "Client not found";
        public const string COD_TBO_PROYECT_NOT_FOUND_DESCRIPTION = "Proyect not found";
        public const string COD_TBO_AGENCY_NOT_FOUND_DESCRIPTION = "Agency not found";
        public const string COD_TBO_SEVERITY_NOT_FOUND_DESCRIPTION = "Severity not found";
        public const string COD_TBO_WORKFLOW_TYPE_NOT_FOUND_DESCRIPTION = "WorkFlow type not found";
        public const string COD_TBO_CONFLICT_LEVEL_NOT_FOUND_DESCRIPTION = "Conflict level not found";
        public const string COD_TBO_SITE_NOT_FOUND_DESCRIPTION = "Site not found";
        public const string COD_TBO_CUSTOMER_WITH_CONEXION_CODE_NOT_FOUND_DESCRIPTION = "There is no operator with this conection code";
        public const string COD_TBO_LENGTH_DATA_EXCEEDS_DESCRIPTION = "The length of the fields has been exceeded: ";
        public const string COD_TBO_INCORRECT_DATE_FORMAT_DESCRIPTION = "Date format is incorrect: ";
        public const string COD_TBO_CUSTOMER_NOT_FOUND_DESCRIPTION = "Customer not found";
        public const string COD_TBO_CURRENCY_NOT_FOUND_DESCRIPTION = "Currency not found";
        public const string COD_TBO_LOCATION_NOT_FOUND_DESCRIPTION = "Location not found: ";
        public const string COD_TBO_RANGE_DAYS_NOT_VALID_DESCRIPTION = "Number of days not valid";
    }

    public static class SITES
    {
        public const string COD_TBO_GLOBAL_STATE_NOT_FOUND_CODE = TBO_ID.SITES + "_001";
        public const string COD_TBO_CATEGORY_NOT_FOUND_CODE = TBO_ID.SITES + "_002";
        public const string COD_TBO_SITE_TYPE_NOT_FOUND_CODE = TBO_ID.SITES + "_003";
        public const string COD_TBO_STRUCTURE_TYPE_NOT_FOUND_CODE = TBO_ID.SITES + "_004";
        public const string COD_TBO_BUILDING_TYPE_NOT_FOUND_CODE = TBO_ID.SITES + "_005";
        public const string COD_TBO_SIZE_NOT_FOUND_CODE = TBO_ID.SITES + "_006";
        public const string COD_TBO_SITE_EXISTS_CODE = TBO_ID.SITES + "_007";
        public const string COD_TBO_ERROR_WHEN_UPDATING_CODE = TBO_ID.SITES + "_008";
        public const string COD_TBO_ERROR_WHEN_SAVING_CODE = TBO_ID.SITES + "_009";
        public const string COD_TBO_ATTRIBUTE_CATEGORY_NOT_FOUND_CODE = TBO_ID.SITES + "_010";
        public const string COD_TBO_ATTRIBUTES_NOT_FOUND_CODE = TBO_ID.SITES + "_011";
        public const string COD_TBO_MANDATORY_ATTRIBUTES_MISSING_CODE = TBO_ID.SITES + "_012";
        public const string COD_TBO_ATTRIBUTES_INVALID_ATTRIBUTES_CODE = TBO_ID.SITES + "_013";

        public const string COD_TBO_GLOBAL_STATE_NOT_FOUND_DESCRIPTION = "Global state not found";
        public const string COD_TBO_CATEGORY_NOT_FOUND_DESCRIPTION = "Category not found";
        public const string COD_TBO_SITE_TYPE_NOT_FOUND_DESCRIPTION = "Site type not found";
        public const string COD_TBO_STRUCTURE_TYPE_NOT_FOUND_DESCRIPTION = "Structure type not found";
        public const string COD_TBO_BUILDING_TYPE_NOT_FOUND_DESCRIPTION = "Building type not found";
        public const string COD_TBO_SIZE_NOT_FOUND_DESCRIPTION = "Size not found";
        public const string COD_TBO_SITE_EXISTS_DESCRIPTION = "Site already exists";
        public const string COD_TBO_ERROR_WHEN_UPDATING_DESCRIPTION = "Error when updating";
        public const string COD_TBO_ERROR_WHEN_SAVING_DESCRIPTION = "Error when saving";
        public const string COD_TBO_ATTRIBUTE_CATEGORY_NOT_FOUND_DESCRIPTION = "Attribute category not found";
        public const string COD_TBO_ATTRIBUTES_NOT_FOUND_DESCRIPTION = "Attributes not found: ";
        public const string COD_TBO_MANDATORY_ATTRIBUTES_MISSING_DESCRIPTION = "Mandatory attributes are missing: ";
        public const string COD_TBO_ATTRIBUTES_INVALID_ATTRIBUTES_DESCRIPTION = "Invalid attributes: ";
    }

    public static class INVENTORY_ELEMENT
    {
        public const string COD_TBO_INVENTORY_CODE_NOT_FOUND_CODE = TBO_ID.INVENTORY_ELEMENT + "_001";
        public const string COD_TBO_CATEGORY_ATTRIBUTE_NOT_FOUND_CODE = TBO_ID.INVENTORY_ELEMENT + "_002";
        public const string COD_TBO_CATEGORY_ELEMENT_NOT_FOUND_CODE = TBO_ID.INVENTORY_ELEMENT + "_003";
        public const string COD_TBO_OPERATOR_NOT_FOUND_CODE = TBO_ID.INVENTORY_ELEMENT + "_004";
        public const string COD_TBO_INVENTORY_ELEMENT_CODE_DUPLICATE = TBO_ID.INVENTORY_ELEMENT + "_005";
        public const string COD_TBO_STATUS_NOT_FOUND_CODE = TBO_ID.INVENTORY_ELEMENT + "_006";
        public const string COD_TBO_CATEGORY_ELEMENT_ATTRIBUTE_NOT_FOUND_CODE = TBO_ID.INVENTORY_ELEMENT + "_007";
        public const string COD_TBO_CATEGORY_NOT_FOUND_CODE = TBO_ID.INVENTORY_ELEMENT + "_008";
        public const string COD_TBO_TEMPLATE_NOT_FOUND_CODE = TBO_ID.INVENTORY_ELEMENT + "_009";
        public const string COD_TBO_TEMPLATE_NOT_ALLOW_PARENT_CATEGORY_CODE = TBO_ID.INVENTORY_ELEMENT + "_010";
        public const string COD_TBO_ELEMENTS_IN_DIFFERENT_SITES_CODE = TBO_ID.INVENTORY_ELEMENT + "_011";
        public const string COD_TBO_ALREADY_LINK_BETWEEN_ELEMENTS_CODE = TBO_ID.INVENTORY_ELEMENT + "_012";
        public const string COD_TBO_LINKAGE_NOT_ALLOWED_CODE = TBO_ID.INVENTORY_ELEMENT + "_013";
        public const string COD_TBO_ATRIBUTTE_INVALID_FORMAT_CODE = TBO_ID.INVENTORY_ELEMENT + "_014";
        public const string COD_TBO_CATEGORY_TEMPLATE_NOT_FOUND_CODE = TBO_ID.INVENTORY_ELEMENT + "_015";
        public const string COD_TBO_REQUIRED_TEMPLATE_CODE = TBO_ID.INVENTORY_ELEMENT + "_016";
        public const string COD_TBO_CATEGORY_ATRIBUTTE_NOT_FOUND_CODE = TBO_ID.INVENTORY_ELEMENT + "_017";
        public const string COD_TBO_ERROR_CARDINALITY_CODE = TBO_ID.INVENTORY_ELEMENT + "_018";



        public const string COD_TBO_INVENTORY_CODE_NOT_FOUND_DESCRIPTION = "Inventory code not found";
        public const string COD_TBO_CATEGORY_ATTRIBUTE_NOT_FOUND_DESCRIPTION = "Category attribute not found";
        public const string COD_TBO_CATEGORY_ELEMENT_NOT_FOUND_DESCRIPTION = "Category inventory not found";
        public const string COD_TBO_OPERATOR_NOT_FOUND_DESCRIPTION = "There is no customer with this code";
        public const string COD_TBO_INVENTORY_ELEMENT_DUPLICATE_DESCRIPTION = "Inventory Element Duplicate";
        public const string COD_TBO_STATUS_NOT_FOUND_DESCRIPTION = "Status inventory not found";
        public const string COD_TBO_CATEGORY_ELEMENT_ATTRIBUTE__NOT_FOUND_DESCRIPTION = "The Attribute does not correspond to the category of the element";
        public const string COD_TBO_CATEGORY_NOT_FOUND_DESCRIPTION = "The Category can not be changed";
        public const string COD_TBO_TEMPLATE_NOT_FOUND_DESCRIPTION = "Template not found";
        public const string COD_TBO_TEMPLATE_NOT_ALLOW_PARENT_CATEGORY_DESCRIPTION = "Parent element category not allow";
        public const string COD_TBO_ELEMENTS_IN_DIFFERENT_SITES_DESCRIPTION = "Elements in different sites";
        public const string COD_TBO_ALREADY_LINK_BETWEEN_ELEMENTS_DESCRIPTION = "There is already a link between the elements";
        public const string COD_TBO_LINKAGE_NOT_ALLOWED_DESCRIPTION = "Linkage not allowed";
        public const string COD_TBO_ATRIBUTTE_INVALID_FORMAT_DESCRIPTION = "Invalid Attribute";
        public const string COD_TBO_CATEGORY_TEMPLATE_NOT_FOUND_DESCRIPTION = "Category Template not found";
        public const string COD_TBO_REQUIRED_TEMPLATE_DESCRIPTION = "Templates are missing for some required attributes";
        public const string COD_TBO_CATEGORY_ATRIBUTTE_NOT_FOUND_DESCRIPTION = "Form section not found";
        public const string COD_TBO_ERROR_CARDINALITY_DESCRIPTION = "The cardinality of this relation does not allow this relation";
    }

    public static class MAINTENANCE_INCIDENT
    {
        public const string COD_TBO_NAME_INCIDENCE_EXISTS_CODE = TBO_ID.MAINTENANCE_INCIDENT + "_001";
        public const string COD_TBO_INCIDENCE_IS_IN_PROCESS_CODE = TBO_ID.MAINTENANCE_INCIDENT + "_002";
        public const string COD_TBO_INCIDENCE_CANCEL_CODE = TBO_ID.MAINTENANCE_INCIDENT + "_003";
        public const string COD_TBO_CATEGORY_NOT_FOUND_CODE = TBO_ID.MAINTENANCE_INCIDENT + "_004";
        public const string COD_TBO_INCIDENCE_NOT_FOUND_CODE = TBO_ID.MAINTENANCE_INCIDENT + "_005";
        public const string COD_TBO_ELEMENT_INVENTORY_NOT_FOUND_CODE = TBO_ID.MAINTENANCE_INCIDENT + "_006";


        public const string COD_TBO_NAME_INCIDENCE_EXISTS_DESCRIPTION = "Incidece name already exists";
        public const string COD_TBO_INCIDENCE_IS_IN_PROCESS_DESCRIPTION = "The incidece is in process";
        public const string COD_TBO_INCIDENCE_CANCEL_DESCRIPTION = "The incidece is canceled";
        public const string COD_TBO_CATEGORY_NOT_FOUND_DESCRIPTION = "Category not found";
        public const string COD_TBO_INCIDENCE_NOT_FOUND_DESCRIPTION = "Incidence not found";
        public const string COD_TBO_ELEMENT_INVENTORY_NOT_FOUND_DESCRIPTION = "Element inventory not found";
    }
}
