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
    reservationId: string;
    firstName: string;
    lastName: string;
    userName: string;
    activityId: string;
    activityName: string;
    activityDate: string;
    comments: string;
    amount: number;
    state: string;
    activityRecommendedAge: number;
}

export interface ListReservationResponse {
    result: ReservationDto[];
}

export interface ListReservationStatsResponse {
    result: object[];
}

export interface UpdateReservationData {
    reservationId: string;
    state: string;
}