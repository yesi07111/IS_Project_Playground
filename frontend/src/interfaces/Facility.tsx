export interface Facility {
    id: string;
    name: string;
    location: string;
    type: string;
    usagePolicy: string;
    maximumCapacity: number;
}

export interface ListFacilityResponse {
    result: Facility[] | string[];
}
