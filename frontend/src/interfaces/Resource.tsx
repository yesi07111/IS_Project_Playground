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

export interface ResourceFormData{
    name: string;
    type: string;
    resourceCondition: string;
    facilityId: string;
}

export interface UpdateResourceFormData{
    id: string;
    name: string;
    type: string;
    resourceCondition: string;
    facilityId: string;
}

export interface ResourceCardProps{
    resource: Resource;
}

export interface ResourceDate {
    id: string;
    name: string;
    date: Date;
    useFrequency: number;
}

export interface ListResourceDateResponse{
    result: ResourceDate[];
}