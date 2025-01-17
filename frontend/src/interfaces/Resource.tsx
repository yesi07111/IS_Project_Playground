export interface Resource {
    id: string;
    name: string;
    type: string;
    useFrequency: number;
    condition: string;
    facilityName: string;
    facilityLocation: string;
    facilityType: string;
}

export interface ListResourceResponse{
    result: object[];
}