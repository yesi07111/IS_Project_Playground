export interface ReservationFormData {
    amount: number;
    comments: string;
    userId: string;
    activityId: string;
}

export interface ReservationCreationResponse {
    success: boolean;
}

export interface ReservationDto {
    activityId: string;
    activityName: string;
    comments: string;
    amount: number;
    state: string;
}

export interface ListReservationResponse {
    result: ReservationDto[];
}