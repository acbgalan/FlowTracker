export interface QueryParametersInterface {
    searchTerm?: string | null;
    sortBy?: string | null;
    sortDesc: boolean;
    page: number;
    limit: number;
}
